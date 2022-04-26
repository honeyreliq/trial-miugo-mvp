using System;
using IUGOCare.Application.Messages.Commands.SetMessageAcknowledged;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Common;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    [TestFixture]
    public class MessageAcknowledgedDtoMappersTests
    {
        [Test]
        public void MapToSetMessageAcknowledgedCommand_ValidProperties_ReturnsValidCommand()
        {
            // Arrange
            var messageId = Guid.NewGuid();

            var externalEvent = new MessageAcknowledgedDto()
            {
                MessageId = messageId
            };

            // Act
            var result = externalEvent.MapToSetMessageAcknowledgedCommand();

            // Assert
            Assert.IsInstanceOf<SetMessageAcknowledgedCommand>(result);
            Assert.That(result.MessageId.Equals(messageId));
        }
    }
}
