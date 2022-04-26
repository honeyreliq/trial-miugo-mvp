using System;
using System.Collections.Generic;
using System.Linq;
using IUGOCare.Domain.Common;
using IUGOCare.Domain.Common.Constants;

namespace IUGOCare.Domain.Entities
{
    public class Patient : AuditableEntity
    {
        public Patient() : base()
        {
            Clinics = new List<ClinicPatient>();
        }

        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string Auth0Id { get; set; }
        public bool Active { get; set; }
        public bool AllowMarketingEmails { get; set; }
        public string PatientLanguage { get; private set; }
        public string PatientTheme { get; set; }
        public bool Tooltips { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public IList<ClinicPatient> Clinics { get; }
        public Activation Activation { get; }
        public UpdateEmailRequest UpdateEmailRequest { get; }

        public void SetLanguage(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                PatientLanguage = Languages.EnglishLanguage;
                return;
            }

            switch (language.ToUpper())
            {
                case (Languages.SpanishLanguage):
                    PatientLanguage = language.ToUpper();
                    break;
                default:
                    PatientLanguage = Languages.EnglishLanguage;
                    break;
            }
        }

        public ClinicPatient PrimaryClinicPatient
        {
            get
            {
                return Clinics.OrderBy(c => c.Created).FirstOrDefault();
            }
        }
    }
}
