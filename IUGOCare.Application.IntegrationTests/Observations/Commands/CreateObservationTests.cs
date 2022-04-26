using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Observations.Commands.CreateObservation;
using IUGOCare.Application.Observations.Queries.GetPatientObservations;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Observations.Commands
{
    public class CreateObservationTests : TestBase
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

            var command = new CreateObservationCommand()
            {
                ObservationId = Guid.NewGuid(),
                ClinicPatientId = patient.Clinics.First().ClinicPatientId,
                ObservationCode = "blood-pressure",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-10),
                Source = "validic",
                ObservationStatus = "Critical",
                ObservationLevel = "Above",
            };

            await Testing.SendAsync(command);

            var observation = await Testing.FindAsync<Observation>(command.ObservationId);
            observation.ObservationCode.Should().Be(command.ObservationCode);
            observation.EffectiveDate.Should().Be(command.EffectiveDate);
            observation.Source.Should().Be(command.Source);
            observation.ObservationStatus.Should().Be(command.ObservationStatus);
            observation.ObservationLevel.Should().Be(command.ObservationLevel);
            observation.Created.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
            observation.CreatedBy.Should().Be(userId);
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

            var command = new CreateObservationCommand()
            {
                ObservationId = Guid.NewGuid(),
                ClinicPatientId = patient.Clinics.First().ClinicPatientId,
                ObservationCode = "blood-pressure",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-5)
            };
            command.ObservationDataList.Add(
                new ObservationDataItem()
                {
                    Id = Guid.NewGuid(),
                    ObservationCode = "systolic",
                    Unit = "mm Hg",
                    Value = 120
                });

            await Testing.SendAsync(command);

            var observationData = await Testing.FindAsync<ObservationData>(command.ObservationDataList.FirstOrDefault().Id);
            observationData.ObservationId.Should().Be(command.ObservationId);
            observationData.Created.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
            observationData.CreatedBy.Should().Be(userId);
        }

        [TestCase("blood-glucose", "blood-glucose", "mg/dL", 92, "mmol/L", 5.1)]
        [TestCase("body-temperature", "body-temperature", "°F", 113, "°C", 45)]
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

            var command = new CreateObservationCommand()
            {
                ObservationId = Guid.NewGuid(),
                ClinicPatientId = patient.Clinics.FirstOrDefault().ClinicPatientId,
                ObservationCode = observationCode,
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-5),
                Source = "test",
                ObservationStatus = "test status",
                ObservationLevel = "test level",
                IsReviewed = true,
                IsReviewedDate = null,
                ReviewedByName = "test user",
                Manufacturer = "test manufacturer"
            };
            command.ObservationDataList.Add(
                new ObservationDataItem()
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
                ObservationCodes = new[] { observationCode }
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

            var command = new CreateObservationCommand()
            {
                ObservationId = Guid.NewGuid(),
                ClinicPatientId = patient.Clinics.FirstOrDefault().ClinicPatientId,
                ObservationCode = observationCode,
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-5),
                Source = "test",
                ObservationStatus = "test status",
                ObservationLevel = "test level",
                IsReviewed = true,
                IsReviewedDate = null,
                ReviewedByName = "test user",
                Manufacturer = "test manufacturer"
            };
            command.ObservationDataList.Add(
                new ObservationDataItem()
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
        public void ShouldNotAllowEmptyPatientId()
        {
            var o = new Observation() { Id = Guid.NewGuid() };
            var command = new CreateObservationCommand()
            {
                ObservationId = Guid.NewGuid(),
                ObservationCode = "blood-pressure",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(5)
            };
            command.ObservationDataList.Add(
                new ObservationDataItem()
                {
                    Id = Guid.NewGuid(),
                    ObservationCode = "heart-rate",
                    Unit = "bpm",
                    Value = 75
                });


            FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("ClinicPatientId"))
                    .And.Errors["ClinicPatientId"].Should().Contain("'Clinic Patient Id' must not be empty.");
        }

        [Test]
        public async Task ShouldRequireUniqueObservationId()
        {
            var userId = Testing.RunAsDefaultUser();

            var patient = new Patient() { Id = Guid.NewGuid() };
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = Guid.NewGuid()
            });

            await Testing.AddAsync(patient);

            var observationGuid = Guid.NewGuid();

            var command = new CreateObservationCommand()
            {
                ObservationId = observationGuid,
                ClinicPatientId = patient.Clinics.First().ClinicPatientId,
                ObservationCode = "blood-pressure",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-10)
            };

            command.ObservationDataList.Add(
               new ObservationDataItem()
               {
                   Id = Guid.NewGuid(),
                   ObservationCode = "heart-rate",
                   Unit = "bpm",
                   Value = 75
               });

            await Testing.SendAsync(command);

            var command2 = new CreateObservationCommand()
            {
                ObservationId = observationGuid,
                ClinicPatientId = patient.Clinics.First().ClinicPatientId,
                ObservationCode = "blood-pressure",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-10)
            };

            command2.ObservationDataList.Add(
               new ObservationDataItem()
               {
                   Id = Guid.NewGuid(),
                   ObservationCode = "heart-rate",
                   Unit = "bpm",
                   Value = 75
               });

            FluentActions.Invoking(() => Testing.SendAsync(command2))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("ObservationId"))
                    .And.Errors["ObservationId"].Should().Contain("The observation id is already registered.");
        }

         [Test]
        public async Task ShouldNotAllowSleepTimeSurpass24Hours()
        {
            var userId = Testing.RunAsDefaultUser();

            var patient = new Patient() { Id = Guid.NewGuid() };
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = Guid.NewGuid()
            });

            await Testing.AddAsync(patient);

            var observationGuid = Guid.NewGuid();

            var command = new CreateObservationCommand()
            {
                ObservationId = observationGuid,
                ClinicPatientId = patient.Clinics.First().ClinicPatientId,
                ObservationCode = "sleep",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-10)
            };

            command.ObservationDataList.Add(
               new ObservationDataItem()
               {
                   Id = Guid.NewGuid(),
                   ObservationCode = "light",
                   Unit = "hh:mm",
                   Value = 90000
               });

            command.ObservationDataList.Add(
             new ObservationDataItem()
             {
                 Id = Guid.NewGuid(),
                 ObservationCode = "total",
                 Unit = "hh:mm",
                 Value = 90000
             });

            FluentActions.Invoking(() => Testing.SendAsync(command))
               .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey(""))
               .And.Errors[""].Should().Contain("The sum of the sleep times (excluding total) must not surpass 24 hours");
        }

        [Test]
        public async Task ShouldThrowErrorIfTotalAndOtherTimesDontMatch()
        {
            var userId = Testing.RunAsDefaultUser();

            var patient = new Patient() { Id = Guid.NewGuid() };
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patient.Id,
                ClinicPatientId = Guid.NewGuid()
            });

            await Testing.AddAsync(patient);

            var observationGuid = Guid.NewGuid();

            var command = new CreateObservationCommand()
            {
                ObservationId = observationGuid,
                ClinicPatientId = patient.Clinics.First().ClinicPatientId,
                ObservationCode = "sleep",
                EffectiveDate = DateTimeOffset.UtcNow.AddDays(-10)
            };

            command.ObservationDataList.Add(
               new ObservationDataItem()
               {
                   Id = Guid.NewGuid(),
                   ObservationCode = "light",
                   Unit = "hh:mm",
                   Value = 90000
               });

            command.ObservationDataList.Add(
             new ObservationDataItem()
             {
                 Id = Guid.NewGuid(),
                 ObservationCode = "total",
                 Unit = "hh:mm",
                 Value = 70000
             });

            FluentActions.Invoking(() => Testing.SendAsync(command))
               .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey(""))
               .And.Errors[""].Should().Contain("The sum of the sleep times categories must match the total time");
        }
    }
}
