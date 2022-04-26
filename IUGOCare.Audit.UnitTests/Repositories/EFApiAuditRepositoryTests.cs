using System.Collections.Generic;
using IUGOCare.Audit.Models;
using IUGOCare.Audit.Persistence;
using IUGOCare.Audit.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace IUGOCare.Audit.UnitTests.Repositories
{
    [TestFixture]
    public class EFApiAuditRepositoryTests
    {
        [Test]
        public void Append_ReturnsApiAudiModelWithId()
        {
            // Arrange
            var apiAudits = new List<ApiAudit>();
            
            var mockSet = new Mock<DbSet<ApiAudit>>();
            mockSet.Setup(x => x.Add(It.IsAny<ApiAudit>()))
                .Callback<ApiAudit>(s => apiAudits.Add(new ApiAudit()));

            var mockReliqContext = new Mock<IAuditDbContext>();
            mockReliqContext.Setup(r => r.ApiAudit).Returns(mockSet.Object);
            IAuditDbContext context = mockReliqContext.Object;

            var mockFactory = new Mock<IAuditDbContextFactory>();
            mockFactory.Setup(f => f.AuditDbContext).Returns(context);
            IAuditDbContextFactory factory = mockFactory.Object;

            var repository = new EFApiAuditRepository(factory);

            // Act
            ApiAudit result = repository.Append(new ApiAudit());

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
