using System;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class UpdateEmailRequest : AuditableEntity
    {
        /// <summary>
        /// The unique ID of a patient, from the Patients table
        /// </summary>
        public Guid PatientId { get; set; }
        public string Token { get; set; }
        public string EmailAddress { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }

        public Patient Patient { get; }
    }
}
