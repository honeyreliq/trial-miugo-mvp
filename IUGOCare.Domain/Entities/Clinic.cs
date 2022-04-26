using System;
using System.Collections.Generic;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class Clinic : AuditableEntity
    {
        public Clinic()
        {
            ClinicPatients = new List<ClinicPatient>();
        }

        public Guid Id { get; set; }
        public string Subdomain { get; set; }
        public bool EmailsEnabled { get; set; }

        public IList<ClinicPatient> ClinicPatients { get; }
    }
}
