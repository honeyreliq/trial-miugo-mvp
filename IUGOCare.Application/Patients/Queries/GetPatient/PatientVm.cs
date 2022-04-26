using System;
using System.Collections.Generic;

namespace IUGOCare.Application.Patients.Queries.GetPatient
{
    public class PatientVm
    {
        public Guid Id { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string FamilyName { get; set; }
        public string EmailAddress { get; set; }
        public string PatientLanguage { get; set; }
        public string TimeZone { get; set; }
        public string WindowsTimeZone { get; set; }
        public string PatientTheme { get; set; }
        public bool Tooltips { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MedicaidNumber { get; set; }
        public string MedicareNumber { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public IList<Guid> ClinicPatientIds { get; set; }
    }
}
