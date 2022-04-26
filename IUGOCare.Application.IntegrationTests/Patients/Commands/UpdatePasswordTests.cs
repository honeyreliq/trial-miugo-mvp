using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.UpdatePassword;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    public class UpdatePasswordTests : TestBase
    {
        private const string _allOnesGuid = "11111111-1111-1111-1111-111111111111";
        private const string _allZerosGuid = "00000000-0000-0000-0000-000000000000";
        private const string _auth0Id = "123";
        private const string _auth0Password = "password";
        private const string _invalidAuth0Id = "-99";

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new UpdatePasswordCommand();

            FluentActions.Invoking(() =>
                Testing.SendAsync(command)).Should().Throw<ValidationException>();
        }

        [TestCase(null, "password1")]
        [TestCase("", "password2")]
        [TestCase(" ", "password3")]
        public void ShouldNotUpdatePasswordIfCurrentIsInvalid(string currentPassword, string password)
        {
            var command = new UpdatePasswordCommand
            {
                PatientId = Guid.NewGuid(),
                CurrentPassword = currentPassword,
                NewPassword = password
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [TestCase(_auth0Password, null)]
        [TestCase(_auth0Password, "")]
        [TestCase(_auth0Password, " ")]
        public void ShouldNotUpdatePasswordIfNewIsInvalid(string currentPassword, string newPassword)
        {
            var command = new UpdatePasswordCommand
            {
                PatientId = Guid.NewGuid(),
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldNotUpdatePasswordIfPatientIdIsInvalid()
        {
            var command = new UpdatePasswordCommand
            {
                CurrentPassword = _auth0Password,
                NewPassword = "password2"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [TestCase(_allOnesGuid, _auth0Password, "password1")]
        [TestCase(_allZerosGuid, _auth0Password, "password2")]
        [TestCase(_allOnesGuid, _auth0Password, "password3")]
        public async Task ShouldUpdatePassword(string guidPatientId, string currentPassword, string newPassword)
        {
            Testing.RunAsDefaultUser();
            var p = new Patient() { Id = Guid.Parse(guidPatientId), Active = true, EmailAddress = "newEmailAddress@example.com", Auth0Id = _auth0Id };
            p.Clinics.Add(new ClinicPatient() { GivenName = "Himanshu", MiddleName = "Kumar", FamilyName = "Suri" });

            await Testing.AddAsync(p);

            var command = new UpdatePasswordCommand
            {
                PatientId = p.Id,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            var result = await Testing.SendAsync(command);

            result.Should<MediatR.Unit>().NotBeNull();
        }

        [Test]
        public void ShouldNotUpdatePasswordIfPatientNotFound()
        {
            var command = new UpdatePasswordCommand
            {
                PatientId = Guid.Parse(_allOnesGuid),
                CurrentPassword = _auth0Password,
                NewPassword = "password1"
            };
            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldNotUpdatePasswordIfAuth0AccountNotFound()
        {
            string userId = Testing.RunAsDefaultUser();
            var p = new Patient() { Id = Guid.Parse(_allOnesGuid), Active = false, EmailAddress = "newEmailAddress@example.com", Auth0Id = _invalidAuth0Id };
            await Testing.AddAsync(p);

            var command = new UpdatePasswordCommand
            {
                PatientId = p.Id,
                CurrentPassword = _auth0Password,
                NewPassword = "password1"
            };
            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>();
        }
    }
}
