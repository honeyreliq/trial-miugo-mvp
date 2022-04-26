using System;
using IUGOCare.Application.Patients.Commands.UpdateEmailFromExternalSystem;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Patients;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    [TestFixture]
    public class PatientEmailUpdatedDtoMapperTests
    {
        [Test]
        public void MapToUpdateEmailFromExternalSystemCommand_ValidProperties_ReturnsValidCommand()
        {
            // Arrange
            var clinicPatientId = Guid.NewGuid();
            var emailAddress = "janeshepard@systemsalliance.com";

            var externalEvent = new PatientEmailUpdatedDto
            {
                PatientId = clinicPatientId,
                Email = emailAddress
            };

            // Act
            var result = externalEvent.MapToUpdateEmailFromExternalSystemCommand();

            // Assert
            Assert.IsInstanceOf<UpdateEmailFromExternalSystemCommand>(result);
            Assert.That(result.ClinicPatientId.Equals(clinicPatientId));
            Assert.That(result.EmailAddress.Equals(emailAddress));
        }
    }
}
