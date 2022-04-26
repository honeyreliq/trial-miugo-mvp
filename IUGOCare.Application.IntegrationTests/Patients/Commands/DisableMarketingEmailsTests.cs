using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.DisableMarketingEmails;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    public class DisableMarketingEmailsTests : TestBase
    {
        [Test]
        public void ShouldRequireValidPatient()
        {
            var command = new DisableMarketingEmailsCommand() { PatientId = Guid.NewGuid() };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldRequireNonEmptyGuid()
        {
            var command = new DisableMarketingEmailsCommand() { PatientId = Guid.Empty };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldUpdatePatient()
        {
            var userId = Testing.RunAsDefaultUser();

            var p = new Patient() { Id = Guid.NewGuid() };
            await Testing.AddAsync(p);

            var command = new DisableMarketingEmailsCommand() { PatientId = p.Id };
            await Testing.SendAsync(command);

            var patient = await Testing.FindAsync<Patient>(p.Id);
            patient.AllowMarketingEmails.Should().BeFalse();

            // AllowMarketingEmails is false by default, no modification exists
            patient.Created.Should().BeCloseTo(DateTime.Now, 1000);
            patient.CreatedBy.Should().Be(userId);
        }
    }
}
