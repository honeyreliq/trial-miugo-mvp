using System;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class PatientCareManagementProgram : AuditableEntity
    {
        public Guid ClinicPatientId { get; set; }
        public Guid CareManagementProgramId { get; set; }
        // According to IV5-419 requeriments this value must remain nullable 
        public Guid? BillingProviderId { get; set; }

        public Provider BillingProvider { get; }
        public ClinicPatient ClinicPatient { get; }
        public CareManagementProgram CareManagementProgram { get; }
    }
}
