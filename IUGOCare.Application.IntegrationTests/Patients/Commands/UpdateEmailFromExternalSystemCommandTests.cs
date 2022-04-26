using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.UpdateEmailFromExternalSystem;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    using static Testing;

    [TestFixture]
    public class UpdateEmailFromExternalSystemCommandTests : TestBase
    {
        [Test]
        public async Task WhenExecutedOnAnActivePatient_ShouldNotUpdateEmailAddress()
        {
            // Arrange
            var patient = new Patient { Id = TestConstants.AllOnesGuid, Active = true };
            var clinicPatient = new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = TestConstants.AllTwosGuid
            };

            await AddAsync(patient);
            await AddAsync(clinicPatient);

            var command = new UpdateEmailFromExternalSystemCommand(clinicPatient.ClinicPatientId, string.Empty);

            // Act
            await SendAsync(command);

            // Assert
            var updatedPatient = await FindAsync<Patient>(patient.Id);
            updatedPatient.EmailAddress.Should().BeNull();
        }

        [Test]
        public void WhenExecutedOnANonexistentClinicPatient_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new UpdateEmailFromExternalSystemCommand(TestConstants.AllTwosGuid, string.Empty);

            // Act, Assert
            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task WhenProvidedEmailIsAssignedToAnotherPatient_ShouldNotUpdateEmailAddress()
        {
            // Arrange
            var email = "greatest.detective.slash.genius@nypd.com";
            var alreadyRegisteredPatient = new Patient
            {
                Id = TestConstants.AllOnesGuid,
                Active = true,
                EmailAddress = email
            };
            var patient = new Patient { Id = TestConstants.AllTwosGuid };
            var clinicPatient = new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = TestConstants.AllThreesGuid
            };

            await AddAsync(alreadyRegisteredPatient);
            await AddAsync(patient);
            await AddAsync(clinicPatient);

            var command = new UpdateEmailFromExternalSystemCommand(clinicPatient.ClinicPatientId, email);

            // Act
            await SendAsync(command);

            // Assert
            var updatedPatient = await FindAsync<Patient>(patient.Id);
            updatedPatient.EmailAddress.Should().BeNull();
        }

        [Test]
        public async Task WhenAllPropertiesAreValid_ShouldUpdateEmailAddress()
        {
            // Arrange
            var newEmail = "bingpot@nypd.com";
            var patient = new Patient { Id = TestConstants.AllOnesGuid };
            var clinicPatient = new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = TestConstants.AllTwosGuid
            };

            await AddAsync(patient);
            await AddAsync(clinicPatient);

            var command = new UpdateEmailFromExternalSystemCommand(clinicPatient.ClinicPatientId, newEmail);

            // Act
            await SendAsync(command);

            // Assert
            var updatedPatient = await FindAsync<Patient>(patient.Id);

            updatedPatient.EmailAddress.Should().Be(newEmail);
        }
    }
}

