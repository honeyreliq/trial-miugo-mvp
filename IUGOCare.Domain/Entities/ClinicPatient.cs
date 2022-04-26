using System;
using System.Collections.Generic;
using System.Linq;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class ClinicPatient : AuditableEntity
    {
        public ClinicPatient()
        {
            PatientCareManagementPrograms = new List<PatientCareManagementProgram>();
            TargetRanges = new List<TargetRange>();
        }

        /// <summary>
        /// The unique ID of a patient, from the Patients table
        /// </summary>
        public Guid PatientId { get; set; }
        /// <summary>
        /// The Patient's ID as provided by the v4 clinical system
        /// </summary>
        public Guid ClinicPatientId { get; set; }
        public Guid ClinicId { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string FamilyName { get; set; }
        public string TimeZone { get; set; }
        public string WindowsTimeZone { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MedicaidNumber { get; set; }
        public string MedicareNumber { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public Address Address { get; set; }
        public Guid? PrimaryCareProviderId { get; set; }

        public Patient Patient { get; }
        public Clinic Clinic { get; }
        public Provider PrimaryCareProvider { get; }
        public PatientEmergencyContact EmergencyContact { get; private set; }
        public IList<PatientCareManagementProgram> PatientCareManagementPrograms { get; }
        public IList<TargetRange> TargetRanges { get; }

        public string FullName
        {
            get { return string.Join(" ", new string[] { GivenName, MiddleName, FamilyName }.Where(c => !string.IsNullOrEmpty(c))); }
        }

        public void SetCareManagementProgramEnrollment(Guid careProgramId, bool isEnrolled)
        {
            var existingProgram = PatientCareManagementPrograms.FirstOrDefault(p => p.CareManagementProgramId == careProgramId);
            if (isEnrolled)
            {
                if (existingProgram is null)
                {
                    PatientCareManagementPrograms.Add(new PatientCareManagementProgram
                    {
                        ClinicPatientId = ClinicPatientId,
                        CareManagementProgramId = careProgramId
                    });
                }
            }
            else
            {
                if (existingProgram != null)
                {
                    PatientCareManagementPrograms.Remove(existingProgram);
                }
            }
        }
    
        public void SetEmergencyContact(string contactName, string phoneNumber, string relationship)
        {
            if (contactName is null && phoneNumber is null && relationship is null)
            {
                EmergencyContact = null;
                return;
            }

            if (EmergencyContact is null)
            {
                EmergencyContact = new PatientEmergencyContact();
            }
            EmergencyContact.ClinicPatientId = ClinicPatientId;
            EmergencyContact.ContactName = contactName;
            EmergencyContact.Phone = phoneNumber;
            EmergencyContact.Relationship = relationship;
        }
    }
}
