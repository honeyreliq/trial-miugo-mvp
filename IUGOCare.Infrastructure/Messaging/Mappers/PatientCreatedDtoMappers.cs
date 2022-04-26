using IUGOCare.Application.Patients.Commands.RegisterPatient;
using IUGOCare.Messages.ClinicalToPatient.Patients;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class PatientCreatedDtoMappers
    {
        public static RegisterPatientCommand MapToRegisterPatientCommand(this PatientCreatedDto dto)
        {
            return new RegisterPatientCommand
            {
                ClinicPatientId = dto.PatientId,
                ClinicId = dto.ClinicId,
                ClinicSubdomain = dto.ClinicSubdomain,
                PatientTimeZone = dto.PatientTimeZone,
                PatientWindowsTimeZone = dto.PatientWindowsTimeZone,
                GivenName = dto.GivenName,
                MiddleName = dto.MiddleName,
                FamilyName = dto.FamilyName,
                EmailAddress = dto.EmailAddress,
                PatientLanguage = dto.PatientLanguage,
                PhoneNumber = dto.PhoneNumber,
                BirthDate = dto.DateOfBirth,
                AddressLine1 = dto.Address?.Address1,
                AddressLine2 = dto.Address?.Address2,
                City = dto.Address?.City,
                State = dto.Address?.State,
                ZipCode = dto.Address?.PostalCode,
                Country = dto.Address?.Country,
                EmergencyContactName = dto.EmergencyContact?.FullName,
                EmergencyContactPhoneNumber = dto.EmergencyContact?.PhoneNumber,
                EmergencyContactRelationship = dto.EmergencyContact?.Relationship
            };
        }
    }
}
