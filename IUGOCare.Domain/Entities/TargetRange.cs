using System;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class TargetRange : AuditableEntity
    {
        public Guid Id { get; set; }
        public string ObservationCode { get; set; }
        public Guid ClinicPatientId { get; set; }        
        public string Unit { get; set; }
        public decimal CriticalHigh { get; set; }
        public decimal AtRiskHigh { get; set; }
        public decimal AtRiskLow { get; set; }
        public decimal CriticalLow { get; set; }

        public ClinicPatient ClinicPatient { get; }

    }
}
