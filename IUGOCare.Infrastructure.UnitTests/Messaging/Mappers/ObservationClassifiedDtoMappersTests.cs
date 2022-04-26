using System;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Observations;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    public class ObservationClassifiedDtoMappersTests
    {
        [Test]
        public void ShouldMapToCommand()
        {
            // Arrange
            var id = Guid.NewGuid();

            var externalSystemEvent = new ObservationClassifiedDto
            {
                Id = id,
                ObservationStatus = "Critical",
                ObservationLevel = "Above"
            };

            // Act
            var command = externalSystemEvent.MapToClassifyObservationCommand();

            // Assert
            Assert.That(command.Id, Is.EqualTo(id));
            Assert.That(command.ObservationStatus, Is.EqualTo(externalSystemEvent.ObservationStatus));
            Assert.That(command.ObservationLevel, Is.EqualTo(externalSystemEvent.ObservationLevel));
        }
    }
}
