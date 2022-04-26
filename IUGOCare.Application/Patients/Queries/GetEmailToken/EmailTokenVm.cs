using System;

namespace IUGOCare.Application.Patients.Queries.GetEmailToken
{
    public class EmailTokenVm
    {
        public string Token { get; set; }
        public DateTimeOffset TokenExpiration { get; set; }
        public Guid PatientId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string NewEmailAddress { get; set; }
    }
}
