using System;
using System.Linq;
using IUGOCare.Application.TargetRanges.Commands.SetTargetRanges;
using IUGOCare.Domain.Entities;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.TargetRanges;
using NUnit.Framework;
using static IUGOCare.Infrastructure.Messaging.Mappers.TargetRangesCreatedDtoMappers;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.Mappers
{
    [TestFixture]
    public class TargetRangesCreatedDtoMappersTests
    {
        [Test]
        public void MapToSetTargetRangesCommand_ValidProperties_ReturnsValidCommand()
        {
            // Arrange
            var clinicPatientId = Guid.NewGuid();
            var observationCodeDiastolic = "blood-pressure-diastolic";
            var observationCodeSystolic = "blood-pressure-systolic";
            var unit = "mmHg";

            var externalEvent = new TargetRangesCreatedDto
            {
                PatientId = clinicPatientId
            };

            var externalDiastolicRange = new TargetRangesCreatedDto.TargetRangeDto()
            {
                ObservationCode = observationCodeDiastolic,
                Unit = unit,
                CriticalHigh = 100,
                AtRiskHigh = 90,
                AtRiskLow = 60,
                CriticalLow = 50
            };

            var externalSystolicRange = new TargetRangesCreatedDto.TargetRangeDto()
            {
                ObservationCode = observationCodeSystolic,
                Unit = unit,
                CriticalHigh = 180,
                AtRiskHigh = 140,
                AtRiskLow = 90,
                CriticalLow = 80
            };

            externalEvent.TargetRanges.Add(externalDiastolicRange);
            externalEvent.TargetRanges.Add(externalSystolicRange);

            // Act
            var result = externalEvent.MapToSetTargetRangesCommand();

            // Assert
            Assert.IsInstanceOf<SetTargetRangesCommand>(result);
            Assert.That(result.ClinicPatientId.Equals(clinicPatientId));
            Assert.That(result.TargetRanges.Count == 2);
            Assert.That(result.TargetRanges.Any(tr => tr.ObservationCode == observationCodeSystolic));
            Assert.That(result.TargetRanges.Any(tr => tr.ObservationCode == observationCodeDiastolic));
        }

        [Test]
        public void TargetRangesCreated_MapToTargetRange_ValidProperties_ReturnsValidTargetRange()
        {
            // Arrange
            var observationCodeDiastolic = "blood-pressure-diastolic";
            var unit = "mmHg";
            var criticalHigh = 100;
            var atRiskHigh = 90;
            var atRiskLow = 60;
            var criticalLow = 50;

            var externalTargetRange = new TargetRangesCreatedDto.TargetRangeDto()
            {
                ObservationCode = observationCodeDiastolic,
                Unit = unit,
                CriticalHigh = criticalHigh,
                AtRiskHigh = atRiskHigh,
                AtRiskLow = atRiskLow,
                CriticalLow = criticalLow
            };

            var expectedTargetRange = new TargetRange
            {
                ObservationCode = observationCodeDiastolic,
                Unit = unit,
                CriticalHigh = criticalHigh,
                AtRiskHigh = atRiskHigh,
                AtRiskLow = atRiskLow,
                CriticalLow = criticalLow
            };

            // Act
            var result = externalTargetRange.MapToTargetRange();

            // Assert
            Assert.That(result.ObservationCode.Equals(expectedTargetRange.ObservationCode));
            Assert.That(result.Unit.Equals(expectedTargetRange.Unit));
            Assert.That(result.CriticalHigh.Equals(expectedTargetRange.CriticalHigh));
            Assert.That(result.AtRiskHigh.Equals(expectedTargetRange.AtRiskHigh));
            Assert.That(result.CriticalLow.Equals(expectedTargetRange.CriticalLow));

        }
    }
}
