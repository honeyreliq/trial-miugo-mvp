using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.TargetRanges.Commands.SetTargetRanges;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.TargetRanges.Commands
{
    using static Testing;
    public class SetTargetRangesTests : TestBase
    {
        [Test]
        public void ShouldRequireNonNullTargetRanges()
        {
            var command = new SetTargetRangesCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("TargetRanges"))
                    .And.Errors["TargetRanges"].Should().Contain("TargetRanges are required.");
        }

        [Test]
        public void ShouldRequireNonEmptyTargetRanges()
        {
            var targetRanges = new List<TargetRange>();
            var command = new SetTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("TargetRanges"))
                    .And.Errors["TargetRanges"].Should().Contain("TargetRanges are required.");
        }

        [Test]
        public void ShouldRequireClinicPatientId()
        {
            var targetRanges = new List<TargetRange>
            {
                new TargetRange
                {
                    ObservationCode = "someObservationCode",
                    Unit = "someUnit",
                    CriticalHigh = 1,
                    AtRiskHigh = 2,
                    AtRiskLow = 3,
                    CriticalLow = 4
                }
            };
            var command = new SetTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("ClinicPatientId"))
                    .And.Errors["ClinicPatientId"].Should().Contain("ClinicPatientId is required.");
        }

        [Test]
        public void ShouldRequireObservationCode()
        {
            var targetRanges = new List<TargetRange>
            {
                new TargetRange
                {
                    ObservationCode = string.Empty
                }
            };
            var command = new SetTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("TargetRanges[0].ObservationCode"))
                    .And.Errors["TargetRanges[0].ObservationCode"].Should().Contain("Observation Code is required.");
        }

        [Test]
        public void ShouldRequireUnit()
        {
            var targetRanges = new List<TargetRange>
            {
                new TargetRange
                {
                    ObservationCode = "someObservationCode",
                    Unit = string.Empty
                }
            };
            var command = new SetTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("TargetRanges[0].Unit"))
                    .And.Errors["TargetRanges[0].Unit"].Should().Contain("Unit is required.");
        }

        [Test]
        public void ShouldRequireValidValues()
        {
            var expectedErrors = new Dictionary<string, string>
            {
                { "TargetRanges[0].CriticalHigh", "CriticalHigh must be greater than or equal to 0." },
                { "TargetRanges[0].AtRiskHigh", "AtRiskHigh must be greater than or equal to 0." },
                { "TargetRanges[0].AtRiskLow", "AtRiskLow must be greater than or equal to 0." },
                { "TargetRanges[0].CriticalLow", "CriticalLow must be greater than or equal to 0." }
            };

            var targetRanges = new List<TargetRange>
            {
                new TargetRange
                {
                    ObservationCode = "someObservationCode",
                    Unit = "someUnit",
                    CriticalHigh = -1,
                    AtRiskHigh = (decimal)-0.1,
                    AtRiskLow = -10,
                    CriticalLow = -100
                }
            };
            var command = new SetTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ContainsErrors(expectedErrors, ex.Errors));
        }

        [Test]
        public async Task ShouldRequireExistingPatient()
        {
            var clinicPatientId = TestConstants.AllOnesGuid;
            var patientId = TestConstants.AllTwosGuid;
            var noMatchingPatientId = TestConstants.AllThreesGuid;

            var expectedErrorMessage = $"ClinicPatient not found for ClinicPatientId {noMatchingPatientId}";

            await Testing.AddAsync(new Patient { Id = patientId });
            await Testing.AddAsync(new ClinicPatient { ClinicPatientId = clinicPatientId, PatientId = patientId });

            var targetRanges = new List<TargetRange>
            {
                new TargetRange
                {
                    ObservationCode = "someObservationCode",
                    Unit = "someUnit",
                    CriticalHigh = 1,
                    AtRiskHigh = 2,
                    AtRiskLow = 3,
                    CriticalLow = 4
                }
            };
            var command = new SetTargetRangesCommand
            {
                ClinicPatientId = noMatchingPatientId,
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>().Where(ex => ex.Message.Equals(expectedErrorMessage));
        }

        [Test]
        public async Task ShouldAddValidTargetRanges()
        {
            var clinicPatientId = TestConstants.AllOnesGuid;
            var patientId = TestConstants.AllTwosGuid;

            await Testing.AddAsync(new Patient { Id = patientId });
            await Testing.AddAsync(new ClinicPatient { ClinicPatientId = clinicPatientId, PatientId = patientId });

            var diastolicRange = new TargetRange
            {
                ObservationCode = "blood-pressure-diastolic",
                Unit = "mmHg",
                CriticalHigh = 110,
                AtRiskHigh = 90,
                AtRiskLow = 60,
                CriticalLow = 50
            };

            var systolicRange = new TargetRange
            {
                ObservationCode = "blood-pressure-systolic",
                Unit = "mmHg",
                CriticalHigh = 180,
                AtRiskHigh = 140,
                AtRiskLow = 90,
                CriticalLow = 80
            };

            var expectedTargetRanges = new List<TargetRange> { diastolicRange, systolicRange };

            var command = new SetTargetRangesCommand
            {
                ClinicPatientId = clinicPatientId,
                TargetRanges = expectedTargetRanges
            };

            await Testing.SendAsync(command);

            var actualTargetRanges = await Testing.GetAll<TargetRange>();

            var actualDiastolicRange = actualTargetRanges.FirstOrDefault(r => r.ObservationCode == diastolicRange.ObservationCode);
            var actualSystolicRange = actualTargetRanges.FirstOrDefault(r => r.ObservationCode == systolicRange.ObservationCode);

            Assert.IsTrue(RangesAreEqual(diastolicRange, actualDiastolicRange));
            Assert.IsTrue(RangesAreEqual(systolicRange, actualSystolicRange));

            foreach (var range in actualTargetRanges)
                Assert.AreEqual(clinicPatientId, range.ClinicPatientId);
        }

        private bool ContainsErrors(IDictionary<string, string> expectedErrors, IDictionary<string, string[]> actualErrors)
        {
            foreach (var error in expectedErrors)
            {
                if (!actualErrors.ContainsKey(error.Key) || !actualErrors[error.Key].Contains(error.Value))
                    return false;
            }

            return true;
        }

        private bool RangesAreEqual(TargetRange expected, TargetRange actual)
        {
            return actual != null &&
                expected.ObservationCode == actual.ObservationCode &&
                expected.Unit == actual.Unit &&
                expected.CriticalHigh == actual.CriticalHigh &&
                expected.AtRiskHigh == actual.AtRiskHigh &&
                expected.AtRiskLow == actual.AtRiskLow &&
                expected.CriticalLow == actual.CriticalLow;
        }
    }
}
