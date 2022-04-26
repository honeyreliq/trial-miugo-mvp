using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Patients.Commands.PatientAcceptsToS;
using IUGOCare.Application.UnitTests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Patients.Events
{
    public class PatientActivatedTests : CommandTestBase
    {
        Mock<ILogger<PatientActivated>> _mockLogger;
        Mock<IServiceBusSender> _mockSender;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<PatientActivated>>();
            _mockSender = new Mock<IServiceBusSender>();
        }

        [Test]
        public async Task Handle_WhenPatientIsNotFound_ShouldNotSendMessages()
        {
            // Arrange
            var notification = new PatientActivated { PatientId = Guid.NewGuid() };
            var eventHandler = new PatientActivatedHandler(_mockLogger.Object, _mockSender.Object, Context);

            // Act
            await eventHandler.Handle(notification, new CancellationToken());

            // Assert
            _mockSender.Verify(s => s.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public async Task Handle_WhenPatientWithMultipleClinicsExists_ShouldSendMessagesToAllClinics()
        {
            // Arrange
            var notification = new PatientActivated { PatientId = TestConstants.AllOnesGuid };
            var eventHandler = new PatientActivatedHandler(_mockLogger.Object, _mockSender.Object, Context);

            // Act
            await eventHandler.Handle(notification, new CancellationToken());

            // Assert
            int totalNumberOfClinics = Context.Patients
                .Include(p => p.Clinics)
                .First(p => p.Id == TestConstants.AllOnesGuid)
                .Clinics.Count;

            _mockSender.Verify(s => s.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()), Times.Exactly(totalNumberOfClinics));
        }
    }
}
