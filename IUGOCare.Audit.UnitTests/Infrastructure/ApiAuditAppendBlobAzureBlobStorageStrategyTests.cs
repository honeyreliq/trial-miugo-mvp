using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using IUGOCare.Audit.Infrastructure.Wrappers;
using IUGOCare.Audit.Infrastructure;
using IUGOCare.Audit.Constants;
using IUGOCare.Audit.Models;
using IUGOCare.Audit.Repositories;
using System.Threading;
using Microsoft.Azure.Storage;

namespace IUGOCare.Audit.UnitTests.Infrastructure
{
    [TestFixture]
    public class ApiAuditAppendBlobAzureBlobStorageStrategyTests
    {
        [Test]
        public void Add_WhenNoExceptionIsThrown_ReturnsTrue()
        {
            // Arrange
            var apiAuditModel = new ApiAudit();
            var mockBlob = new Mock<ICloudAppendBlob>();
            var mockGenerator = new Mock<IAzureBlobNameGenerator>();
            var mockUtilities = new Mock<IAzureBlobStorageUtilities>();
            mockUtilities.Setup(x => x.GetCurrentApiAuditBlobReference(It.IsAny<string>())).Returns(mockBlob.Object);
            var mockFailoverRepository = new Mock<IApiAuditFailoverRepository>();
            var blobWriter = new ApiAuditAppendBlobAzureBlobStorageStrategy(mockGenerator.Object, mockUtilities.Object, mockFailoverRepository.Object, "some client name");

            // Act
            var result = blobWriter.Add(apiAuditModel);

            // Assert
            Assert.IsTrue(result);

            // Cleanup
            blobWriter.Flush();
        }

        [Test]
        public void Add_AfterFlushIsCalled_ThrowsException()
        {
            // Arrange
            var apiAuditModel = new ApiAudit();
            var mockGenerator = new Mock<IAzureBlobNameGenerator>();
            var mockUtilities = new Mock<IAzureBlobStorageUtilities>();
            var mockFailoverRepository = new Mock<IApiAuditFailoverRepository>();
            var blobWriter = new ApiAuditAppendBlobAzureBlobStorageStrategy(mockGenerator.Object, mockUtilities.Object, mockFailoverRepository.Object, "some client name");

            // Act
            // Assert
            blobWriter.Flush();
            Assert.That(() => blobWriter.Add(apiAuditModel), Throws.InvalidOperationException);
        }

