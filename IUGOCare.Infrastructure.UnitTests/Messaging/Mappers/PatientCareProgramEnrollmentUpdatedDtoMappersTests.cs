using System;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Patients;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    public class PatientCareProgramEnrollmentUpdatedDtoMappersTests
    {
        [Test]
        public void ShouldMapToSetPatientCareManagementEnrollmentCommand()
        {
            // Arrange
            var clinicPatientId = Guid.NewGuid();
            var careProgram = "CCM";
            var isEnrolled = true;

            var externalSystemEvent = new PatientCareProgramEnrollmentUpdatedDto
            {
                PatientId = clinicPatientId,
                CareProgram = careProgram,
                IsEnrolled = isEnrolled
            };

            // Act
            var command = externalSystemEvent.MapToSetPatientCareManagementEnrollmentCommand();

            // Assert
            Assert.That(command.ClinicPatientId, Is.EqualTo(clinicPatientId));
            Assert.That(command.CareProgramShortName, Is.EqualTo(careProgram));
            Assert.That(command.IsEnrolled, Is.EqualTo(isEnrolled));
        }
    }
}
