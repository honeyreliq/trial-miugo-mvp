using IUGOCare.Application.PatientCareManagementPrograms.Commands.EnrollNewPatientFromExternalSystem;
using IUGOCare.Messages.ClinicalToPatient.Patients;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class NewPatientEnrolledInCareProgramsDtoMappers
    {
        public static EnrollNewPatientFromExternalSystemCommand MapToEnrollNewPatientFromExternalSystemCommand(this NewPatientEnrolledInCareProgramsDto dto)
        {
            return new EnrollNewPatientFromExternalSystemCommand(dto.PatientId, dto.CarePrograms);
        }
    }
}
