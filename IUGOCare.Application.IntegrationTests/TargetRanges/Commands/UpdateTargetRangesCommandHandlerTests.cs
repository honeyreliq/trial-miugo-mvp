using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.TargetRanges.Commands.UpdateTargetRanges;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;
namespace IUGOCare.Application.IntegrationTests.TargetRanges.Commands
{
    using static Testing;
    public class UpdateTargetRangesCommandHandlerTests : TestBase
    {
        [Test]
        public async Task ShouldRequireExistingPatient()
        {
            var clinicPatientId = TestConstants.AllOnesGuid;
            var patientId = TestConstants.AllTwosGuid;
            var noMatchingPatientId = TestConstants.AllThreesGuid;

            var expectedErrorMessage = $"ClinicPatientId not found for ClinicPatientId {noMatchingPatientId}";

            await AddAsync(new Patient { Id = patientId });
            await AddAsync(new ClinicPatient { ClinicPatientId = clinicPatientId, PatientId = patientId });

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
            var command = new UpdateTargetRangesCommand
            {
                ClinicPatientId = noMatchingPatientId,
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>().Where(ex => ex.Message.Equals(expectedErrorMessage));
        }

        [Test]
        public async Task ShouldAddOrUpdateTargetRanges()
        {
            var clinicPatientId = TestConstants.AllOnesGuid;
            var patientId = TestConstants.AllTwosGuid;

            var existingDiastolicRange = new TargetRange
            {
                ClinicPatientId = clinicPatientId,
                ObservationCode = "diastolic-blood-pressure",
                Unit = "mmHg",
                CriticalHigh = 110,
                AtRiskHigh = 90,
                AtRiskLow = 60,
                CriticalLow = 50
            };

            var existingSystolicRange = new TargetRange
            {
                ClinicPatientId = clinicPatientId,
                ObservationCode = "systolic-blood-pressure",
                Unit = "mmHg",
                CriticalHigh = 180,
                AtRiskHigh = 140,
                AtRiskLow = 90,
                CriticalLow = 80
            };

            await AddAsync(new Patient { Id = patientId });
            await AddAsync(new ClinicPatient { ClinicPatientId = clinicPatientId, PatientId = patientId });
            await AddAsync(existingDiastolicRange);
            await AddAsync(existingSystolicRange);

            var updatedDiastolicRange = new TargetRange
            {
                ObservationCode = "diastolic-blood-pressure",
                Unit = "mmHg",
                CriticalHigh = 120,
                AtRiskHigh = 100,
                AtRiskLow = 70,
                CriticalLow = 60
            };

            var updatedSystolicRange = new TargetRange
            {
                ObservationCode = "systolic-blood-pressure",
                Unit = "mmHg",
                CriticalHigh = 190,
                AtRiskHigh = 150,
                AtRiskLow = 100,
                CriticalLow = 90
            };

            var newFastingRange = new TargetRange
            {
                ObservationCode = "blood-glucose-fasting",
                Unit = "mg/dL",
                CriticalHigh = 350,
                AtRiskHigh = 131,
                AtRiskLow = 79,
                CriticalLow = 60
            };

            var newNonFastingRange = new TargetRange
            {
                ObservationCode = "blood-glucose-non-fasting",
                Unit = "mg/dL",
                CriticalHigh = 350,
                AtRiskHigh = 180,
                AtRiskLow = 79,
                CriticalLow = 60
            };

            var expectedTargetRanges = new List<TargetRange>
            {
                updatedDiastolicRange,
                updatedSystolicRange,
                newFastingRange,
                newNonFastingRange
            };

            var command = new UpdateTargetRangesCommand
            {
                ClinicPatientId = clinicPatientId,
                TargetRanges = expectedTargetRanges
            };

            await SendAsync(command);

            var actualTargetRanges = await Testing.GetAll<TargetRange>();

            var actualDiastolicRange = actualTargetRanges.FirstOrDefault(r => r.ObservationCode == updatedDiastolicRange.ObservationCode);
            var actualSystolicRange = actualTargetRanges.FirstOrDefault(r => r.ObservationCode == updatedSystolicRange.ObservationCode);
            var actualFastingRange = actualTargetRanges.FirstOrDefault(r => r.ObservationCode == newFastingRange.ObservationCode);
            var actualNonFastingRange = actualTargetRanges.FirstOrDefault(r => r.ObservationCode == newNonFastingRange.ObservationCode);

            Assert.IsTrue(RangesAreEqual(updatedDiastolicRange, actualDiastolicRange));
            Assert.IsTrue(RangesAreEqual(updatedSystolicRange, actualSystolicRange));
            Assert.IsTrue(RangesAreEqual(newFastingRange, actualFastingRange));
            Assert.IsTrue(RangesAreEqual(newNonFastingRange, actualNonFastingRange));

            foreach (var range in actualTargetRanges)
                Assert.AreEqual(clinicPatientId, range.ClinicPatientId);
        }

        [Test]
        public async Task ShouldRemoveUnmatchedExistingTargetRanges()
        {
            // Arrange
            var patientId = TestConstants.AllOnesGuid;
            var clinicPatientId = TestConstants.AllTwosGuid;
            var patient = new Patient { Id = patientId };
            var clinicPatient = new ClinicPatient { PatientId = patientId, ClinicPatientId = clinicPatientId };

            var existingDiastolicRange = new TargetRange
            {
                ClinicPatientId = clinicPatientId,
                ObservationCode = "diastolic-blood-pressure",
                Unit = "mmHg",
                CriticalHigh = 110,
                AtRiskHigh = 90,
                AtRiskLow = 60,
                CriticalLow = 50
            };

            var existingSystolicRange = new TargetRange
            {
                ClinicPatientId = clinicPatientId,
                ObservationCode = "systolic-blood-pressure",
                Unit = "mmHg",
                CriticalHigh = 180,
                AtRiskHigh = 140,
                AtRiskLow = 90,
                CriticalLow = 80
            };

            var existingUnmatchedRange = new TargetRange
            {
                ClinicPatientId = clinicPatientId,
                ObservationCode = "blood-glucose-fasting",
                Unit = "mg/dL",
                CriticalHigh = 350,
                AtRiskHigh = 131,
                AtRiskLow = 79,
                CriticalLow = 60
            };

            await AddAsync(patient);
            await AddAsync(clinicPatient);
            await AddAsync(existingSystolicRange);
            await AddAsync(existingDiastolicRange);
            await AddAsync(existingUnmatchedRange);

            var updatedDiastolicRange = new TargetRange
            {
                ObservationCode = "diastolic-blood-pressure",
                Unit = "mmHg",
                CriticalHigh = 120,
                AtRiskHigh = 100,
                AtRiskLow = 70,
                CriticalLow = 60
            };

            var updatedSystolicRange = new TargetRange
            {
                ObservationCode = "systolic-blood-pressure",
                Unit = "mmHg",
                CriticalHigh = 190,
                AtRiskHigh = 150,
                AtRiskLow = 100,
                CriticalLow = 90
            };

            var expectedTargetRanges = new List<TargetRange>
            {
                updatedDiastolicRange,
                updatedSystolicRange
            };

            var command = new UpdateTargetRangesCommand
            {
                ClinicPatientId = clinicPatientId,
                TargetRanges = expectedTargetRanges
            };

            // Act
            await SendAsync(command);

            var actualTargetRanges = await Testing.GetAll<TargetRange>();

            // Assert
            actualTargetRanges.Should().NotContain(tr => tr.ObservationCode.Equals(existingUnmatchedRange.ObservationCode));
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
