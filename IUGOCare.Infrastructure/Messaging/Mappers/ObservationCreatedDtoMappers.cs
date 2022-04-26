using System.Linq;
using IUGOCare.Application.Observations.Commands.CreateObservation;
using IUGOCare.Messages.ClinicalToPatient.Observations;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class ObservationCreatedDtoMappers
    {
        public static CreateObservationCommand MapToCreateObservationCommand(this ObservationCreatedDto dto)
        {
            var command = new CreateObservationCommand
            {
                ObservationId = dto.Id,
                ClinicPatientId = dto.PatientId,
                ObservationCode = dto.ObservationCode,
                EffectiveDate = dto.EffectiveDate,
                Source = dto.Source,
                ObservationStatus = dto.ObservationStatus,
                ObservationLevel = dto.ObservationLevel,
                IsReviewed = dto.IsReviewed,
                IsReviewedDate = dto.IsReviewedDate,
                ReviewedByName = dto.ReviewedByName,
                Manufacturer = dto.Manufacturer,
            };

            command.ObservationDataList = dto.ObservationData.Select(
                od => new ObservationDataItem
                {
                    ObservationCode = od.ObservationCode,
                    Unit = od.Unit,
                    Value = od.Value,
                }
            ).ToList();

            return command;
        }
    }
}
