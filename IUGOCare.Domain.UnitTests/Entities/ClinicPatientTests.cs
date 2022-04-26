using System;
using System.Linq;
using FluentAssertions;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Domain.UnitTests.Entities
{
    public class ClinicPatientTests
    {
        [Test]
        public void ShouldAddNewPatientCareManagementProgram()
        {
            // Arrange
            var careProgramId = TestConstants.AllOnesGuid;
            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = Guid.NewGuid()
            };

            // Act
            clinicPatient.SetCareManagementProgramEnrollment(careProgramId, true);

            // Assert
            Assert.That(clinicPatient.PatientCareManagementPrograms.Any(p => p.CareManagementProgramId == careProgramId), Is.True);
        }

        [Test]
        public void ShouldRemovePatientCareManagementProgram()
        {
            // Arrange
            var careProgramId = TestConstants.AllOnesGuid;
            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = Guid.NewGuid()
            };
            clinicPatient.SetCareManagementProgramEnrollment(careProgramId, true);

            // Act
            clinicPatient.SetCareManagementProgramEnrollment(careProgramId, false);

            // Assert
            Assert.That(clinicPatient.PatientCareManagementPrograms.Any(p => p.CareManagementProgramId == careProgramId), Is.False);
        }

        [Test]
        public void ShouldNotAddDuplicatePatientCareManagementPrograms()
        {
            // Arrange
            var careProgramId = TestConstants.AllOnesGuid;
            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = Guid.NewGuid()
            };

            // Act
            clinicPatient.SetCareManagementProgramEnrollment(careProgramId, true);
            clinicPatient.SetCareManagementProgramEnrollment(careProgramId, true);

            // Assert
            Assert.That(clinicPatient.PatientCareManagementPrograms.Count(p => p.CareManagementProgramId == careProgramId), Is.EqualTo(1));
        }

        [Test]
        public void ShouldHandleAttemptedRemovalOfNonExistentPatientCareManagementProgram()
        {
            // Arrange
            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = Guid.NewGuid()
            };

            // Act/Assert
            FluentActions.Invoking(() => clinicPatient.SetCareManagementProgramEnrollment(Guid.NewGuid(), false)).Should().NotThrow<Exception>();   
        }
    
        [Test]
        public void SetEmergencyContact_ShouldSetNewEmergencyContact()
        {
            // Arrange
            var contactName = "someName";
            var contactPhone = "444-444-4444";
            var contactRelationship = "Sister";

            var clinicPatient = new ClinicPatient
            {
                PatientId = Guid.NewGuid(),
                ClinicPatientId = Guid.NewGuid()
            };

            // Act
            clinicPatient.SetEmergencyContact(contactName, contactPhone, contactRelationship);

            // Assert
            Assert.AreEqual(clinicPatient.EmergencyContact.ClinicPatientId, clinicPatient.ClinicPatientId);
            Assert.That(clinicPatient.EmergencyContact.ContactName, Is.EqualTo(contactName));
            Assert.That(clinicPatient.EmergencyContact.Phone, Is.EqualTo(contactPhone));
            Assert.That(clinicPatient.EmergencyContact.Relationship, Is.EqualTo(contactRelationship));
        }

        [Test]
        public void SetEmergencyContact_ShouldUpdateExistingEmergencyContact()
        {
            // Arrange
            var newContactName = "someNewContactName";
            var newContactPhone = "someNewContactPhone";
            var newContactRelationship = "someNewContactRelationship";

            var clinicPatient = new ClinicPatient
            {
                PatientId = Guid.NewGuid(),
                ClinicPatientId = Guid.NewGuid()
            };

            clinicPatient.SetEmergencyContact("someContactName", "444-444-4444", "Mother");

            // Act
            clinicPatient.SetEmergencyContact(newContactName, newContactPhone, newContactRelationship);

            // Assert
            Assert.That(clinicPatient.EmergencyContact.ContactName, Is.EqualTo(newContactName));
            Assert.That(clinicPatient.EmergencyContact.Phone, Is.EqualTo(newContactPhone));
            Assert.That(clinicPatient.EmergencyContact.Relationship, Is.EqualTo(newContactRelationship));
        }

        [Test]
        public void SetEmergencyContact_ShouldBlankOutExistingContact()
        {
            // Arrange
            var clinicPatient = new ClinicPatient
            {
                PatientId = Guid.NewGuid(),
                ClinicPatientId = Guid.NewGuid()
            };

            clinicPatient.SetEmergencyContact("someContactName", "444-444-4444", "Mother");

            // Act
            clinicPatient.SetEmergencyContact(null, null, null);

            // Assert
            Assert.IsNull(clinicPatient.EmergencyContact);
        }
    }
}
