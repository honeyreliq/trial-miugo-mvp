using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Organizations.Queries;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Organizations.Queries
{
    public class GetOrganizationsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnOrganizationsViewModel()
        {
            await Testing.AddAsync(new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Phone = "555 989 1234",
                Address = new Address()
                {
                    City = "Laredo",
                    State = "TX",
                    ZipCode = "78046"
                }
            });
            var query = new GetOrganizationsQuery();
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(OrganizationsVm), result.GetType());
        }

        [Test]
        public async Task ShouldReturnCareManagementPrograms()
        {
            await Testing.AddAsync(new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Phone = "555 989 1234",
                Address = new Address()
                {
                    City = "Laredo",
                    State = "TX",
                    ZipCode = "78046"
                }
            });
            var query = new GetOrganizationsQuery();
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            result.Organizations.Should().HaveCount(1);
            result.Organizations.FirstOrDefault().Name.Should().Be("Test");
        }
    }
}
