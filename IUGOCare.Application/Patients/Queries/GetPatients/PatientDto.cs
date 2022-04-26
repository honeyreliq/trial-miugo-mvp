using System;
using System.Linq;
using AutoMapper;
using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace IUGOCare.Application.Patients.Queries.GetPatients
{
    public class PatientDto : IMapFrom<Patient>
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string ActivationCode { get; set; }
        public string PatientLanguage { get; set; }
        public string TimeZone { get; set; }
        public string WindowsTimeZone { get; set; }
        public string PatientTheme { get; set; }
        public bool Tooltips { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string MedicaidNumber { get; set; }
        public string MedicareNumber { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string InsuranceNumber { get; set; }

        public Guid ClinicPatientId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Patient, PatientDto>()
                .ForMember(d => d.ActivationCode, opt => { 
                    opt.PreCondition(src => (src.Activation != null));  
                    opt.MapFrom(s => s.Activation.ActivationCode); 
                }).ForMember(d => d.ClinicPatientId, opt => {
                    opt.PreCondition(src => src.Clinics.Any());
                    opt.MapFrom(s => s.Clinics.FirstOrDefault().ClinicPatientId);
                })
                .ForMember(d => d.TimeZone, opt => opt.MapFrom(p => p.PrimaryClinicPatient.TimeZone))
                .ForMember(d => d.WindowsTimeZone, opt => opt.MapFrom(p => p.PrimaryClinicPatient.WindowsTimeZone))
                .ForMember(d => d.MedicaidNumber, opt => opt.MapFrom(p => p.PrimaryClinicPatient.MedicaidNumber))
                .ForMember(d => d.MedicareNumber, opt => opt.MapFrom(p => p.PrimaryClinicPatient.MedicareNumber))
                .ForMember(d => d.MedicalRecordNumber, opt => opt.MapFrom(p => p.PrimaryClinicPatient.MedicalRecordNumber))
                .ForMember(d => d.InsuranceNumber, opt => opt.MapFrom(p => p.PrimaryClinicPatient.InsuranceNumber))
                ;
        }
    }
}
