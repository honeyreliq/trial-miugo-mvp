using System;
using System.Threading.Tasks;
using IUGOCare.Application.Observations.Commands.ReviewObservation;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Observations.Commands
{
    public class ReviewObservationTests : TestBase
    {
        [Test]
        public async Task ShouldReviewObservation()
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
            var command = new ReviewObservationCommand
            {
                Id = observation.Id,
                IsReviewedDate = new DateTimeOffset(1757, 8, 9, 1, 32, 0, TimeSpan.FromHours(-4)),
                ReviewedByName = "Elizabeth Schuyler",
            };
            await Testing.SendAsync(command);

            // Assert
            var reviewedObservation = await Testing.FindAsync<Observation>(observation.Id);
            Assert.IsTrue(reviewedObservation.IsReviewed);
            Assert.AreEqual(command.IsReviewedDate, reviewedObservation.IsReviewedDate);
            Assert.AreEqual(command.ReviewedByName, reviewedObservation.ReviewedByName);
        }
    }
}
