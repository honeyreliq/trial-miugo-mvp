using IUGOCare.Application.PatientCareManagementPrograms.Commands.SetPatientCareManagementProgramEnrollment;
using IUGOCare.Messages.ClinicalToPatient.Patients;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class PatientCareProgramEnrollmentUpdatedDtoMappers
    {
        public static SetPatientCareManagementProgramEnrollmentCommand MapToSetPatientCareManagementEnrollmentCommand(this PatientCareProgramEnrollmentUpdatedDto dto)
        {
            return new SetPatientCareManagementProgramEnrollmentCommand
            {
                ClinicPatientId = dto.PatientId,
                CareProgramShortName = dto.CareProgram,
                IsEnrolled = dto.IsEnrolled
            };
        }
    }


}
