using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Observations.Commands.CreateManualObservation;
using IUGOCare.Application.Observations.Queries.GetPatientObservations;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Observations.Commands
{
    public class CreateManualObservationTests : TestBase
    {
        [Test]
        public async Task ShouldAddObservation()
        {
            var userId = Testing.RunAsDefaultUser();

            var patient = new Patient() { Id = Guid.NewGuid() };
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = Guid.NewGuid(),
                ClinicId = Guid.NewGuid()
            });
            await Testing.AddAsync(patient);

            var command = new CreateManualObservationCommand()
            {
                ObservationCode = "blood-pressure",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-10)
            };

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().NotThrow<Exception>();
        }

        [Test]
        public async Task ShouldAddObservationData()
        {
            var userId = Testing.RunAsDefaultUser();

            var patient = new Patient() { Id = Guid.NewGuid() };
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = Guid.NewGuid()
            });
            await Testing.AddAsync(patient);

            var command = new CreateManualObservationCommand()
            {
                ObservationCode = "blood-pressure",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-5)
            };
            command.ObservationDataList.Add(
                new ManualObservationDataItem()
                {
                    ObservationCode = "systolic",
                    Unit = "mm Hg",
                    Value = 120
                });

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().NotThrow<Exception>();
        }

        [TestCase("blood-glucose", "blood-glucose", "mg/dL", 92, "mmol/L", 5.1)]
        [TestCase("body-temperature", "body-temperature", "??F", 113, "??C", 45)]
        [TestCase("weight", "weight", "lbs", 153, "kg", 69.4)]
        [TestCase("workouts", "distance", "mi", 20, "km", 32.2)]
        [TestCase("workouts", "elevation", "ft", 10, "m", 3)]
        [TestCase("workouts", "water-consumed", "oz", 60, "mL", 1774.4)]
        public async Task ShouldConvertObservationUnit(
            string observationCode, string observationDataCode, string unit, decimal value, string expectedUnit, decimal expectedValue
        )
        {
            var userId = Testing.RunAsDefaultUser();

            var patient = new Patient() { Id = Guid.NewGuid(), Auth0Id = userId };
            var clinicPatientId = Guid.NewGuid();
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = clinicPatientId
            });
            await Testing.AddAsync(patient);

            var command = new CreateManualObservationCommand()
            {
                ObservationCode = observationCode,
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-5)
            };
            command.ObservationDataList.Add(
                new ManualObservationDataItem()
                {
                    ObservationCode = observationDataCode,
                    Unit = unit,
                    Value = value
                });

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().NotThrow<Exception>();

            var query = new GetPatientObservationsQuery
            {
                ClinicPatientId = clinicPatientId,
                EffectiveDateStart = command.EffectiveDate,
                EffectiveDateEnd = command.EffectiveDate,
                ObservationCodes = new [] { observationCode }
            };

            var observation = Testing.SendAsync(query).Result.Observations;
            var observationData = observation.First().ObservationsData.First();
            Assert.AreEqual(expectedUnit, observationData.Unit);
            Assert.That(observationData.Value, Is.EqualTo(expectedValue).Within(0.01));
        }

        [TestCase("blood-pressure", "systolic", "mmHg", 120)]
        [TestCase("heart-rate", "heart-rate", "bpm", 80)]
        [TestCase("oxygen-saturation", "oxygen-saturation", "SpO2", 50)]
        [TestCase("respiratory-rate", "respiratory-rate", "rpm", 60)]
        [TestCase("a1c", "a1c", "%", 40)]
        [TestCase("workouts", "floors", "floors", 3)]
        [TestCase("workouts", "calories-burned", "kcal", 1000)]
        [TestCase("workouts", "steps", "steps", 1500)]
        public async Task ShouldNotConvertObservationUnit(string observationCode, string observationDataCode, string unit, decimal value)
        {
            var userId = Testing.RunAsDefaultUser();

            var patient = new Patient() { Id = Guid.NewGuid(), Auth0Id = userId };
            var clinicPatientId = Guid.NewGuid();
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = clinicPatientId
            });
            await Testing.AddAsync(patient);

            var command = new CreateManualObservationCommand()
            {
                ObservationCode = observationCode,
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-5)
            };
            command.ObservationDataList.Add(
                new ManualObservationDataItem()
                {
                    ObservationCode = observationDataCode,
                    Unit = unit,
                    Value = value
                });

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().NotThrow<Exception>();

            var query = new GetPatientObservationsQuery
            {
                ClinicPatientId = clinicPatientId,
                EffectiveDateStart = command.EffectiveDate,
                EffectiveDateEnd = command.EffectiveDate,
                ObservationCodes = new string[] { observationCode }
            };

            var observation = Testing.SendAsync(query).Result.Observations;
            var observationData = observation.First().ObservationsData.First();
            Assert.AreEqual(unit, observationData.Unit);
            Assert.That(observationData.Value, Is.EqualTo(value));
        }

        [Test]
        public async Task ShouldNotAllowFutureDate()
        {
            var userId = Testing.RunAsDefaultUser();

            var patient = new Patient() { Id = Guid.NewGuid() };
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = Guid.NewGuid()
            });
            await Testing.AddAsync(patient);

            var command = new CreateManualObservationCommand()
            {
                ObservationCode = "blood-pressure",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(5)
            };
            command.ObservationDataList.Add(
                new ManualObservationDataItem()
                {
                    ObservationCode = "heart-rate",
                    Unit = "bpm",
                    Value = 75
                });

            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>();
        }
    }
}
