using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.AddOrUpdateEmergencyContact;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    public class AddOrUpdateEmergencyContactTests : TestBase
    {
        [Test]
        public void WhenActivatedOnANonexistentPatient_ShouldThrowPatientNotFoundException()
        {
            var command = new AddOrUpdateEmergencyContactCommand
            {
                ClinicPatientId = Guid.NewGuid(),
                ContactName = "Deborah Caron",
                Relationship = "Daughter",
                Phone = "345 888 7733"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task WhenExecutedOnAnExistingContact_ShouldUpdateEmergencyContact()
        {
            var patientId = Guid.NewGuid();
            var clinicId = Guid.NewGuid();
            var clinicPatientId = Guid.NewGuid();

            await Testing.AddAsync(new Patient
            {
                Id = patientId
            });

            await Testing.AddAsync(new ClinicPatient
            {
                ClinicPatientId = clinicPatientId,
                ClinicId = clinicId,
                PatientId = patientId
            });

            await Testing.AddAsync(new PatientEmergencyContact
            {
                ClinicPatientId = clinicPatientId,
                ContactName = "Deborah Caron",
                Relationship = "Daughter",
                Phone = "345 888 7733"
            });

            var command = new AddOrUpdateEmergencyContactCommand
            {
                ClinicPatientId = clinicPatientId,
                ContactName = "Deborah Caron",
                Relationship = "Daughter",
                Phone = "345 777 7733"
            };

            await Testing.SendAsync(command);

            var contact = await Testing.FindAsync<PatientEmergencyContact>(clinicPatientId);

            contact.Should().NotBeNull();
            contact.ContactName.Should().Be(command.ContactName);
            contact.Relationship.Should().Be(command.Relationship);
            contact.Phone.Should().Be(command.Phone);
        }

        [Test]
        public async Task WhenExecutedOnANullContact_ShouldRegisterEmergencyContact()
        {
            var patientId = Guid.NewGuid();
            var clinicId = Guid.NewGuid();
            var clinicPatientId = Guid.NewGuid();

            await Testing.AddAsync(new Patient
            {
                Id = patientId
            });

            await Testing.AddAsync(new ClinicPatient
            {
                ClinicPatientId = clinicPatientId,
                ClinicId = clinicId,
                PatientId = patientId
            });

            var command = new AddOrUpdateEmergencyContactCommand
            {
                ClinicPatientId = clinicPatientId,
                ContactName = "Deborah Caron",
                Relationship = "Daughter",
                Phone = "345 888 7733"
            };
            await Testing.SendAsync(command);

            var up = await Testing.FindAsync<PatientEmergencyContact>(clinicPatientId);

            up.Should().NotBeNull();
            up.ContactName.Should().Be(command.ContactName);
            up.Relationship.Should().Be(command.Relationship);
            up.Phone.Should().Be(command.Phone);
        }
    }
}
