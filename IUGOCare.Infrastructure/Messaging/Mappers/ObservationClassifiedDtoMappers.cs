using IUGOCare.Application.Observations.Commands.ClassifyObservation;
using IUGOCare.Messages.ClinicalToPatient.Observations;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class ObservationClassifiedDtoMappers
    {
        public static ClassifyObservationCommand MapToClassifyObservationCommand(this ObservationClassifiedDto dto)
        {
            return new ClassifyObservationCommand
            {
                Id = dto.Id,
                SourceId = dto.SourceId,
                ObservationStatus = dto.ObservationStatus,
                ObservationLevel = dto.ObservationLevel,
            };
        }
    }
}
