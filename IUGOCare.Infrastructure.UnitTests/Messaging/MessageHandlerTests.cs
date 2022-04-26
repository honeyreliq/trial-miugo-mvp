using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Patients.Commands.RegisterPatient;
using IUGOCare.Infrastructure.Messaging;
using MediatR;
using Moq;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging
{
    public class MessageHandlerTests
    {
        MessageHandler _messageHandler;
        Mock<ICommandFactory> _mockCommandFactory;
        Mock<IMediator> _mockMediator;

        [SetUp]
        public void Setup()
        {
            _mockCommandFactory = new Mock<ICommandFactory>();
            _mockMediator = new Mock<IMediator>();
            _messageHandler = new MessageHandler(_mockMediator.Object, _mockCommandFactory.Object);
        }

        [Test]
        public async Task Handle_ShouldDispatchValidCommand()
        {
            // Arrange
            _mockCommandFactory
                .Setup(cf => cf.CreateCommand(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new RegisterPatientCommand { ClinicPatientId = Guid.NewGuid() });

            var eventName = "someEventName";
            var messageBody = "someMessageBody";

            // Act
            await _messageHandler.Handle(eventName, messageBody);

            // Assert
            _mockCommandFactory.Verify(cf => cf.CreateCommand(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockMediator.Verify(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public void Handle_WhenCommandIsNull_ThrowsNotFoundException()
        {
            // Arrange
            _mockCommandFactory
                .Setup(cf => cf.CreateCommand(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((IRequest)null);

            var eventName = "someEventName";
            var messageBody = "someMessageBody";

            // Act and assert
            Assert.ThrowsAsync(typeof(NotFoundException), async () => await _messageHandler.Handle(eventName, messageBody));
        }
    }
}
