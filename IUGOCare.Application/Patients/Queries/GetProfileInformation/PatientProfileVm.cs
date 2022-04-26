using System;
using System.Collections.Generic;
using AutoMapper;
using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;
using static System.String;

namespace IUGOCare.Application.Patients.Queries.GetProfileInformation
{
    public class PatientProfileVm : IMapFrom<ClinicPatient>
    {
        public string PatientName { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public EmergencyContactDto EmergencyContact { get; set; }
        public IList<PatientCareManagementProgramDto> PatientPrograms { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ClinicPatient, PatientProfileVm>()
                .ForMember(p => p.PatientName, opt => opt.MapFrom(p => p.FullName))
                .ForMember(p => p.PatientPrograms, opt => opt.Ignore())
                .ForMember(
                    p => p.EmergencyContact,
                    opt => opt.MapFrom(src => src.EmergencyContact)
                );
        }
    }
}
