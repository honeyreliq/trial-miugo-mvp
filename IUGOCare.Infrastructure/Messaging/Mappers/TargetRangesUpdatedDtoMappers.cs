using System.Collections.Generic;
using IUGOCare.Application.TargetRanges.Commands.UpdateTargetRanges;
using IUGOCare.Domain.Entities;
using IUGOCare.Messages.ClinicalToPatient.TargetRanges;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class TargetRangesUpdatedDtoMappers
    {
        public static UpdateTargetRangesCommand MapToUpdateTargetRangesCommand(this TargetRangesUpdatedDto dto)
        {
            var targetRanges = new List<TargetRange>();

            foreach (var range in dto.TargetRanges)
            {
                targetRanges.Add(range.MapToTargetRange());
            }

            return new UpdateTargetRangesCommand
            {
                ClinicPatientId = dto.PatientId,
                TargetRanges = targetRanges
            };
        }

        public static TargetRange MapToTargetRange(this TargetRangesUpdatedDto.TargetRangeDto dto)
        {
            return new TargetRange()
            {
                ObservationCode = dto.ObservationCode,
                Unit = dto.Unit,
                CriticalHigh = dto.CriticalHigh,
                AtRiskHigh = dto.AtRiskHigh,
                AtRiskLow = dto.AtRiskLow,
                CriticalLow = dto.CriticalLow
            };
        }
    }
}