        [Test]
        public void Add_WhenEverythingWorksProperly_ResultsInCallToPersistToBlobStorage()
        {
            // Arrange
            var apiAuditModel = new ApiAudit() { Id = Guid.Empty };
            var blobName = "some blob name";
            var mockGenerator = new Mock<IAzureBlobNameGenerator>();
            mockGenerator.Setup(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(blobName);
            var mockBlob = new Mock<ICloudAppendBlob>();
            var mockUtilities = new Mock<IAzureBlobStorageUtilities>();
            mockUtilities.Setup(x => x.GetCurrentApiAuditBlobReference(It.IsAny<string>())).Returns(mockBlob.Object);
            var mockFailoverRepository = new Mock<IApiAuditFailoverRepository>();
            var blobWriter = new ApiAuditAppendBlobAzureBlobStorageStrategy(mockGenerator.Object, mockUtilities.Object, mockFailoverRepository.Object, "some client name");

            // Act
            blobWriter.Add(apiAuditModel);

            // Assert
            Thread.Sleep(100); // There is a bit of latency for the background thread to pickup the message
            mockGenerator.Verify(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
            mockUtilities.Verify(x => x.GetCurrentApiAuditBlobReference(blobName), Times.Once);
            mockBlob.Verify(x => x.AppendText(apiAuditModel.ToDelimitedString("|")), Times.Once);
            mockFailoverRepository.VerifyNoOtherCalls();

            // Cleanup
            blobWriter.Flush();
        }

        [Test]
        public void Add_WhenStorageExceptionIsThrown_IncrementsFileNumberAndRetriesOnce()
        {
            // Arrange
            var retryInterval = 10;
            var blobName = "some blob name";
            var apiAuditModel = new ApiAudit() { Id = Guid.Empty };
            var result = new RequestResult() { HttpStatusCode = 409 };
            var mockGenerator = new Mock<IAzureBlobNameGenerator>();
            mockGenerator.Setup(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(blobName);
            var mockBlob = new Mock<ICloudAppendBlob>();
            mockBlob.SetupSequence(x => x.AppendText(It.IsAny<string>()))
                .Throws(new StorageException(result, "first attempt", new Exception()))
                .Pass(); // first retry
            var mockUtilities = new Mock<IAzureBlobStorageUtilities>();
            mockUtilities.Setup(x => x.GetCurrentApiAuditBlobReference(It.IsAny<string>())).Returns(mockBlob.Object);
            var mockFailoverRepository = new Mock<IApiAuditFailoverRepository>();
            var blobWriter = new ApiAuditAppendBlobAzureBlobStorageStrategy(mockGenerator.Object, mockUtilities.Object, mockFailoverRepository.Object, "some client name", retryInterval);

            // Act
            blobWriter.Add(apiAuditModel);

            // Assert
            Thread.Sleep(retryInterval * 4 + 100); // Allow enough time for total possible attempts (4) plus some latency
            mockGenerator.Verify(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(2));
            mockGenerator.Verify(x => x.IncrementFileNumber(), Times.Once);
            mockUtilities.Verify(x => x.GetCurrentApiAuditBlobReference(blobName), Times.Exactly(2));
            mockBlob.Verify(x => x.AppendText(apiAuditModel.ToDelimitedString("|")), Times.Exactly(2));

            // Cleanup
            blobWriter.Flush();
        }

        [Test]
        public void Add_WhenStorageExceptionIsThrown_IncrementsFileNumberAndRetriesTwice()
        {
            // Arrange
            var retryInterval = 10;
            var blobName = "some blob name";
            var apiAuditModel = new ApiAudit() { Id = Guid.Empty };
            var result = new RequestResult() { HttpStatusCode = 409 };
            var mockGenerator = new Mock<IAzureBlobNameGenerator>();
            mockGenerator.Setup(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(blobName);
            var mockBlob = new Mock<ICloudAppendBlob>();
            mockBlob.SetupSequence(x => x.AppendText(It.IsAny<string>()))
                .Throws(new StorageException(result, "first attempt", new Exception()))
                .Throws(new StorageException(result, "first retry", new Exception()))
                .Pass(); // second retry
            var mockUtilities = new Mock<IAzureBlobStorageUtilities>();
            mockUtilities.Setup(x => x.GetCurrentApiAuditBlobReference(It.IsAny<string>())).Returns(mockBlob.Object);
            var mockFailoverRepository = new Mock<IApiAuditFailoverRepository>();
            var blobWriter = new ApiAuditAppendBlobAzureBlobStorageStrategy(mockGenerator.Object, mockUtilities.Object, mockFailoverRepository.Object, "some client name", retryInterval);

            // Act
            blobWriter.Add(apiAuditModel);

            // Assert
            Thread.Sleep(retryInterval * 4 + 100); // Allow enough time for total possible attempts (4) plus some latency
            mockGenerator.Verify(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(3));
            mockGenerator.Verify(x => x.IncrementFileNumber(), Times.Exactly(2));
            mockUtilities.Verify(x => x.GetCurrentApiAuditBlobReference(blobName), Times.Exactly(3));
            mockBlob.Verify(x => x.AppendText(apiAuditModel.ToDelimitedString("|")), Times.Exactly(3));

            // Cleanup
            blobWriter.Flush();
        }

        [Test]
        public void Add_WhenStorageExceptionIsThrown_IncrementsFileNumberAndRetriesThreeTimes()
        {
            // Arrange
            var retryInterval = 10;
            var blobName = "some blob name";
            var apiAuditModel = new ApiAudit() { Id = Guid.Empty };
            var result = new RequestResult() { HttpStatusCode = 409 };
            var mockGenerator = new Mock<IAzureBlobNameGenerator>();
            mockGenerator.Setup(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(blobName);
            var mockBlob = new Mock<ICloudAppendBlob>();
            mockBlob.SetupSequence(x => x.AppendText(It.IsAny<string>()))
                .Throws(new StorageException(result, "first attempt", new Exception()))
                .Throws(new StorageException(result, "first retry", new Exception()))
                .Throws(new StorageException(result, "second retry", new Exception()))
                .Pass(); // third retry
            var mockUtilities = new Mock<IAzureBlobStorageUtilities>();
            mockUtilities.Setup(x => x.GetCurrentApiAuditBlobReference(It.IsAny<string>())).Returns(mockBlob.Object);
            var mockFailoverRepository = new Mock<IApiAuditFailoverRepository>();
            var blobWriter = new ApiAuditAppendBlobAzureBlobStorageStrategy(mockGenerator.Object, mockUtilities.Object, mockFailoverRepository.Object, "some client name", retryInterval);

            // Act
            blobWriter.Add(apiAuditModel);

            // Assert
            Thread.Sleep(retryInterval * 4 + 100); // Allow enough time for total possible attempts (4) plus some latency
            mockGenerator.Verify(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(4));
            mockGenerator.Verify(x => x.IncrementFileNumber(), Times.Exactly(2));
            mockUtilities.Verify(x => x.GetCurrentApiAuditBlobReference(blobName), Times.Exactly(4));
            mockBlob.Verify(x => x.AppendText(apiAuditModel.ToDelimitedString("|")), Times.Exactly(4));

            // Cleanup
            blobWriter.Flush();
        }

        [Test]
        public void Add_WhenStorageExceptionIsThrownFourTimes_PersistsToTheDatabase()
        {
            // Arrange
            int retryInterval = 10;
            var blobName = "some blob name";
            var apiAuditModel = new ApiAudit
            {
                Id = Guid.NewGuid(),
                Uri = "Some uri",
                RequestId = Guid.NewGuid(),
                Method = "Some method",
                StatusCode = "Some status code",
                ReasonPhrase = "Some reason phrase",
                Headers = "Some headers",
                Content = "Some content",
                RequestReliqUserId = Guid.NewGuid(),
                CreateDate = DateTimeOffset.MinValue,
                FirstName = "General",
                LastName = "Kenobi"
            };
            var result = new RequestResult() { HttpStatusCode = 409 };
            var message = apiAuditModel.ToDelimitedString("|");
            var mockGenerator = new Mock<IAzureBlobNameGenerator>();
            mockGenerator.Setup(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(blobName);
            var mockBlob = new Mock<ICloudAppendBlob>();
            mockBlob.SetupSequence(x => x.AppendText(message))
                .Throws(new StorageException(result, "first attempt", new Exception()))
                .Throws(new StorageException(result, "first retry", new Exception()))
                .Throws(new StorageException(result, "second retry", new Exception()))
                .Throws(new StorageException(result, "third retry and fail", new Exception()));
            var mockUtilities = new Mock<IAzureBlobStorageUtilities>();
            mockUtilities.Setup(x => x.GetCurrentApiAuditBlobReference(It.IsAny<string>())).Returns(mockBlob.Object);
            var apiAudits = new List<ApiAudit>();
            var mockFailoverRepository = new Mock<IApiAuditFailoverRepository>();
            var blobWriter = new ApiAuditAppendBlobAzureBlobStorageStrategy(mockGenerator.Object, mockUtilities.Object, mockFailoverRepository.Object, "some client name", retryInterval);

            // Act
            blobWriter.Add(apiAuditModel);

            // Assert
            Thread.Sleep(retryInterval * 4 + 100); // Allow enough time for total possible attempts (4) plus some latency
            mockGenerator.Verify(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(4));
            mockGenerator.Verify(x => x.IncrementFileNumber(), Times.Exactly(2));
            mockUtilities.Verify(x => x.GetCurrentApiAuditBlobReference(blobName), Times.Exactly(4));
            mockBlob.Verify(x => x.AppendText(message), Times.Exactly(4));
            mockFailoverRepository.Verify(x => x.Append(apiAuditModel), Times.Once);

            // Cleanup
            blobWriter.Flush();
        }

        [Test]
        public void IsFlushed_WhenQueueHasEmptiedAndIsComplete_SignalsThatTheBackgroundThreadHasCompleted()
        {
            // Arrange
            var blobName = "some blob name";
            var apiAuditModel = new ApiAudit();
            var mockGenerator = new Mock<IAzureBlobNameGenerator>();
            mockGenerator.Setup(x => x.Generate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(blobName);
            var mockBlob = new Mock<ICloudAppendBlob>();
            var mockUtilities = new Mock<IAzureBlobStorageUtilities>();
            mockUtilities.Setup(x => x.GetCurrentApiAuditBlobReference(It.IsAny<string>())).Returns(mockBlob.Object);
            var mockFailoverRepository = new Mock<IApiAuditFailoverRepository>();
            var blobWriter = new ApiAuditAppendBlobAzureBlobStorageStrategy(mockGenerator.Object, mockUtilities.Object, mockFailoverRepository.Object, "some client name");

            // Act
            blobWriter.Add(apiAuditModel);
            Thread.Sleep(100);

            // Assert
            Assert.IsTrue(blobWriter.Flush());
        }
    }
}
