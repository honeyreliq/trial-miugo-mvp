using IUGOCare.Application.Patients.Commands.UpdateEmailFromExternalSystem;
using IUGOCare.Messages.ClinicalToPatient.Patients;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class PatientEmailUpdatedDtoMappers
    {
        public static UpdateEmailFromExternalSystemCommand MapToUpdateEmailFromExternalSystemCommand(this PatientEmailUpdatedDto dto)
        {
            return new UpdateEmailFromExternalSystemCommand(dto.PatientId, dto.Email);
        }
    }
}
