using System;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Observations;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    public class ObservationReviewedDtoMappersTests
    {
        [Test]
        public void ShouldMapToCommand()
        {
            // Arrange
            var id = Guid.NewGuid();

            var externalSystemEvent = new ObservationReviewedDto
            {
                Id = id,
                IsReviewedDate = DateTimeOffset.UtcNow,
                ReviewedByName = "Hercules Mulligan"
            };

            // Act
            var command = externalSystemEvent.MapToReviewObservationCommand();

            // Assert
            Assert.That(command.Id, Is.EqualTo(id));
            Assert.That(command.IsReviewedDate, Is.EqualTo(externalSystemEvent.IsReviewedDate));
            Assert.That(command.ReviewedByName, Is.EqualTo(externalSystemEvent.ReviewedByName));
        }
    }
}
