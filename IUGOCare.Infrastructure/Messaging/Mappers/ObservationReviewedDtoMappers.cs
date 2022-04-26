using IUGOCare.Application.Observations.Commands.ReviewObservation;
using IUGOCare.Messages.ClinicalToPatient.Observations;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class ObservationReviewedDtoMappers
    {
        public static ReviewObservationCommand MapToReviewObservationCommand(this ObservationReviewedDto dto)
        {
            return new ReviewObservationCommand
            {
                Id = dto.Id,
                SourceId = dto.SourceId,
                IsReviewedDate = dto.IsReviewedDate.Value,
                ReviewedByName = dto.ReviewedByName,
            };
        }
    }
}
