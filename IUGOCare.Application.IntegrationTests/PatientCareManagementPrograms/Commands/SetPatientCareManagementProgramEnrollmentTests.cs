using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.PatientCareManagementPrograms.Commands.SetPatientCareManagementProgramEnrollment;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.PatientCareManagementPrograms.Commands
{
    public class SetPatientCareManagementProgramEnrollmentTests : TestBase
    {
        [Test]
        public void ShouldThrowIfClinicPatientNotFound()
        {
            // Arrange
            var command = new SetPatientCareManagementProgramEnrollmentCommand
            {
                ClinicPatientId = Guid.NewGuid(),
                CareProgramShortName = "CCM",
                IsEnrolled = true
            };

            // Act/Assert
            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>()
                .Where(ex => ex.Message.Contains($"Clinic Patient not found for ClinicPatientId {command.ClinicPatientId}"));
        }

        [Test]
        public async Task ShouldThrowIfCareProgramNotFound()
        {
            // Arrange
            var patient = new Patient
            {
                Id = Guid.NewGuid()
            };

            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = Guid.NewGuid(),
                PatientId = patient.Id
            };

            await Testing.AddAsync(patient);
            await Testing.AddAsync(clinicPatient);

            var command = new SetPatientCareManagementProgramEnrollmentCommand
            {
                ClinicPatientId = clinicPatient.ClinicPatientId,
                CareProgramShortName = "someInvalidCareProgram",
                IsEnrolled = true
            };

            // Act/Assert
            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<NotFoundException>()
                .Where(ex => ex.Message.Contains($"Care Program not found with Short Name {command.CareProgramShortName}"));
        }

        [Test]
        public async Task WhenEverythingIsValid_ShouldSetPatientCareManagementProgramEnrollment()
        {
            // Arrange
            var patient = new Patient
            {
                Id = Guid.NewGuid()
            };

            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = Guid.NewGuid(),
                PatientId = patient.Id
            };

            await Testing.AddAsync(patient);
            await Testing.AddAsync(clinicPatient);

            var command = new SetPatientCareManagementProgramEnrollmentCommand
            {
                ClinicPatientId = clinicPatient.ClinicPatientId,
                CareProgramShortName = "CCM",
                IsEnrolled = true
            };

            // Act
            await Testing.SendAsync(command);

            // Assert
            var patientCarePrograms = await Testing.GetAll<PatientCareManagementProgram>();
            var clinicPatientPrograms = patientCarePrograms
                .Where(p => p.ClinicPatientId == clinicPatient.ClinicPatientId)
                .ToList();

            var correlatedCarePrograms = new List<CareManagementProgram>();
            foreach (var p in clinicPatientPrograms)
            {
                var careProgram = await Testing.FindAsync<CareManagementProgram>(p.CareManagementProgramId);
                correlatedCarePrograms.Add(careProgram);
            }

            Assert.That(clinicPatientPrograms.Count(), Is.GreaterThanOrEqualTo(1));
            Assert.That(correlatedCarePrograms.Any(p => p.ShortName == command.CareProgramShortName), Is.True);
        }

        [Test]
        public async Task WhenEverythingIsValid_ShouldNotAddIfEnrollmentExists()
        {
            // Arrange
            var patient = new Patient
            {
                Id = Guid.NewGuid()
            };

            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = Guid.NewGuid(),
                PatientId = patient.Id
            };

            var ccmProgram = Testing.GetAll<CareManagementProgram>().Result.FirstOrDefault(p => p.ShortName == "CCM");

            var patientCareProgramEnrollment = new PatientCareManagementProgram
            {
                ClinicPatientId = clinicPatient.ClinicPatientId,
                CareManagementProgramId = ccmProgram.Id
            };

            await Testing.AddAsync(patient);
            await Testing.AddAsync(clinicPatient);
            await Testing.AddAsync(patientCareProgramEnrollment);

            var command = new SetPatientCareManagementProgramEnrollmentCommand
            {
                ClinicPatientId = clinicPatient.ClinicPatientId,
                CareProgramShortName = ccmProgram.ShortName,
                IsEnrolled = true
            };

            // Act
            await Testing.SendAsync(command);

            // Assert
            var clinicPatientPrograms = Testing.GetAll<PatientCareManagementProgram>()
                .Result.Where(p => p.ClinicPatientId == clinicPatient.ClinicPatientId).ToList();

            Assert.That(clinicPatientPrograms.Count(p => p.CareManagementProgramId == ccmProgram.Id), Is.EqualTo(1));
        }

        [Test]
        public async Task WhenEverythingIsValid_ShouldRemoveIfEnrollmentExists()
        {
            // Arrange
            var patient = new Patient
            {
                Id = Guid.NewGuid()
            };

            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = Guid.NewGuid(),
                PatientId = patient.Id
            };

            var ccmProgram = Testing.GetAll<CareManagementProgram>().Result.FirstOrDefault(p => p.ShortName == "CCM");

            var patientCareProgramEnrollment = new PatientCareManagementProgram
            {
                ClinicPatientId = clinicPatient.ClinicPatientId,
                CareManagementProgramId = ccmProgram.Id
            };

            await Testing.AddAsync(patient);
            await Testing.AddAsync(clinicPatient);
            await Testing.AddAsync(patientCareProgramEnrollment);

            var command = new SetPatientCareManagementProgramEnrollmentCommand
            {
                ClinicPatientId = clinicPatient.ClinicPatientId,
                CareProgramShortName = ccmProgram.ShortName,
                IsEnrolled = false
            };

            // Act
            await Testing.SendAsync(command);

            // Assert
            var clinicPatientPrograms = Testing.GetAll<PatientCareManagementProgram>()
                .Result.Where(p => p.ClinicPatientId == clinicPatient.ClinicPatientId).ToList();

            Assert.That(clinicPatientPrograms.Any(p => p.CareManagementProgramId == ccmProgram.Id), Is.False);
        }

        [Test]
        public async Task WhenEverythingIsValid_ShouldHandleRemovalRequestOfNonexistantEnrollment()
        {
            // Arrange
            var patient = new Patient
            {
                Id = Guid.NewGuid()
            };

            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = Guid.NewGuid(),
                PatientId = patient.Id
            };

            var ccmProgram = Testing.GetAll<CareManagementProgram>().Result.FirstOrDefault(p => p.ShortName == "CCM");

            await Testing.AddAsync(patient);
            await Testing.AddAsync(clinicPatient);

            var command = new SetPatientCareManagementProgramEnrollmentCommand
            {
                ClinicPatientId = clinicPatient.ClinicPatientId,
                CareProgramShortName = ccmProgram.ShortName,
                IsEnrolled = false
            };

            // Act / Assert
            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().NotThrow<Exception>();
        }
    }
}
