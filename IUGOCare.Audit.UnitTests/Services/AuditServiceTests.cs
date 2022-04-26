using System;
using NUnit.Framework;
using Moq;
using IUGOCare.Audit.Repositories;
using IUGOCare.Audit.Models;
using IUGOCare.Audit.Services;

namespace IUGOCare.Audit.UnitTests.Services
{
    [TestFixture]
    public class AuditServiceTests
    {
        [Test]
        public void AddRequestAudit_WhenSuccessfullyAdded_ReturnsNonEmptyGuid()
        {
            // Arrange
            var mockRepository = new Mock<IApiAuditRepository>();
            mockRepository.Setup(x => x.Append(It.IsAny<ApiAudit>()))
                .Returns<ApiAudit>(a =>
                {
                    a.Id = Guid.NewGuid();
                    return a;
                });
            var auditService = new AuditService(mockRepository.Object);

            // Act
            Guid result = auditService.AddRequestAudit(Guid.NewGuid(), new Uri("http://www.example.com"), "someMethod", "someHeaders", 
                "someContent", Guid.NewGuid(), "", "");

            // Assert
            Assert.AreNotEqual(Guid.Empty, result);
        }

        [Test]
        public void AddRequestAudit_WhenUnsuccessful_ReturnsEmptyGuid()
        {
            // Arrange
            var mockRepository = new Mock<IApiAuditRepository>();
            mockRepository.Setup(x => x.Append(It.IsAny<ApiAudit>())).Throws(new Exception());
            var auditService = new AuditService(mockRepository.Object);

            // Act
            Guid result = auditService.AddRequestAudit(Guid.NewGuid(), new Uri("http://www.example.com"), "someMethod", "someHeaders", "someContent",
                Guid.NewGuid(), "", "");

            // Assert
            Assert.AreEqual(Guid.Empty, result);
        }

        [TestCase("http://www.example.com/api/v1/User/11111111-1111-1111-1111-111111111111/PasswordReset")]
        [TestCase("http://www.example.com/api/v1/User/ChangePassword")]
        public void AddRequestAudit_ForIgnoredContentRoutes_DoesNotLogContent(string ignoredContentRouteUri)
        {
            // Arrange
            var mockRepository = new Mock<IApiAuditRepository>();
            var auditService = new AuditService(mockRepository.Object);

            // Act
            Guid result = auditService.AddRequestAudit(Guid.NewGuid(), new Uri(ignoredContentRouteUri), "someMethod", "someHeaders", "someContent",
                Guid.NewGuid(), "", "");

            // Assert
            mockRepository.Verify(m => m.Append(It.Is<ApiAudit>(arg => arg.Content == null)));
        }

        [Test]
        public void AddRequestAudit_ForRoutesThatAreNotConfiguredToIgnoreContent_LogsContent()
        {
            // Arrange
            var mockRepository = new Mock<IApiAuditRepository>();
            var auditService = new AuditService(mockRepository.Object);

            // Act
            Guid result = auditService.AddRequestAudit(Guid.NewGuid(), new Uri("http://www.example.com"), "someMethod", "someHeaders", "someContent",
                Guid.NewGuid(), "", "");

            // Assert
            mockRepository.Verify(m => m.Append(It.Is<ApiAudit>(arg => arg.Content != null)));
        }

        [Test]
        public void AddResponseAudit_WhenSuccessfullyAdded_ReturnsNonEmptyGuid()
        {
            // Arrange
            var mockRepository = new Mock<IApiAuditRepository>();
            mockRepository.Setup(x => x.Append(It.IsAny<ApiAudit>()))
                .Returns<ApiAudit>(a =>
                {
                    a.Id = Guid.NewGuid();
                    return a;
                });
            var auditService = new AuditService(mockRepository.Object);

            // Act
            Guid result = auditService.AddResponseAudit(Guid.NewGuid(), new Uri("http://www.example.com"), "someMethod", "someStatusCode", "someReasonPhrase", "someHeaders", "someContent",
                Guid.NewGuid(), "", "");

            // Assert
            Assert.AreNotEqual(Guid.Empty, result);
        }

        [Test]
        public void AddResponseAudit_WhenUnsuccessful_ReturnsEmptyGuid()
        {
            // Arrange
            var mockRepository = new Mock<IApiAuditRepository>();
            mockRepository.Setup(x => x.Append(It.IsAny<ApiAudit>())).Throws(new Exception());
            var auditService = new AuditService(mockRepository.Object);

            // Act
            Guid result = auditService.AddResponseAudit(Guid.NewGuid(), new Uri("http://www.example.com"), "someMethod", "someStatusCode", "someReasonPhrase", "someHeaders", "someContent",
                Guid.NewGuid(), "", "");

            // Assert
            Assert.AreEqual(Guid.Empty, result);
        }

        [Test]
        [TestCase("/api/v1/version")]
        public void AddRequestAudit_WhenUriContainsIgnoredPath_DoesNotAppendLog(string ignoredPath)
        {
            var uriWithIgnoredPath = new Uri("http://www.example.com" + ignoredPath);
            var mockRepository = new Mock<IApiAuditRepository>();
            var auditService = new AuditService(mockRepository.Object);

            Guid result = auditService.AddResponseAudit(Guid.Empty, uriWithIgnoredPath, "someMethod", "someStatusCode", "someReasonPhrase", "someHeaders", "someContent",
                Guid.NewGuid(), "", "");

            // Assert
            mockRepository.VerifyNoOtherCalls();
        }

        [Test]
        public void AddRequestAudit_WhenUriDoesNotContainIgnoredPath_AppendsLog()
        {
            var uri = new Uri("http://www.example.com/api/v1/someAllowedPath");
            var mockRepository = new Mock<IApiAuditRepository>();
            var auditService = new AuditService(mockRepository.Object);

            Guid result = auditService.AddResponseAudit(Guid.Empty, uri, "someMethod", "someStatusCode", "someReasonPhrase", "someHeaders", "someContent",
                Guid.NewGuid(), "", "");

            // Assert
            mockRepository.Verify(x => x.Append(It.IsAny<ApiAudit>()), Times.Once);
            mockRepository.VerifyNoOtherCalls();
        }
    }
}
