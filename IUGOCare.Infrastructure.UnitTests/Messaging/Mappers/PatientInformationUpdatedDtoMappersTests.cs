using System;
using System.Collections.Generic;
using System.Text;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Patients;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    public class PatientInformationUpdatedDtoMappersTests
    {
        [Test]
        public void MapToUpdatePatientInformationCommand_ValidProperties_ReturnsValidCommand()
        {
            // Arrange
            var clinicPatientId = Guid.NewGuid();
            var expectedGivenName = "Grace";
            var expectedFamilyName = "Adler";

            var externalSystemEvent = new PatientInformationUpdatedDto
            {
                PatientId = clinicPatientId,
                GivenName = expectedGivenName,
                FamilyName = expectedFamilyName
            };

            // Act
            var result = externalSystemEvent.MapToUpdatePatientInformationCommand();

            // Assert
            Assert.That(result.ClinicPatientId, Is.EqualTo(clinicPatientId));
            Assert.That(result.GivenName, Is.EqualTo(expectedGivenName));
            Assert.That(result.FamilyName, Is.EqualTo(expectedFamilyName));
        }
    }
}
