using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Organizations.Commands;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Organizations.Commands
{
    class CreateOrganizationTests : TestBase
    {
        [Test]
        public void ShouldNotAllowInvalidId()
        {
            var command = new CreateOrganizationCommand
            {
                Name = "Test",
                Phone = "555 989 1234",
                City = "Laredo",
                State = "TX",
                ZipCode = "78046"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Id"))
                    .And.Errors["Id"].Should().Contain("The organization id is required.");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void ShouldNotAllowInvalidName(string name)
        {
            var command = new CreateOrganizationCommand
            {
                Id = Guid.NewGuid(),
                Name = name,
                Phone = "555 989 1234",
                City = "Laredo",
                State = "TX",
                ZipCode = "78046"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name"))
                    .And.Errors["Name"].Should().Contain("The organization name is required.");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void ShouldNotAllowInvalidPhoneNumber(string phone)
        {
            var command = new CreateOrganizationCommand
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Phone = phone,
                City = "Laredo",
                State = "TX",
                ZipCode = "78046"
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
            var command = new CreateOrganizationCommand
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Phone = phone,
                City = "Laredo",
                State = "TX",
                ZipCode = "78046"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Phone"))
                    .And.Errors["Phone"].Should().Contain("The format of the phone number is incorrect.");
        }

        [Test]
        public async Task ShouldNotAllowRegisteredName()
        {
            await Testing.AddAsync(new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Phone = "555 989 1234"
            });

            var command = new CreateOrganizationCommand
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Phone = "555 989 1234"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name"))
                    .And.Errors["Name"].Should().Contain("The organization name is already registered.");
        }

        [Test]
        public async Task ShouldCreateOrganization()
        {
            var id = Guid.NewGuid();
            var userId = Testing.RunAsDefaultUser();

            var command = new CreateOrganizationCommand
            {
                Id = id,
                Name = "Test",
                Phone = "555 989 1234"
            };

            await Testing.SendAsync(command);

            var organization = await Testing.FindAsync<Organization>(id);

            organization.Should().NotBeNull();
            organization.Name.Should().NotBeNull();
            organization.Name.Should().Be("Test");
            organization.Phone.Should().Be("555 989 1234");
            organization.CreatedBy.Should().Be(userId);
            organization.Created.Should().BeCloseTo(DateTime.Now, 10000);
            organization.LastModifiedBy.Should().BeNull();
            organization.LastModified.Should().BeNull();
        }
    }
}
