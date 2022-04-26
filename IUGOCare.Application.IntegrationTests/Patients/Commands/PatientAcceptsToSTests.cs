using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.PatientAcceptsToS;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    public class PatientAcceptsToSTests : TestBase
    {
        [Test]
        public void ShouldRequireValidPatient()
        {
            var command = new PatientAcceptsToSCommand() { PatientId = Guid.NewGuid() };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldUpdatePatient()
        {
            var userId = Testing.RunAsDefaultUser();

            var p = new Patient() { Id = Guid.NewGuid() };
            await Testing.AddAsync(p);

            var command = new PatientAcceptsToSCommand() { PatientId = p.Id };
            await Testing.SendAsync(command);

            var patient = await Testing.FindAsync<Patient>(p.Id);
            patient.Active.Should().BeTrue();
            patient.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            patient.LastModifiedBy.Should().Be(userId);
        }

        [Test]
        public async Task ShouldNotUpdatePatientIfPatientIsAlreadyActive()
        {
            var p = new Patient() { Id = Guid.NewGuid(), Active = true };
            await Testing.AddAsync(p);

            var command = new PatientAcceptsToSCommand() { PatientId = p.Id };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }
    }
}
