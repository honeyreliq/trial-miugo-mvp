using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.UpdateEmailAddress;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    using static Testing;
    public class UpdateEmailAddressTests : TestBase
    {        
        private const string _oldEmailAddress = "oldemail@example.com";
        private const string _allOnesGuid = "11111111-1111-1111-1111-111111111111";
        private const string _allZerosGuid = "00000000-0000-0000-0000-000000000000";
        private const string _auth0Id = "123";
        private const string _invalidAuth0Id = "-99";

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new UpdateEmailAddressCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("invalid@email")]
        [TestCase("anotherinvalid@email.")]
        public void ShouldNotUpdateEmailIfEmailIsInvalid(string emailAddress)
        {
            var command = new UpdateEmailAddressCommand
            {
                PatientId = Guid.NewGuid(),
                EmailAddress = emailAddress
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldNotUpdateEmailIfPatientIdIsInvalid()
        {
            var command = new UpdateEmailAddressCommand
            {
                PatientId = Guid.Empty,
                EmailAddress = "practitioner@example.com"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [TestCase(_allOnesGuid, "newemail1@example.com")]
        [TestCase(_allZerosGuid, "newemail2@example.com")]
        [TestCase(_allOnesGuid, "newemail3@example.com")]
        public async Task ShouldUpdateEmail(string guidPatientId, string newEmailAddress)
        {
            string userId = RunAsDefaultUser();

            var p = new Patient()
            {
                Id = Guid.Parse(guidPatientId),
                Active = false,
                EmailAddress = _oldEmailAddress,
                Auth0Id = _auth0Id
            };

            await Testing.AddAsync(p);

            var command = new UpdateEmailAddressCommand
            {
                PatientId = p.Id,
                EmailAddress = newEmailAddress
            };

            await SendAsync(command);

            var up = await Testing.FindAsync<Patient>(p.Id);

            up.EmailAddress.Should().Be(command.EmailAddress);
            up.LastModifiedBy.Should().NotBeNull();
            up.LastModifiedBy.Should().Be(userId);
            up.LastModified.Should().NotBeNull();
            up.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }

        [Test]
        public void ShouldNotUpdateEmailIfPatientNotFound()
        {
            var command = new UpdateEmailAddressCommand
            {
                PatientId = Guid.Parse(_allOnesGuid),
                EmailAddress = "newEmailAddress@example.com"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldNotUpdateEmailIfAuth0AccountNotFound()
        {
            string userId = RunAsDefaultUser();

            var p = new Patient()
            {
                Id = Guid.Parse(_allOnesGuid),
                Active = false,
                EmailAddress = _oldEmailAddress,
                Auth0Id = _invalidAuth0Id
            };

            await Testing.AddAsync(p);

            var command = new UpdateEmailAddressCommand
            {
                PatientId = p.Id,
                EmailAddress = "newEmailAddress@example.com"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>();
        }


        [Test]
        public async Task ShouldNotUpdateEmailIfEmailIsAlreadyRegistered()
        {
            var p = new Patient()
            {
                Id = Guid.Parse(_allOnesGuid),
                Active = false,
                EmailAddress = _oldEmailAddress,
                Auth0Id = _auth0Id
            };

            await Testing.AddAsync(p);

            var p2 = new Patient()
            {
                Id = Guid.Parse(_allZerosGuid),
                Active = false,
                EmailAddress = "newemail1@example.com",
                Auth0Id = _auth0Id
            };

            await Testing.AddAsync(p2);

            var command = new UpdateEmailAddressCommand
            {
                PatientId = p2.Id,
                EmailAddress = _oldEmailAddress
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
               .Should().Throw<ValidationException>();

            var up = await Testing.FindAsync<Patient>(p.Id);

            up.EmailAddress.Should().Be(_oldEmailAddress);

        }
    }
}
