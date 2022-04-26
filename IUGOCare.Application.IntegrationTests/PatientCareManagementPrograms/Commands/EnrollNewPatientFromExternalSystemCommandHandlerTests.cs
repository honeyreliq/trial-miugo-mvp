using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.PatientCareManagementPrograms.Commands.EnrollNewPatientFromExternalSystem;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.PatientCareManagementPrograms.Commands
{
    [TestFixture]
    public class EnrollNewPatientFromExternalSystemCommandHandlerTests : TestBase
    {
        [Test]
        public async Task GivenAnEmptyListOfCarePrograms_DoesNotEnrollClinicPatient()
        {
            // Arrange
            var carePrograms = new List<string>();
            var clinicPatientId = TestConstants.AllTwosGuid;
            var patient = new Patient { Id = TestConstants.AllOnesGuid };
            var clinicPatient = new ClinicPatient { ClinicPatientId = clinicPatientId, PatientId = patient.Id };

            await Testing.AddAsync(patient);
            await Testing.AddAsync(clinicPatient);

            var command = new EnrollNewPatientFromExternalSystemCommand(clinicPatientId, carePrograms);

            // Act
            await Testing.SendAsync(command);

            var foundClinicPatient = Testing.FindAsync<ClinicPatient>(clinicPatientId).Result;

            // Assert
            foundClinicPatient.PatientCareManagementPrograms.Count.Should().Be(0);
        }

        [Test]
        public void GivenANonExistentClinicPatient_ThrowsNotFoundException()
        {
            // Arrange
            var clinicPatientId = TestConstants.AllOnesGuid;
            var carePrograms = new List<string>();

            var command = new EnrollNewPatientFromExternalSystemCommand(clinicPatientId, carePrograms);

            // Act, Assert
            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task GivenAListOfCarePrograms_EnrollsClinicPatientInProvidedPrograms()
        {
            Assume.That((await Testing.GetAll<CareManagementProgram>()).Any(c => c.ShortName.Equals("CCM")),
                "The testing database needs a CCM care program for this test to run.");
            Assume.That((await Testing.GetAll<CareManagementProgram>()).Any(c => c.ShortName.Equals("RPM")),
                "The testing database needs an RPM care program for this test to run.");

            // Arrange
            var ccm = "CCM";
            var rpm = "RPM";
            var carePrograms = new List<string> { ccm, rpm };
            var clinicPatientId = TestConstants.AllTwosGuid;
            var patient = new Patient { Id = TestConstants.AllOnesGuid };
            var clinicPatient = new ClinicPatient { ClinicPatientId = clinicPatientId, PatientId = patient.Id };

            await Testing.AddAsync(patient);
            await Testing.AddAsync(clinicPatient);

            var command = new EnrollNewPatientFromExternalSystemCommand(clinicPatientId, carePrograms);

            // Act
            await Testing.SendAsync(command);

            var allPatientCareManagementPrograms = await Testing.GetAll<PatientCareManagementProgram>();
            var allCarePrograms = await Testing.GetAll<CareManagementProgram>();

            var enrolledPrograms = allPatientCareManagementPrograms.Where(p => p.ClinicPatientId == clinicPatientId).ToList();

            // Assert
            enrolledPrograms.Count.Should().Be(2);
            enrolledPrograms
                .Any(p => p.CareManagementProgramId.Equals(allCarePrograms.FirstOrDefault(cp => cp.ShortName.Equals(ccm)).Id))
                .Should().BeTrue();
            enrolledPrograms
                .Any(p => p.CareManagementProgramId.Equals(allCarePrograms.FirstOrDefault(cp => cp.ShortName.Equals(rpm)).Id))
                .Should().BeTrue();
        }
    }
}
