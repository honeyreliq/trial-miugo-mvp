using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Commands.SendPatientOnboardingEmail;
using IUGOCare.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;


namespace IUGOCare.Application.IntegrationTests.Patients.Commands
{
    public class SendPatientOnboardingEmailTests : TestBase
    {

        [Test]
        public void ShouldRequirePatientIdOrClinicPatientId()
        {
            var command = new SendPatientOnboardingEmailCommand();

            FluentActions.Invoking(() =>
                Testing.SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("PatientId"))
                    .And.Errors["PatientId"].Should().Contain("Either Patient ID or Clinic Patient ID is required");
        }

        [Test]
        public void ShouldPatientExist()
        {
            var patient = new Patient()
            {
                Id = Guid.NewGuid()
            };
           
            var command = new SendPatientOnboardingEmailCommand { PatientId = patient.Id };

            FluentActions.Invoking(() =>
                Testing.SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("PatientId"))
                    .And.Errors["PatientId"].Should().Contain("Patient not found with the provided Patient ID");
        }

        [Test]
        public void ShouldPatientExistWithClinicPatientId()
        {
            var command = new SendPatientOnboardingEmailCommand { ClinicPatientId = Guid.NewGuid() };

            FluentActions.Invoking(() =>
                Testing.SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("ClinicPatientId"))
                    .And.Errors["ClinicPatientId"].Should().Contain("Patient not found with the provided Clinic Patient ID");
        }

        [Test]
        public async Task ShouldPatientBeInactive()
        {
            var patient = new Patient() { Id = Guid.NewGuid(),
                                          Active = true };
            await Testing.AddAsync(patient);

            var command = new SendPatientOnboardingEmailCommand { PatientId = patient.Id };

            FluentActions.Invoking(() =>
                Testing.SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("PatientId"))
                    .And.Errors["PatientId"].Should().Contain("Patient must be inactive");
        }

        [Test]
        public async Task ShouldPatientBeInactiveWithClinicPatientId()
        {
            var patientId = Guid.NewGuid();

            var patient = new Patient()
            {
                Id = patientId,
                Active = true
            };

            var clinicPatient = new ClinicPatient()
            {
                ClinicPatientId = Guid.NewGuid(),
                PatientId = patientId
            };

            await Testing.AddAsync(patient);
            await Testing.AddAsync(clinicPatient);       

            var command = new SendPatientOnboardingEmailCommand { ClinicPatientId = clinicPatient.ClinicPatientId };

            FluentActions.Invoking(() =>
                Testing.SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("ClinicPatientId"))
                    .And.Errors["ClinicPatientId"].Should().Contain("Patient must be inactive");
        }

        [Test]
        public async Task ShouldSendPatientOnboardingEmail()
        {
            var patient = new Patient()
            {
                Id = Guid.NewGuid()
            };
            await Testing.AddAsync(patient);

            var command = new SendPatientOnboardingEmailCommand { PatientId = patient.Id };

            FluentActions.Invoking(() =>
                Testing.SendAsync(command)).Should().NotThrow<ValidationException>();
        }
    }
}
