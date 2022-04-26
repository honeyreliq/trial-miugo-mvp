using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Messages.Commands.SetMessageAcknowledged;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Messages.Commands
{
    using static Testing;
    public class SetMessageAcknowledgedTests : TestBase
    {
        [Test]
        public void ShouldRequireMessageId()
        {
            var command = new SetMessageAcknowledgedCommand();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("MessageId"))
                .And.Errors["MessageId"].Should().Contain("Message Id must not be null or empty");
        }

        [Test]
        public void WhenMessageDoesNotExist_ShouldThrowValidationException()
        {
            var messageId = Guid.NewGuid();
            var command = new SetMessageAcknowledgedCommand { MessageId = messageId };

            var expectedErrorMessage = $"Error in SetMessageAcknowledgedCommandHandler: Message not found in Outbox with MessageId {messageId}";

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey(nameof(Outbox)))
                .And.Errors[nameof(Outbox)].Should().Contain(expectedErrorMessage);
        }

        [Test]
        public async Task WhenMessageDoesExist_ShouldSetMessageAcknowledged()
        {
            // Arrange
            var messageId = TestConstants.AllOnesGuid;
            var errorMessage = "some error message";
            var message = new Outbox
            {
                MessageId = messageId,
                MessageAcknowledged = false,
                CorrelationId = "someCorrelationId",
                DateStaged = DateTimeOffset.UtcNow,
                Label = "someLabel",
                MessageBody = "someMessageBody"
            };
            await AddAsync(message);

            var command = new SetMessageAcknowledgedCommand
            {
                MessageId = messageId,
                Errors = errorMessage
            };

            // Act
            await SendAsync(command);

            // Assert
            var actualMessage = await FindAsync<Outbox>(messageId);

            actualMessage.Should().NotBeNull();
            actualMessage.MessageId.Should().Be(messageId);
            actualMessage.MessageAcknowledged.Should().Be(true);
            actualMessage.Errors.Should().Be(errorMessage);
        }
    }
}
