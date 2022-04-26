using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Providers.Queries;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Providers.Queries
{
    public class GetProvidersTests : TestBase
    {
        [Test]
        public async Task ShouldReturnProvidersViewModel()
        {
            var id = Guid.NewGuid();
            var organizationId = Guid.NewGuid();

            await Testing.AddAsync(new Organization
            {
                Id = organizationId,
                Name = "Health Clinic",
                Phone = "555 989 1234"
            });

            await Testing.AddAsync(new Provider
            {
                Id = id,
                Name = "Dr. David Smith",
                Type = "Physician",
                Phone = "555 989 1234",
                OrganizationId = organizationId
            });

            var query = new GetProvidersQuery();
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ProvidersVm), result.GetType());
        }

        [Test]
        public async Task ShouldReturnProviders()
        {
            var id = Guid.NewGuid();
            var organizationId = Guid.NewGuid();

            await Testing.AddAsync(new Organization
            {
                Id = organizationId,
                Name = "Health Clinic",
                Phone = "555 989 1234"
            });

            await Testing.AddAsync(new Provider
            {
                Id = id,
                Name = "Dr. David Smith",
                Type = "Physician",
                Phone = "555 989 1234",
                OrganizationId = organizationId
            });

            var query = new GetProvidersQuery();
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            result.Providers.Should().HaveCount(1);
            result.Providers.FirstOrDefault().Name.Should().Be("Dr. David Smith");
        }
    }
}
