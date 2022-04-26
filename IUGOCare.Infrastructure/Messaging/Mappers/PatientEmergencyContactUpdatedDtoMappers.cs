using IUGOCare.Application.Patients.Commands.AddOrUpdateEmergencyContact;
using IUGOCare.Messages.ClinicalToPatient.Patients;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class PatientEmergencyContactUpdatedDtoMappers
    {
        public static AddOrUpdateEmergencyContactCommand MapToAddOrUpdateEmergencyContactCommand(this PatientEmergencyContactUpdatedDto dto)
        {
            return new AddOrUpdateEmergencyContactCommand
            {
                ClinicPatientId = dto.PatientId,
                ContactName = dto.FullName,
                Phone = dto.PhoneNumber,
                Relationship = dto.Relationship
            };
        }
    }
}
