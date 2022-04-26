using System;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class Activation : AuditableEntity
    {
        public Activation() : base()
        {
            
        }

        /// <summary>
        /// The unique ID of a patient, from the Patients table
        /// </summary>
        public Guid PatientId { get; set; }

        public string ActivationCode { get; set; }

        public DateTimeOffset ExpirationDate { get; set; }

        public Patient Patient { get; }
    }
}
