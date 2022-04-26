using System;
using IUGOCare.Application.Patients.Commands.RegisterPatient;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Patients;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    public class PatientCreatedDtoMappersTests
    {
        [Test]
        public void GivenANullAddressAndEmergencyContact_ShouldReturnValidMappedCommand()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedClinicId = Guid.NewGuid();

            var externalSystemEvent = new PatientCreatedDto
            {
                PatientId = expectedId,
                ClinicId = expectedClinicId,
                GivenName = "Arthur",
                FamilyName = "Morgan",
                DateOfBirth = new DateTime(1863, 09, 18),
            };

            // Act
            var result = externalSystemEvent.MapToRegisterPatientCommand();

            // Assert
            Assert.IsInstanceOf<RegisterPatientCommand>(result);
            Assert.That(result.ClinicPatientId.Equals(expectedId));
            Assert.That(result.ClinicId.Equals(expectedClinicId));
            Assert.That(result.GivenName.Equals(externalSystemEvent.GivenName));
            Assert.That(result.FamilyName.Equals(externalSystemEvent.FamilyName));
            Assert.That(result.BirthDate.Equals(externalSystemEvent.DateOfBirth));
            Assert.IsNull(result.EmergencyContactName);
            Assert.IsNull(result.AddressLine1);
        }

        [Test]
        public void GivenValidProperties_ShouldReturnValidMappedCommand()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedClinicId = Guid.NewGuid();

            var externalSystemEvent = new PatientCreatedDto
            {
                PatientId = expectedId,
                ClinicId = expectedClinicId,
                GivenName = "John",
                FamilyName = "Marston",
                DateOfBirth = new DateTime(1873, 07, 27),
                Address = new PatientCreatedDto.AddressDto
                {
                    Address1 = "Beecher's Hope",
                    City = "Great Plains",
                    State = "West Elizabeth"
                },
                EmergencyContact = new PatientCreatedDto.PatientEmergencyContactDto
                {
                    FullName = "Abigail Marston",
                    PhoneNumber = "555-555-5555",
                    Relationship = "Spouse"
                }
            };

            // Act
            var result = externalSystemEvent.MapToRegisterPatientCommand();

            // Assert
            Assert.IsInstanceOf<RegisterPatientCommand>(result);
            Assert.That(result.ClinicPatientId.Equals(expectedId));
            Assert.That(result.ClinicId.Equals(expectedClinicId));
            Assert.That(result.GivenName.Equals(externalSystemEvent.GivenName));
            Assert.That(result.FamilyName.Equals(externalSystemEvent.FamilyName));
            Assert.That(result.BirthDate.Equals(externalSystemEvent.DateOfBirth));
            Assert.That(result.AddressLine1.Equals(externalSystemEvent.Address.Address1));
            Assert.That(result.City.Equals(externalSystemEvent.Address.City));
            Assert.That(result.State.Equals(externalSystemEvent.Address.State));
            Assert.That(result.EmergencyContactName.Equals(externalSystemEvent.EmergencyContact.FullName));
            Assert.That(result.EmergencyContactPhoneNumber.Equals(externalSystemEvent.EmergencyContact.PhoneNumber));
            Assert.That(result.EmergencyContactRelationship.Equals(externalSystemEvent.EmergencyContact.Relationship));
        }
    }
}
