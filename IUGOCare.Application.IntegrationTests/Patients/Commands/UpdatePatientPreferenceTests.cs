using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.UpdatePatientPreferences;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Common.Constants;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    using static Testing;
    public class UpdatePatientPreferenceTests : TestBase
    {
        [Test]
        public void ShouldRequirePatientId()
        {
            var command = new UpdatePatientPreferencesCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("PatientId"))
                    .And.Errors["PatientId"].Should().Contain("Patient Id is required.");
        }

        [Test]
        public void ShouldRequireExistingPatient()
        {
            var command = new UpdatePatientPreferencesCommand
            {
                PatientId = Guid.NewGuid()
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("PatientId"))
                    .And.Errors["PatientId"].Should().Contain("Patient must be registered.");
        }

        [Test]
        public async Task ShouldUpdatePatientPreferences()
        {
            var patientId = TestConstants.AllOnesGuid;
            var clinicPatientId = Guid.NewGuid();
            var patientLanguage = Languages.EnglishLanguage;
            var patientTimeZone = DefaultPreferences.DefaultTimeZone;
            var patientWindowsTimeZone = DefaultPreferences.DefaultWindowsTimeZone;
            var patientTheme = DefaultPreferences.DefaultTheme;
            var patientTooltips = DefaultPreferences.DefaultTooltips;
            var patientDateFormat = DefaultPreferences.DefaultDateFormat;
            var patientTimeFormat = DefaultPreferences.DefaultTimeFormat;

            var expectedPatientLanguage = Languages.SpanishLanguage;
            var expectedPatientTimeZone = "Another TimeZone";
            var expectedPatientWindowsTimeZone = "Another WindowsTimeZone";
            var expectedPatientTheme = "Another theme";
            var expectedPatientTooltips = false;
            var expectedPatientDateFormat = "Another Date Format";
            var expectedPatientTimeFormat = "Another Time Format";

            var patient = new Patient
            {
                Id = patientId,
                PatientTheme = patientTheme,
                Tooltips = patientTooltips,
                DateFormat = patientDateFormat,
                TimeFormat = patientTimeFormat
            };
            patient.Clinics.Add(new ClinicPatient {
                ClinicPatientId = clinicPatientId,
                TimeZone = patientTimeZone,
                WindowsTimeZone = patientWindowsTimeZone,
            });

            patient.SetLanguage(patientLanguage);

            await Testing.AddAsync(patient);

            var command = new UpdatePatientPreferencesCommand
            {
                PatientId = patientId,
                PatientLanguage = expectedPatientLanguage,
                TimeZone = expectedPatientTimeZone,
                WindowsTimeZone = expectedPatientWindowsTimeZone,
                PatientTheme = expectedPatientTheme,
                Tooltips = expectedPatientTooltips,
                DateFormat = expectedPatientDateFormat,
                TimeFormat = expectedPatientTimeFormat
            };

            await Testing.SendAsync(command);

            var updatedPatient = await Testing.FindAsync<Patient>(patientId);

            Assert.AreEqual(expectedPatientLanguage, updatedPatient.PatientLanguage);
            Assert.AreEqual(expectedPatientTheme, updatedPatient.PatientTheme);
            Assert.AreEqual(expectedPatientTooltips, updatedPatient.Tooltips);
            Assert.AreEqual(expectedPatientDateFormat, updatedPatient.DateFormat);
            Assert.AreEqual(expectedPatientTimeFormat, updatedPatient.TimeFormat);

            var updatedClinicPatient = await Testing.FindAsync<ClinicPatient>(clinicPatientId);
            Assert.AreEqual(expectedPatientTimeZone, updatedClinicPatient.TimeZone);
            Assert.AreEqual(expectedPatientWindowsTimeZone, updatedClinicPatient.WindowsTimeZone);
        }
    }
}
