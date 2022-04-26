using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Providers.Commands;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Providers.Commands
{
    public class CreateProviderTests : TestBase
    {
        [Test]
        public void ShouldNotAllowInvalidId()
        {
            var command = new CreateProviderCommand
            {
                Name = "Dr. David Smith",
                Phone = "555 989 1234",
                Type = "Physician",
                City = "Laredo",
                State = "TX",
                ZipCode = "78046",
                OrganizationId = Guid.NewGuid()
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Id"))
                    .And.Errors["Id"].Should().Contain("The provider id is required.");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void ShouldNotAllowInvalidName(string name)
        {
            var command = new CreateProviderCommand
            {
                Id = Guid.NewGuid(),
                Name = name,
                Phone = "555 989 1234",
                Type = "Physician",
                City = "Laredo",
                State = "TX",
                ZipCode = "78046",
                OrganizationId = Guid.NewGuid()
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name"))
                    .And.Errors["Name"].Should().Contain("The provider name is required.");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void ShouldNotAllowInvalidType(string type)
        {
            var command = new CreateProviderCommand
            {
                Id = Guid.NewGuid(),
                Name = "Dr. David Smith",
                Phone = "555 989 1234",
                Type = type,
                City = "Laredo",
                State = "TX",
                ZipCode = "78046",
                OrganizationId = Guid.NewGuid()
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Type"))
                    .And.Errors["Type"].Should().Contain("The provider type is required.");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void ShouldNotAllowInvalidPhoneNumber(string phone)
        {
            var command = new CreateProviderCommand
            {
                Id = Guid.NewGuid(),
                Name = "Dr. David Smith",
                Phone = phone,
                Type = "Physician",
                City = "Laredo",
                State = "TX",
                ZipCode = "78046",
                OrganizationId = Guid.NewGuid()
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Phone"))
                    .And.Errors["Phone"].Should().Contain("The phone number is required.");
        }

        [TestCase("123")]
        [TestCase("   4")]
        [TestCase("5-*")]
        [TestCase("()")]
        [TestCase("(123)")]
        [TestCase("(123) 1")]
        [TestCase("()")]
        [TestCase("12 12 12")]
        [TestCase("123456789")]
        public void ShouldNotAllowInvalidPhoneNumberFormat(string phone)
        {
            var command = new CreateProviderCommand
            {
                Id = Guid.NewGuid(),
                Name = "Dr. David Smith",
                Phone = phone,
                Type = "Physician",
                City = "Laredo",
                State = "TX",
                ZipCode = "78046",
                OrganizationId = Guid.NewGuid()
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Phone"))
                    .And.Errors["Phone"].Should().Contain("The format of the phone number is incorrect.");
        }

        [Test]
        public async Task ShouldNotAllowRegisteredName()
        {
            var organizationId = Guid.NewGuid();

            await Testing.AddAsync(new Organization
            {
                Id = organizationId,
                Name = "Good Health Clinic",
                Phone = "555 989 1234",
                Address = new Address()
                {
                    City = "Laredo",
                    State = "TX",
                    ZipCode = "78046"
                }
            });

            await Testing.AddAsync(new Provider
            {
                Id = Guid.NewGuid(),
                Name = "Dr. David Smith",
                Phone = "555 989 1234",
                Type = "Physician",
                Address = new Address()
                {
                    City = "Laredo",
                    State = "TX",
                    ZipCode = "78046"
                },
                OrganizationId = organizationId
            });

            var command = new CreateProviderCommand
            {
                Id = Guid.NewGuid(),
                Name = "Dr. David Smith",
                Phone = "555 989 1234",
                City = "Laredo",
                State = "TX",
                ZipCode = "78046",
                OrganizationId = organizationId
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name"))
                    .And.Errors["Name"].Should().Contain("The provider name is already registered.");
        }

        [Test]
        public async Task ShouldCreateProvider()
        {
            var id = Guid.NewGuid();
            var userId = Testing.RunAsDefaultUser();
            var organizationId = Guid.NewGuid();

            await Testing.AddAsync(new Organization
            {
                Id = organizationId,
                Name = "Health Clinic",
                Phone = "555 989 1234",
                Address = new Address()
                {
                    City = "Laredo",
                    State = "TX",
                    ZipCode = "78046"
                }
            });

            var command = new CreateProviderCommand
            {
                Id = id,
                Name = "Dr. David Smith",
                Type = "Physician",
                Phone = "555 989 1234",
                City = "Laredo",
                State = "TX",
                ZipCode = "78046",
                OrganizationId = organizationId
            };

            await Testing.SendAsync(command);

            var provider = await Testing.FindAsync<Provider>(id);

            provider.Should().NotBeNull();
            provider.Name.Should().NotBeNull();
            provider.Name.Should().Be("Dr. David Smith");
            provider.Type.Should().Be("Physician");
            provider.Phone.Should().Be("555 989 1234");
            provider.CreatedBy.Should().Be(userId);
            provider.Created.Should().BeCloseTo(DateTime.Now, 10000);
            provider.LastModifiedBy.Should().BeNull();
            provider.LastModified.Should().BeNull();
        }
    }
}
