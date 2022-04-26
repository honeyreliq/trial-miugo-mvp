using System;
using System.Linq;
using System.Threading.Tasks;
using IUGOCare.Application.Observations.Queries.GetPatientObservations;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Common.Constants;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Observations.Queries
{
    public class GetPatientObservationsTest : TestBase
    {
        [Test]
        public async Task ShouldReturnPatientObservationsViewModel()
        {
            var patientId = TestConstants.AllOnesGuid;
            var clinicPatientId = Guid.NewGuid();
            Testing.RunAsUser(patientId);
            var cp = new ClinicPatient
            {
                ClinicPatientId = clinicPatientId,
                PatientId = patientId,
                TimeZone = "Australia/Melbourne"
            };
            var patient = new Patient
            {
                Id = patientId,
                Auth0Id = patientId.ToString(),
                Active = true,
                AllowMarketingEmails = false
            };
            patient.Clinics.Add(cp);
            await Testing.AddAsync(patient);
            var query = new GetPatientObservationsQuery
            {
                ClinicPatientId = clinicPatientId,
                EffectiveDateStart = DateTimeOffset.Parse("2020-06-27 12:00:00 AM +00:00"),
                EffectiveDateEnd = DateTimeOffset.Parse("2020-06-28 12:00:00 AM +00:00"),
                ObservationCodes = new string[] { "someCode" }
            };
            var result = await Testing.SendAsync(query);
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(PatientObservationsVm), result.GetType());
        }

        [Test]
        public async Task ShouldReturnPatientObservations()
        {
            var patientId = Guid.NewGuid();
            var clinicPatientId = Guid.NewGuid();
            var observationId = Guid.NewGuid();
            var observationEffectiveDate = DateTimeOffset.Parse("2020-06-27 12:45:00 AM +00:00");

            Testing.RunAsUser(patientId);
            var cp = new ClinicPatient
            {
                ClinicPatientId = clinicPatientId,
                PatientId = patientId,
                TimeZone = "America/Chicago"
            };
            var p = new Patient
            {
                Id = patientId,
                Auth0Id = patientId.ToString(),
                Active = true,
                AllowMarketingEmails = false
            };
            p.Clinics.Add(cp);
            await Testing.AddAsync(p);
            await Testing.AddAsync(new Observation
            {
                Id = observationId,
                ClinicPatientId = clinicPatientId,
                Source = "test",
                ObservationStatus = "test",
                ObservationLevel = "test",
                ObservationCode = "test",
                IsReviewed = false,
                EffectiveDate = observationEffectiveDate
            });

            var query = new GetPatientObservationsQuery
            {
                ClinicPatientId = clinicPatientId,
                EffectiveDateStart = observationEffectiveDate.AddDays(-1),
                EffectiveDateEnd = observationEffectiveDate.AddDays(1),
                ObservationCodes = new string[] { "test" }
            };

            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Observations.Count);
        }

        [Test]
        public async Task PatientObservationsShouldBeInPatientTimeZone()
        {
            var patientId = TestConstants.AllOnesGuid;
            var clinicPatientId = TestConstants.AllTwosGuid;
            var observationId = TestConstants.AllThreesGuid;
            var observationEffectiveDate = DateTimeOffset.Parse("2020-06-27 12:45:00 AM +00:00");
            var expectedObservationEffectiveDate = DateTimeOffset.Parse("2020-06-27 10:45:00 AM +10:00");

            Testing.RunAsUser(patientId);
            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = clinicPatientId,
                PatientId = patientId,
                TimeZone = "Australia/Melbourne"
            };
            var patient = new Patient
            {
                Id = patientId,
                Auth0Id = patientId.ToString(),
                Active = true,
                AllowMarketingEmails = false
            };
            patient.Clinics.Add(clinicPatient);
            await Testing.AddAsync(patient);
            await Testing.AddAsync(new Observation
            {
                Id = observationId,
                ClinicPatientId = clinicPatientId,
                EffectiveDate = observationEffectiveDate,
                Source = "someSource",
                ObservationStatus = "someStatus",
                ObservationLevel = "someLevel",
                ObservationCode = "someCode",
                IsReviewed = false
            });

            var query = new GetPatientObservationsQuery
            {
                ClinicPatientId = clinicPatientId,
                EffectiveDateStart = DateTimeOffset.Parse("2020-06-27 12:00:00 AM +00:00"),
                EffectiveDateEnd = DateTimeOffset.Parse("2020-06-28 12:00:00 AM +00:00"),
                ObservationCodes = new string[] { "someCode" }
            };

            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Observations.Count);
            Assert.AreEqual(expectedObservationEffectiveDate, result.Observations.First().EffectiveDate);
        }

        [Test]
        public async Task ShouldOnlyReturnObservationsWithinRequestDates()
        {
            var patientId = TestConstants.AllOnesGuid;
            var clinicPatientId = TestConstants.AllTwosGuid;
            var observationOutOfRangeId = TestConstants.AllThreesGuid;
            var observationWithinRangeId = TestConstants.AllFoursGuid;
            var observationOutOfRangeDate = DateTimeOffset.Parse("2020-06-27 03:00:00 AM +00:00");
            var observationWithinRangeDate = DateTimeOffset.Parse("2020-06-28 04:00:00 AM +00:00");

            Testing.RunAsUser(patientId);
            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = clinicPatientId,
                PatientId = patientId,
                TimeZone = "America/Chicago"
            };
            var patient = new Patient
            {
                Id = patientId,
                Auth0Id = patientId.ToString(),
                Active = true,
                AllowMarketingEmails = false
            };
            patient.Clinics.Add(clinicPatient);
            await Testing.AddAsync(patient);
            await Testing.AddAsync(new Observation
            {
                Id = observationOutOfRangeId,
                ClinicPatientId = clinicPatientId,
                EffectiveDate = observationOutOfRangeDate,
                Source = "someSource",
                ObservationStatus = "someStatus",
                ObservationLevel = "someLevel",
                ObservationCode = "someCode",
                IsReviewed = false
            });

            await Testing.AddAsync(new Observation
            {
                Id = observationWithinRangeId,
                ClinicPatientId = clinicPatientId,
                EffectiveDate = observationWithinRangeDate,
                Source = "someSource",
                ObservationStatus = "someStatus",
                ObservationLevel = "someLevel",
                ObservationCode = "someCode",
                IsReviewed = false
            });

            var query = new GetPatientObservationsQuery
            {
                ClinicPatientId = clinicPatientId,
                EffectiveDateStart = DateTimeOffset.Parse("2020-06-27 5:00:00 AM +00:00"),
                EffectiveDateEnd = DateTimeOffset.Parse("2020-06-28 5:00:00 AM +00:00"),
                ObservationCodes = new string[] { "someCode" }
            };

            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Observations.Count);
            Assert.AreEqual(observationWithinRangeId, result.Observations.First().Id);
        }

        [TestCase("blood-glucose", "blood-glucose", "mg/dL", 90, "mmol/L", 5)]
        [TestCase("body-temperature", "body-temperature", "°F", 113, "°C", 45)]
        [TestCase("weight", "weight", "lbs", 153, "kg", 69.4)]
        [TestCase("workouts", "distance", "mi", 20, "km", 32.2)]
        [TestCase("workouts", "elevation", "ft", 9.8, "m", 3)]
        [TestCase("workouts", "water-consumed", "oz", 60, "mL", 1774.4)]
        public async Task ShouldConvertObservationUnit(
           string observationCode, string observationDataCode, string expectedUnit, decimal expectedValue, string unit, decimal value)
        {
            var patientId = TestConstants.AllOnesGuid;
            var clinicPatientId = TestConstants.AllTwosGuid;
            var observationOutOfRangeId = TestConstants.AllThreesGuid;
            var observationWithinRangeDate = DateTimeOffset.Parse("2020-06-28 04:00:00 AM +00:00");

            Testing.RunAsUser(patientId);
            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = clinicPatientId,
                PatientId = patientId,
                TimeZone = "America/Chicago"
            };
            var patient = new Patient
            {
                Id = patientId,
                Auth0Id = patientId.ToString(),
                Active = true,
                AllowMarketingEmails = false
            };
            patient.Clinics.Add(clinicPatient);
            await Testing.AddAsync(patient);
            Observation newObservation = new Observation
            {
                Id = observationOutOfRangeId,
                ClinicPatientId = clinicPatientId,
                EffectiveDate = observationWithinRangeDate,
                Source = "someSource",
                ObservationStatus = "someStatus",
                ObservationLevel = "someLevel",
                ObservationCode = observationCode,
                IsReviewed = false
            };

            newObservation.ObservationsData.Add(new ObservationData
            {
                ObservationCode = observationDataCode,
                Unit = unit,
                Value = value
            }
                );
            await Testing.AddAsync(newObservation);
            var query = new GetPatientObservationsQuery
            {
                ClinicPatientId = clinicPatientId,
                EffectiveDateStart = observationWithinRangeDate,
                EffectiveDateEnd = observationWithinRangeDate,
                ObservationCodes = new string[] { observationCode },
                UnitSystem = "imperial"
            };

            await Testing.SendAsync(query);

            
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
        public async Task ShouldNotConvertObservationUnit(
           string observationCode, string observationDataCode, string unit, decimal value)
        {
            var patientId = TestConstants.AllOnesGuid;
            var clinicPatientId = TestConstants.AllTwosGuid;
            var observationOutOfRangeId = TestConstants.AllThreesGuid;
            var observationWithinRangeDate = DateTimeOffset.Parse("2020-06-28 04:00:00 AM +00:00");

            Testing.RunAsUser(patientId);
            var clinicPatient = new ClinicPatient
            {
                ClinicPatientId = clinicPatientId,
                PatientId = patientId,
                TimeZone = "America/Chicago"
            };
            var patient = new Patient
            {
                Id = patientId,
                Auth0Id = patientId.ToString(),
                Active = true,
                AllowMarketingEmails = false
            };
            patient.Clinics.Add(clinicPatient);
            await Testing.AddAsync(patient);
            Observation newObservation = new Observation
            {
                Id = observationOutOfRangeId,
                ClinicPatientId = clinicPatientId,
                EffectiveDate = observationWithinRangeDate,
                Source = "someSource",
                ObservationStatus = "someStatus",
                ObservationLevel = "someLevel",
                ObservationCode = observationCode,
                IsReviewed = false
            };

            newObservation.ObservationsData.Add(new ObservationData
            {
                ObservationCode = observationDataCode,
                Unit = unit,
                Value = value
            }
                );
            await Testing.AddAsync(newObservation);
            var query = new GetPatientObservationsQuery
            {
                ClinicPatientId = clinicPatientId,
                EffectiveDateStart = observationWithinRangeDate,
                EffectiveDateEnd = observationWithinRangeDate,
                ObservationCodes = new string[] { observationCode },
                UnitSystem = UnitSystems.ImperialSystem
            };

            await Testing.SendAsync(query);


            var observation = Testing.SendAsync(query).Result.Observations;
            var observationData = observation.First().ObservationsData.First();
            Assert.AreEqual(unit, observationData.Unit);
            Assert.That(observationData.Value, Is.EqualTo(value));
        }


    }
}
