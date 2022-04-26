using System;
using System.Collections.Generic;
using System.Linq;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Patients;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    public class NewPatientEnrolledInCareProgramsDtoMappersTests
    {
        [Test]
        public void ShouldMapToCommand()
        {
            // Arrange
            var clinicPatientId = Guid.NewGuid();
            var enrolledPrograms = new List<string> { "CCM", "RPM" };

            var dto = new NewPatientEnrolledInCareProgramsDto
            {
                PatientId = clinicPatientId,
                CarePrograms = enrolledPrograms
            };

            // Act
            var command = dto.MapToEnrollNewPatientFromExternalSystemCommand();

            // Assert
            Assert.That(command.ClinicPatientId, Is.EqualTo(clinicPatientId));
            foreach (var careProgram in enrolledPrograms)
            {
                Assert.That(command.CarePrograms.Contains(careProgram));
            }

        }
    }
}
