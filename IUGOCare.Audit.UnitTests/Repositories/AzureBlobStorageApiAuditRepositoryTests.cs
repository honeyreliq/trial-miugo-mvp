using System;
using NUnit.Framework;
using Moq;
using IUGOCare.Audit.Models;
using IUGOCare.Audit.Repositories;
using IUGOCare.Audit.Infrastructure;

namespace IUGOCare.Audit.UnitTests.Repositories
{
    [TestFixture]
    public class AzureBlobStorageApiAuditRepositoryTests
    {
        [Test]
        public void Append_ReturnsApiAudiModelWithId()
        {
            // Arrange
            var mockWriter = new Mock<IApiAuditAzureBlobStorageStrategy>();
            var repository = new AzureBlobStorageApiAuditRepository(mockWriter.Object);

            // Act
            ApiAudit result = repository.Append(new ApiAudit());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.Id);
        }

        [Test]
        public void Append_PassesApiAuditModelToBlobWriter()
        {
            // Arrange
            var mockWriter = new Mock<IApiAuditAzureBlobStorageStrategy>();
            var repository = new AzureBlobStorageApiAuditRepository(mockWriter.Object);
            var apiAuditModel = new ApiAudit { Id = Guid.NewGuid() };

            // Act
            ApiAudit result = repository.Append(apiAuditModel);

            // Assert
            mockWriter.Verify(mock => mock.Add(apiAuditModel), Times.Once);
            mockWriter.VerifyNoOtherCalls();
        }
    }
}
