using System;
using IUGOCare.Application.Patients.Commands.AddOrUpdateEmergencyContact;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Patients;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    [TestFixture]
    public class PatientEmergencyContactUpdatedDtoMappersTests
    {
        [Test]
        public void MapToAddOrUpdateEmergencyContactCommand_ValidProperties_ReturnsValidCommand()
        {
            // Arrange
            var clinicPatientId = Guid.NewGuid();
            var contactName = "Jane Shepard";
            var phone = "555 123 4587";
            var relationship = "Spouse";

            var externalEvent = new PatientEmergencyContactUpdatedDto
            {
                PatientId = clinicPatientId,
                FullName = contactName,
                PhoneNumber = phone,
                Relationship = relationship
            };

            // Act
            var result = externalEvent.MapToAddOrUpdateEmergencyContactCommand();

            // Assert
            Assert.IsInstanceOf<AddOrUpdateEmergencyContactCommand>(result);
            Assert.That(result.ClinicPatientId.Equals(clinicPatientId));
            Assert.That(result.ContactName.Equals(contactName));
            Assert.That(result.Phone.Equals(phone));
            Assert.That(result.Relationship.Equals(relationship));
        }
    }
}
