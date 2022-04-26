using System.Collections.Generic;
using IUGOCare.Application.TargetRanges.Commands.SetTargetRanges;
using IUGOCare.Domain.Entities;
using IUGOCare.Messages.ClinicalToPatient.TargetRanges;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class TargetRangesCreatedDtoMappers
    {
        public static SetTargetRangesCommand MapToSetTargetRangesCommand(this TargetRangesCreatedDto dto)
        {
            var targetRanges = new List<TargetRange>();

            foreach (var range in dto.TargetRanges)
            {
                targetRanges.Add(range.MapToTargetRange());
            }

            return new SetTargetRangesCommand
            {
                ClinicPatientId = dto.PatientId,
                TargetRanges = targetRanges
            };
        }

        public static TargetRange MapToTargetRange(this TargetRangesCreatedDto.TargetRangeDto dto)
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
