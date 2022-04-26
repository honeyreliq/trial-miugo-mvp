using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Common.Models;
using IUGOCare.Domain.Entities;
using IUGOCare.Messages.PatientToClinical.Common;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace IUGOCare.Infrastructure.Messaging
{
    public class TopicListener : IServiceBusListener
    {
        private MessageReceiver _messageReceiver;
        private readonly ILogger<TopicListener> _logger;
        private readonly IMessageHandler _messageHandler;
        private readonly IServiceBusSender _messageSender;
        private readonly IMessagingDbContextFactory _contextFactory;
        private readonly ISendEmailService _sendEmailService;

        private readonly string _connectionString;
        private readonly string _topic;
        private readonly string _subscriptionName;
        private readonly string _subscriptionFilter;
        private readonly string _systemFailureNotificationRecipient;

        private const string _messageAcknowledged = "MessageAcknowledged";

        public TopicListener(
            string connectionString,
            string topic,
            string subscriptionName,
            string subscriptionFilter,
            string systemFailureNotificationRecipient,
            ILogger<TopicListener> logger,
            IMessageHandler messageHandler,
            IServiceBusSender messageSender,
            IMessagingDbContextFactory contextFactory,
            ISendEmailService sendEmailService)
        {
            _connectionString = connectionString;
            _topic = topic;
            _subscriptionName = subscriptionName;
            _subscriptionFilter = subscriptionFilter;
            _systemFailureNotificationRecipient = systemFailureNotificationRecipient;
            _logger = logger;
            _messageHandler = messageHandler;
            _messageSender = messageSender;
            _contextFactory = contextFactory;
            _sendEmailService = sendEmailService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RegisterMessageHandlerAndReceiveMessages();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _messageReceiver.CloseAsync();
        }

        private async Task RegisterMessageHandlerAndReceiveMessages()
        {
            try
            {
                await SetupSubscription();

                _messageReceiver = new MessageReceiver(_connectionString, EntityNameHelper.FormatSubscriptionPath(_topic, _subscriptionName));

                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };

                _messageReceiver.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception thrown in TopicListener.RegisterMessageHandlerAndReceiveMessages: {e}");
            }
            
        }

        private async Task SetupSubscription()
        {
            var managementClient = new ManagementClient(_connectionString);

            if (await managementClient.SubscriptionExistsAsync(_topic, _subscriptionName) is false)
            {
                await managementClient.CreateSubscriptionAsync(new SubscriptionDescription(_topic, _subscriptionName), new RuleDescription
                {
                    Filter = new CorrelationFilter($"{_subscriptionFilter}"),
                    Name = $"{_subscriptionFilter}Filter"
                });
            }

            await managementClient.CloseAsync();
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            message.UserProperties.TryGetValue(MessageHeaders.MessageId, out object messageId);
            string messageBody = Encoding.UTF8.GetString(message.Body);

            var model = new Inbox
            {
                MessageId = (Guid)messageId,
                Label = message.Label,
                MessageBody = messageBody
            };

            string errorMessage = null;

            try
            {
                await SaveToInbox(model, cancellationToken);
                await _messageHandler.Handle(message.Label, Encoding.UTF8.GetString(message.Body));
            }
            catch (Exception ex)
            {
                errorMessage = $"{ex.GetType()} caught handling {message.Label} message with message Id {model.MessageId}.";
                _logger.LogError("{0} Full exception: {1}", errorMessage, ex);
            }
            finally
            {
                if (!string.IsNullOrEmpty(errorMessage))
                    await SendFailureNotification(errorMessage);

                await UpdateInbox(model.MessageId, errorMessage, cancellationToken);

                await _messageReceiver.CompleteAsync(message.SystemProperties.LockToken);
                await AcknowledgeMessage(message, errorMessage);
            }
        }

        private async Task AcknowledgeMessage(Message message, string errorMessage)
        {
            if (message.UserProperties.TryGetValue(MessageHeaders.ExpectsAcknowledgement, out object expectsAcknowledgement))
            {
                if (expectsAcknowledgement is bool && (bool)expectsAcknowledgement is true)
                {
                    message.UserProperties.TryGetValue(MessageHeaders.MessageId, out object messageId);
                    message.UserProperties.TryGetValue(MessageHeaders.OriginSubdomain, out object originSubdomain);

                    var dto = new MessageAcknowledgedDto
                    {
                        MessageId = (Guid)messageId,
                        Errors = errorMessage
                    };

                    await _messageSender.SendMessageAsync(
                        (String)originSubdomain,
                        _messageAcknowledged,
                        dto,
                        false);
                }
            }
        }

        private async Task SendFailureNotification(string errorMessage)
        {
            EmailSendConfig email = new EmailSendConfig
            {
                ToEmail = _systemFailureNotificationRecipient,
                Subject = "System Notification Failure in the Patient Portal",
                BodyPlainText = errorMessage
            };

            await _sendEmailService.SendEmail(email);
        }

        private async Task SaveToInbox(Inbox message, CancellationToken cancellationToken)
        {
            using (var context = _contextFactory.MessagingDbContext)
            {
                if (context.Inbox.Find(message.MessageId) != null)
                    throw new Exception($"Error in TopicListener.UpdateInbox: Attempted to insert message into Inbox that already exists. MessageID: {message.MessageId}");

                await context.Inbox.AddAsync(message);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task UpdateInbox(Guid messageId, string errorMessage, CancellationToken cancellationToken)
        {
            using (var context = _contextFactory.MessagingDbContext)
            {
                try
                {
                    var message = context.Inbox.Find(messageId);

                    if (message is null)
                    {
                        _logger.LogError("Error in TopicListener.UpdateInbox: Could not find message in Inbox with MessageId {0}", message.MessageId);
                        return;
                    }

                    message.SetStatus(errorMessage);
                    context.Inbox.Update(message);
                    await context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception caught in TopicListener.UpdateInbox: {0}", ex);
                }
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            string errorMessage = $"Message handler encountered an exception: { exceptionReceivedEventArgs.Exception } ";
            errorMessage += $"Endpoint: { context.Endpoint} - Entity Path: { context.EntityPath } - Executing Action: { context.Action }";
            _logger.LogError(errorMessage);

            return Task.CompletedTask;
        }
    }
}
