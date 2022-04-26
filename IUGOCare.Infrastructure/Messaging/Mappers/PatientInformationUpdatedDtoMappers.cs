using System;
using IUGOCare.Application.Patients.Commands.UpdatePatientInformation;
using IUGOCare.Messages.ClinicalToPatient.Patients;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class PatientInformationUpdatedDtoMappers
    {
        public static UpdatePatientInformationCommand MapToUpdatePatientInformationCommand(this PatientInformationUpdatedDto dto)
        {
            return new UpdatePatientInformationCommand
            {
                ClinicPatientId = dto.PatientId,
                GivenName = dto.GivenName,
                MiddleName = dto.MiddleName,
                FamilyName = dto.FamilyName,
                Phone = dto.PhoneNumber,
                BirthDate = dto.DateOfBirth,
                AddressLine1 = dto.Address?.Address1,
                AddressLine2 = dto.Address?.Address2,
                City = dto.Address?.City,
                State = dto.Address?.State,
                ZipCode = dto.Address?.PostalCode,
                Country = dto.Address?.Country,
            };
        }
    }
}
