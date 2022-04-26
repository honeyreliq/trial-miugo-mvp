using System;
using NUnit.Framework;
using Moq;
using IUGOCare.Audit.Infrastructure.Wrappers;
using IUGOCare.Audit.Infrastructure;
using IUGOCare.Audit.Constants;

namespace IUGOCare.Audit.UnitTests.Infrastructure
{
    [TestFixture]
    public class AzureBlobStorageUtilitiesTests
    {
        [Test]
        public void GetCurrentApiAuditBlobReference_WhenBlobExists_ReturnsBlobReference()
        {
            // Arrange
            var mockBlob = new Mock<ICloudAppendBlob>();
            mockBlob.Setup(x => x.Exists()).Returns(true);
            var mockContainer = new Mock<ICloudBlobContainer>();
            mockContainer.Setup(x => x.GetAppendBlobReference(It.IsAny<string>())).Returns(mockBlob.Object);
            var utilities = new AzureBlobStorageUtilities(mockContainer.Object);

            // Act
            var blob = utilities.GetCurrentApiAuditBlobReference("someBlobName");

            // Assert
            mockContainer.Verify(mock => mock.GetAppendBlobReference(It.IsAny<string>()), Times.Once);
            mockBlob.Verify(mock => mock.Exists(), Times.Once);
            Assert.IsNotNull(blob);
        }

        [Test]
        public void GetCurrentApiAuditBlobReference_WhenBlobDoesNotExist_CreatesAndReturnsBlobReference()
        {
            // Arrange
            var mockBlob = new Mock<ICloudAppendBlob>();
            mockBlob.Setup(x => x.Exists()).Returns(false);
            var mockContainer = new Mock<ICloudBlobContainer>();
            mockContainer.Setup(x => x.GetAppendBlobReference(It.IsAny<string>())).Returns(mockBlob.Object);
            var utilities = new AzureBlobStorageUtilities(mockContainer.Object);

            // Act
            var blob = utilities.GetCurrentApiAuditBlobReference("someBlobName");

            // Assert
            mockContainer.Verify(c => c.GetAppendBlobReference(It.IsAny<string>()), Times.Once);
            mockBlob.Verify(b => b.Exists(), Times.Once);
            mockBlob.Verify(b => b.CreateOrReplace(), Times.Once);
            mockBlob.Verify(b => b.AppendText($"{DelimitedFileHeaders.ApiAuditModelPipeDelimited}{Environment.NewLine}"), Times.Once);
            Assert.IsNotNull(blob);
        }
    }
}
