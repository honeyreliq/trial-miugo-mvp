using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.UpdatePatientInformation;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    using static Testing;

    public class UpdatePatientInformationTests : TestBase
    {
        [Test]
        public void ShouldRequireClinicPatientId()
        {
            var command = new UpdatePatientInformationCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("ClinicPatientId"))
                    .And.Errors["ClinicPatientId"].Should().Contain("ClinicPatientId is required.");
        }

        [Test]
        public void ShouldRequireExistingPatient()
        {
            var command = new UpdatePatientInformationCommand
            {
                ClinicPatientId = Guid.NewGuid()
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("ClinicPatientId"))
                    .And.Errors["ClinicPatientId"].Should().Contain("Patient must be registered.");
        }

        [Test]
        public async Task ShouldUpdatePatientInformation()
        {
            var patientId = TestConstants.AllOnesGuid;
            var clinicPatientId = TestConstants.AllTwosGuid;
            var givenName = "someGivenName";
            var middleName = "someMiddleName";
            var familyName = "someFamilyName";
            var birthDate = new DateTime(1985, 12, 25);

            var expectedGivenName = "someNewGivenName";
            var expectedMiddleName = "someNewMiddleName";
            var expectedFamilyName = "someNewFamilyName";
            var expectedPhone = "555 123 4567";

            var expectedAddressLine1 = "123 Main St.";
            var expectedCity = "Hamilton";

            var patient = new Patient
            {
                Id = patientId
            };

            var clinicPatient = new ClinicPatient
            {
                PatientId = patientId,
                ClinicPatientId = clinicPatientId,
                GivenName = givenName,
                MiddleName = middleName,
                FamilyName = familyName,
                BirthDate = birthDate
            };
            patient.Clinics.Add(clinicPatient);

            await AddAsync(patient);

            var command = new UpdatePatientInformationCommand
            {
                ClinicPatientId = clinicPatientId,
                GivenName = expectedGivenName,
                MiddleName = expectedMiddleName,
                FamilyName = expectedFamilyName,
                Phone = expectedPhone,
                BirthDate = birthDate,
                AddressLine1 = expectedAddressLine1,
                City = expectedCity
            };

            await SendAsync(command);

            var updatedClinicPatient = await FindAsync<ClinicPatient>(clinicPatientId);

            Assert.AreEqual(expectedGivenName, updatedClinicPatient.GivenName);
            Assert.AreEqual(expectedMiddleName, updatedClinicPatient.MiddleName);
            Assert.AreEqual(expectedFamilyName, updatedClinicPatient.FamilyName);
            Assert.AreEqual(expectedPhone, updatedClinicPatient.Phone);
            Assert.AreEqual(birthDate, updatedClinicPatient.BirthDate);
            Assert.AreEqual(expectedAddressLine1, updatedClinicPatient.Address.AddressLines);
            Assert.AreEqual(expectedCity, updatedClinicPatient.Address.City);
            Assert.AreEqual(expectedGivenName, updatedClinicPatient.GivenName);
            Assert.AreEqual(expectedMiddleName, updatedClinicPatient.MiddleName);
            Assert.AreEqual(expectedFamilyName, updatedClinicPatient.FamilyName);
            Assert.AreEqual(expectedPhone, updatedClinicPatient.Phone);
            Assert.AreEqual(birthDate, updatedClinicPatient.BirthDate);
            Assert.AreEqual(expectedAddressLine1, updatedClinicPatient.Address.AddressLines);
            Assert.AreEqual(expectedCity, updatedClinicPatient.Address.City);
        }
    }
}
