using System;
using System.Collections.Generic;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class Provider : AuditableEntity
    {
        public Provider()
        {
            PatientCareManagementPrograms = new List<PatientCareManagementProgram>();
            ClinicPatients = new List<ClinicPatient>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
        public Guid OrganizationId { get; set; }

        public IList<PatientCareManagementProgram> PatientCareManagementPrograms { get; }
        public IList<ClinicPatient> ClinicPatients { get; }
        public Organization Organization { get; }
    }
}
