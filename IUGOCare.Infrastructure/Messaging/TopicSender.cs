using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Polly;

namespace IUGOCare.Infrastructure.Messaging
{
    public class TopicSender : IServiceBusSender
    {
        private ITopicClient _topicClient;
        private ILogger<TopicSender> _logger;
        private IMessagingDbContextFactory _contextFactory;
        private IDateTimeOffset _dateTimeOffset;

        BlockingCollection<Outbox> _queue = new BlockingCollection<Outbox>();
        private readonly ManualResetEvent _isEmpty = new ManualResetEvent(false);
        private readonly string _connectionString;
        private readonly string _topic;

        public TopicSender(
            string connectionString,
            string topic,
            ILogger<TopicSender> logger,
            IMessagingDbContextFactory contextFactory,
            IDateTimeOffset dateTimeOffset)
        {
            _connectionString = connectionString;
            _topic = topic;
            _logger = logger;
            _contextFactory = contextFactory;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task SendMessageAsync(string destinationSubdomain, string eventName, object messageBody, bool expectsAcknowledgement = true)
        {
            try
            {
                var dto = new Outbox
                {
                    CorrelationId = destinationSubdomain,
                    Label = eventName,
                    MessageBody = JsonSerializer.Serialize(messageBody),
                    ExpectsAcknowledgement = expectsAcknowledgement,
                    DateStaged = _dateTimeOffset.UtcNow,
                };

                await SaveToOutbox(dto);
                _queue.Add(dto);
            }
            catch(Exception e)
            {
                _logger.LogError($"Error in TopicSender staging or adding message to queue for {eventName} to {destinationSubdomain}: {e}");
            }
        }

        private async Task SaveToOutbox(Outbox message)
        {
            using (var context = _contextFactory.MessagingDbContext)
            {
                await context.Outbox.AddAsync(message);
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _topicClient = new TopicClient(_connectionString, _topic);
                QueueUnsentMessages();
                Task.Factory.StartNew(RunMessageSender, TaskCreationOptions.LongRunning);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error creating new Topic Client: {e}");
            }
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Flush();
            await _topicClient.CloseAsync();
        }

        private void QueueUnsentMessages()
        {
            using (var context = _contextFactory.MessagingDbContext)
            {
                var messages = context.Outbox
                    .Where(m => m.MessageSent == false)
                    .ToList();

                messages.ForEach(m => _queue.Add(m));
            }
        }

        private async void RunMessageSender()
        {
            try
            {
                var retriesAllowed = 3;
                var circuitBreakDuration = TimeSpan.FromMinutes(5);
                var exceptionsAllowedBeforeBreakingCircuit = 1;

                foreach (Outbox message in _queue.GetConsumingEnumerable())
                {
                    var fallbackPolicy = Policy
                        .Handle<Exception>()
                        .FallbackAsync(
                            fallbackAction: (ct) => { return Task.CompletedTask; },
                            onFallbackAsync: (exception) =>
                            {
                                _logger.LogError("Fallback triggered in TopicSender for {0} message with exception {1}. Message has not been sent.", message.Label, exception);
                                return Task.CompletedTask;
                            });

                    var circuitBreakerPolicy = Policy
                        .Handle<Exception>()
                        .CircuitBreakerAsync(
                            exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreakingCircuit,
                            durationOfBreak: circuitBreakDuration,
                            onBreak: (exception, breakDuration) =>
                            {
                                _logger.LogError("TopicSender failed to send {0} message too many times. Breaking circuit for {1} seconds.", message.Label, breakDuration.TotalSeconds);
                            },
                            onReset: () =>
                            {
                                _logger.LogInformation("Resetting circuit breaker in TopicSender.");
                            });

                    var retryPolicy = Policy
                        .Handle<Exception>()
                        .RetryAsync(
                            retryCount: retriesAllowed,
                            onRetry: (exception, retryCount) =>
                            {
                                _logger.LogWarning("TopicSender failed to send {0} message with exception {1}. Times tried: {2} of {3} total.", message.Label, exception, retryCount, retriesAllowed);
                            });

                    await Policy.WrapAsync(fallbackPolicy, circuitBreakerPolicy, retryPolicy).ExecuteAsync(async () => await SendMessageToTopic(message));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected failure sending messages to Service Bus");
            }
            finally
            {
                _isEmpty.Set();
            }
        }

        private async Task SendMessageToTopic(Outbox queuedMessage)
        {
            var message = new Message(Encoding.UTF8.GetBytes(queuedMessage.MessageBody));
            message.CorrelationId = queuedMessage.CorrelationId;
            message.Label = queuedMessage.Label;
            message.UserProperties.Add(MessageHeaders.MessageId, queuedMessage.MessageId);
            message.UserProperties.Add(MessageHeaders.ExpectsAcknowledgement, queuedMessage.ExpectsAcknowledgement);

            await _topicClient.SendAsync(message);
            await SetMessageSent(queuedMessage.MessageId);
        }

        private async Task SetMessageSent(Guid messageId)
        {
            using (var context = _contextFactory.MessagingDbContext)
            {
                try
                {
                    var message = context.Outbox.Find(messageId);
                    if (message is null)
                    {
                        _logger.LogError("Error in TopicSender.SetMessageSent: Could not find Outbox message with MessageId {0}", message.MessageId);
                        return;
                    }

                    message.MessageSent = true;
                    context.Outbox.Update(message);
                    await context.SaveChangesAsync(new CancellationToken());
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception caught in TopicSender.SetMessageSent: {0}", ex);
                }
            }
        }

        private void Flush()
        {
            _queue.CompleteAdding();
            while (!_isEmpty.WaitOne(1000))
            {
                _logger.LogInformation("Waiting for TopicSender queue to empty.");
            }
            _logger.LogInformation("TopicSender queue is empty.");
        }
    }
}
