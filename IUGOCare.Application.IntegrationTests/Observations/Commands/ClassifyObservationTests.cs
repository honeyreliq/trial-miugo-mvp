using System;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Observations.Commands.ClassifyObservation;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Observations.Commands
{
    public class ClassifyObservationTests : TestBase
    {
        [Test]
        public async Task ShouldSetAppropriateProperties()
        {
            // Arrange
            var observation = new Observation();
            observation.Id = Guid.NewGuid();
            observation.Source = "Validic";
            observation.ObservationCode = "blood-glucose";
            observation.ObservationLevel = "Na";
            observation.ObservationStatus = "Stable";
            await Testing.AddAsync(observation);

            // Act
            var command = new ClassifyObservationCommand
            {
                Id = observation.Id,
                ObservationStatus = "Critical",
                ObservationLevel = "Above",
            };
            await Testing.SendAsync(command);

            // Assert
            var classifiedObservation = await Testing.FindAsync<Observation>(observation.Id);
            Assert.AreEqual(command.ObservationStatus, classifiedObservation.ObservationStatus);
            Assert.AreEqual(command.ObservationLevel, classifiedObservation.ObservationLevel);
        }

        [Test]
        public void WhenObservationNotFound_ShouldThrowNotFoundException()
        {
            // Act
            var command = new ClassifyObservationCommand
            {
                Id = Guid.NewGuid(),
                ObservationStatus = "Critical",
                ObservationLevel = "Above",
            };

            Assert.ThrowsAsync<NotFoundException>(async () => await Testing.SendAsync(command));
        }
    }
}
