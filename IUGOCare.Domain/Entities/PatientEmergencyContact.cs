using System;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class PatientEmergencyContact : AuditableEntity
    {
        public Guid ClinicPatientId { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Relationship { get; set; }

        public ClinicPatient ClinicPatient { get; set; }
    }
}
