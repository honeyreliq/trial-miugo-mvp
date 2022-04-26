using System;
using NUnit.Framework;
using IUGOCare.Audit.Infrastructure;

namespace IUGOCare.Audit.UnitTests.Infrastructure
{
    [TestFixture]
    public class ApiAuditAzureBlobNameGeneratorTests
    {
        [Test]
        public void Generate_WhenFileNumberHasNotBeenIncremented_ReturnsNameWithCurrentTimeAnd000001()
        {
            // Arrange
            var generator = new ApiAuditAzureBlobNameGenerator();
            var client = "someClient";
            var dateTime = DateTime.MinValue;
            var expected = $"{client}.{ dateTime.ToString("yyyy.MM.dd.HH")}00.000001.csv";

            // Act
            var actual = generator.Generate(client, dateTime);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Generate_WhenFileNumberHasBeenIncrementedWithinTheSameHour_ReturnsNameWithCurrentTimeAnd000002()
        {
            // Arrange
            var generator = new ApiAuditAzureBlobNameGenerator();
            var client = "someClient";
            var dateTime = DateTime.MinValue;
            var expected = $"{client}.{ dateTime.ToString("yyyy.MM.dd.HH")}00.000002.csv";

            // Act
            generator.Generate(client, dateTime);
            generator.IncrementFileNumber();
            var actual = generator.Generate(client, dateTime);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Generate_WhenDateChangesFromThePreviousCallToGenerate_ReturnsNameWithCurrentTimeAnd000001()
        {
            // Arrange
            var generator = new ApiAuditAzureBlobNameGenerator();
            var client = "someClient";
            var firstDateTime = DateTime.MinValue;
            var secondDateTime = firstDateTime.AddHours(1);
            var expected = $"{client}.{ secondDateTime.ToString("yyyy.MM.dd.HH")}00.000001.csv";

            // Act
            generator.Generate(client, firstDateTime);
            var actual = generator.Generate(client, secondDateTime);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Generate_WhenDateChangesFromThePreviousCallToGenerateAndFileNumberHasBeenPreviouslyIncremented_ReturnsNameWithCurrentTimeAnd000002()
        {
            // Arrange
            var generator = new ApiAuditAzureBlobNameGenerator();
            var client = "someClient";
            var firstDateTime = DateTime.MinValue;
            var secondDateTime = firstDateTime.AddHours(1);
            var expected = $"{client}.{ secondDateTime.ToString("yyyy.MM.dd.HH")}00.000001.csv";

            // Act
            generator.Generate(client, firstDateTime);
            generator.IncrementFileNumber();
            var actual = generator.Generate(client, secondDateTime);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
