using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.RegisterPatient;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Common.Constants;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    using static Testing;
    public class RegisterPatientTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new RegisterPatientCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldRegisterPatient()
        {
            // Arrange
            var userId = Testing.RunAsDefaultUser();
            var clinicPatientId = TestConstants.AllOnesGuid;
            var clinicSubdomain = "someClinicSubdomain";
            var givenName = "someFirstName";
            var middleName = "someMiddleName";
            var familyName = "someFamilyName";
            var emailAddress = "practitioner@example.com";
            var patientLanguage = "en";
            var patientTimeZone = "Etc/UTC";
            var patientWindowsTimeZone = "UTC";
            var phoneNumber = "555 123 4567";
            var birthDate = new DateTime(2010, 7, 7);
            var addressLine1 = "someAddressLine1";
            var addressLine2 = "someAddressLine2";
            var city = "someCity";
            var state = "someState";
            var zipCode = "someZipCode";
            var country = "someCountry";
            var emergencyContactName = "someContactName";
            var emergencyContactPhone = "444 444 4444";
            var emergencyContactRelationship = "Brother";

            var command = new RegisterPatientCommand
            {
                ClinicPatientId = clinicPatientId,
                ClinicSubdomain = clinicSubdomain,
                ClinicId = Guid.NewGuid(),
                GivenName = givenName,
                MiddleName = middleName,
                FamilyName = familyName,
                EmailAddress = emailAddress,
                PatientLanguage = patientLanguage,
                PatientTimeZone = patientTimeZone,
                PatientWindowsTimeZone = patientWindowsTimeZone,
                PhoneNumber = phoneNumber,
                BirthDate = birthDate,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                State = state,
                ZipCode = zipCode,
                Country = country,
                EmergencyContactName = emergencyContactName,
                EmergencyContactPhoneNumber = emergencyContactPhone,
                EmergencyContactRelationship = emergencyContactRelationship,
            };

            // Act
            await SendAsync(command);

            var clinicPatient = await Testing.FindAsync<ClinicPatient>(clinicPatientId);
            var patient = await Testing.FindAsync<Patient>(clinicPatient.PatientId);

            patient.Should().NotBeNull();
            patient.CreatedBy.Should().Be(userId);
            patient.LastModifiedBy.Should().BeNull();
            patient.LastModified.Should().BeNull();
            patient.EmailAddress.Should().Be(emailAddress);
            patient.PatientLanguage.Should().Be(patientLanguage.ToUpper());

            clinicPatient.GivenName.Should().Be(givenName);
            clinicPatient.MiddleName.Should().Be(middleName);
            clinicPatient.FamilyName.Should().Be(familyName);
            clinicPatient.TimeZone.Should().Be(patientTimeZone);
            clinicPatient.WindowsTimeZone.Should().Be(patientWindowsTimeZone);
            clinicPatient.Phone.Should().Be(phoneNumber);
            clinicPatient.BirthDate.Should().Be(birthDate);
            clinicPatient.Address.AddressLines.Should().Be($"{addressLine1} {addressLine2}");
            clinicPatient.Address.City.Should().Be(city);
            clinicPatient.Address.State.Should().Be(state);
            clinicPatient.Address.ZipCode.Should().Be(zipCode);
            clinicPatient.Address.Country.Should().Be(country);

            var patientEmergencyContacts = await Testing.GetAll<PatientEmergencyContact>();
            var patientEmergencyContact = patientEmergencyContacts.FirstOrDefault(c => c.ClinicPatientId == clinicPatient.ClinicPatientId);

            patientEmergencyContact.Should().NotBeNull();
            patientEmergencyContact.ContactName.Should().Be(emergencyContactName);
            patientEmergencyContact.Phone.Should().Be(emergencyContactPhone);
            patientEmergencyContact.Relationship.Should().Be(emergencyContactRelationship);

            clinicPatient.ClinicPatientId.Should().Be(clinicPatientId);
            clinicPatient.GivenName.Should().Be(givenName);
            clinicPatient.MiddleName.Should().Be(middleName);
            clinicPatient.FamilyName.Should().Be(familyName);
            clinicPatient.TimeZone.Should().Be(patientTimeZone);
            clinicPatient.WindowsTimeZone.Should().Be(patientWindowsTimeZone);
            clinicPatient.Phone.Should().Be(phoneNumber);
            clinicPatient.BirthDate.Should().Be(birthDate);
            clinicPatient.Address.AddressLines.Should().Be($"{addressLine1} {addressLine2}");
            clinicPatient.Address.City.Should().Be(city);
            clinicPatient.Address.State.Should().Be(state);
            clinicPatient.Address.ZipCode.Should().Be(zipCode);
            clinicPatient.Address.Country.Should().Be(country);
        }

        [Test]
        public async Task ShouldRegisterPatientWithoutEmailAddress()
        {
            var userId = Testing.RunAsDefaultUser();
            var clinicPatientId = TestConstants.AllOnesGuid;

            var command = new RegisterPatientCommand
            {
                ClinicPatientId = clinicPatientId,
                ClinicId = Guid.NewGuid(),
                ClinicSubdomain = "someClinicSubdomain",
                EmailAddress = null,
                PatientLanguage = "en",
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC",
                PhoneNumber = "555 123 4567",
                BirthDate = DateTime.Now.AddYears(-10)
            };

            await SendAsync(command);

            var clinicPatient = await Testing.FindAsync<ClinicPatient>(clinicPatientId);
            var patient = await Testing.FindAsync<Patient>(clinicPatient.PatientId);

            patient.Should().NotBeNull();
            patient.EmailAddress.Should().BeNull();
            patient.CreatedBy.Should().Be(userId);
            patient.Created.Should().BeCloseTo(DateTime.Now, 10000);
            patient.LastModifiedBy.Should().BeNull();
            patient.LastModified.Should().BeNull();
        }

        [Test]
        public void ShouldNotRegisterPatientWithInvalidClinicPatientId()
        {
            var command = new RegisterPatientCommand
            {
                ClinicPatientId = Guid.Empty,
                ClinicSubdomain = "someClinicSubdomain",
                ClinicId = Guid.NewGuid(),
                EmailAddress = null,
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldNotRegisterPatientWithInvalidClinicId()
        {
            var command = new RegisterPatientCommand
            {
                ClinicPatientId = Guid.NewGuid(),
                ClinicSubdomain = "someClinicSubdomain",
                ClinicId = Guid.Empty,
                EmailAddress = null,
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldNotRegisterPatientWithInvalidClinicSubdomain()
        {
            var command = new RegisterPatientCommand
            {
                ClinicPatientId = Guid.NewGuid(),
                ClinicSubdomain = "",
                ClinicId = Guid.NewGuid(),
                EmailAddress = null,
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [TestCase("test")]
        [TestCase("test@example")]
        [TestCase("test@example.")]
        [TestCase("test email")]
        public void ShouldNotRegisterPatientWithInvalidEmailAddress(string emailaddress)
        {
            var command = new RegisterPatientCommand
            {
                ClinicPatientId = Guid.Empty,
                ClinicSubdomain = "someClinicSubdomain",
                EmailAddress = emailaddress,
                PatientLanguage = "en",
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("EmailAddress"))
                    .And.Errors["EmailAddress"].Should().Contain("The format of the email address is incorrect.");
        }

        [Test]
        public async Task ShouldRequireUniqueEmailAddress()
        {
            Testing.RunAsDefaultUser();

            await SendAsync(new RegisterPatientCommand
            {
                ClinicPatientId = Guid.NewGuid(),
                ClinicId = Guid.NewGuid(),
                ClinicSubdomain = "someClinicSubdomain",
                EmailAddress = "practitioner@example.com",
                PatientLanguage = "en",
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC",
                PhoneNumber = "555 123 4567",
                BirthDate = DateTime.Now.AddYears(-10)
            });

            var command = new RegisterPatientCommand
            {
                ClinicPatientId = Guid.NewGuid(),
                ClinicSubdomain = "someClinicSubdomain",
                EmailAddress = "practitioner@example.com",
                PatientLanguage = "en",
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC",
                PhoneNumber = "555 123 4567",
                BirthDate = DateTime.Now.AddYears(-10)
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("EmailAddress"))
                    .And.Errors["EmailAddress"].Should().Contain("The email address is already registered.");
        }

        [Test]
        public async Task ShouldRequireUniqueGuid()
        {
            Testing.RunAsDefaultUser();

            var clinicPatientId = TestConstants.AllOnesGuid;

            await SendAsync(new RegisterPatientCommand
            {
                ClinicPatientId = clinicPatientId,
                ClinicSubdomain = "someClinicSubdomain",
                ClinicId = Guid.NewGuid(),
                EmailAddress = "practitioner@example.com",
                PatientLanguage = "en",
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC",
                PhoneNumber = "555 123 4567",
                BirthDate = DateTime.Now.AddYears(-10)
            });

            var command = new RegisterPatientCommand
            {
                ClinicPatientId = clinicPatientId,
                ClinicSubdomain = "someClinicSubdomain",
                EmailAddress = "practitioner2@example.com",
                PatientLanguage = "en",
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC",
                PhoneNumber = "555 123 4567",
                BirthDate = DateTime.Now.AddYears(-10)
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("ClinicPatientId"))
                    .And.Errors["ClinicPatientId"].Should().Contain("The clinic patient id is already registered.");
        }

        [Test]
        public void ShouldRequirePatientTimeZone()
        {
            var command = new RegisterPatientCommand
            {
                ClinicPatientId = Guid.NewGuid(),
                ClinicSubdomain = "",
                ClinicId = Guid.NewGuid(),
                EmailAddress = null,
                PatientTimeZone = string.Empty,
                PatientWindowsTimeZone = "UTC"
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("PatientTimeZone"))
                    .And.Errors["PatientTimeZone"].Should().Contain("The patient's time zone is required.");
        }

        [Test]
        public void ShouldRequirePatientWindowsTimeZone()
        {
            var command = new RegisterPatientCommand
            {
                ClinicPatientId = Guid.NewGuid(),
                ClinicSubdomain = "",
                ClinicId = Guid.NewGuid(),
                EmailAddress = null,
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = string.Empty
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("PatientWindowsTimeZone"))
                    .And.Errors["PatientWindowsTimeZone"].Should().Contain("The patient's Windows time zone is required.");
        }

        [Test]
        public async Task ShouldRegisterPatientWithSpanishLanguage()
        {
            var userId = Testing.RunAsDefaultUser();
            var clinicPatientId = TestConstants.AllOnesGuid;

            var command = new RegisterPatientCommand
            {
                ClinicPatientId = clinicPatientId,
                ClinicSubdomain = "someClinicSubdomain",
                ClinicId = Guid.NewGuid(),
                EmailAddress = "practitioner@example.com",
                PatientLanguage = "es",
                PatientTimeZone = "Etc/UTC",
                PatientWindowsTimeZone = "UTC",
                PhoneNumber = "555 123 4567",
                BirthDate = DateTime.Now.AddYears(-10)
            };

            await SendAsync(command);

            var clinicPatient = await Testing.FindAsync<ClinicPatient>(clinicPatientId);
            var patient = await Testing.FindAsync<Patient>(clinicPatient.PatientId);

            patient.Should().NotBeNull();
            patient.EmailAddress.Should().NotBeNull();
            patient.CreatedBy.Should().Be(userId);
            patient.Created.Should().BeCloseTo(DateTime.Now, 10000);
            patient.LastModifiedBy.Should().BeNull();
            patient.LastModified.Should().BeNull();
            patient.PatientLanguage.Should().BeEquivalentTo(Languages.SpanishLanguage);
        }
    }
}
