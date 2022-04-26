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
    public class PatientRequestUpdateEmail : TestBase
    {        
        private const string _oldEmailAddress = "oldemail@example.com";

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new PatientRequestUpdateEmailCommand();
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        public async Task ShouldUpdateEmail()
        {
            string userId = RunAsDefaultUser();
            var p = new Patient
            {
                Id = Guid.NewGuid(),
                Active = true,
                EmailAddress = _oldEmailAddress,
            };
            await Testing.AddAsync(p);

            var command = new UpdateEmailAddressCommand
            {
                EmailAddress = "a-new-email-address@example.org"
            };
            await SendAsync(command);

            var up = await Testing.FindAsync<Patient>(p.Id);

            up.EmailAddress.Should().Be(command.EmailAddress);
            up.LastModifiedBy.Should().NotBeNull();
            up.LastModifiedBy.Should().Be(userId);
            up.LastModified.Should().NotBeNull();
            up.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}
