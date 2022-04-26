using System.Threading.Tasks;
using System.Collections.Generic;
using IUGOCare.Application.Common.Models;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Common.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmail(EmailSendConfig emailSendConfig);
        Task SendEmail(EmailSendConfig emailSendConfig, string emailTemplate, object dynamicTemplateData);
        Task SendPatientOnboardingEmail(Patient patient, ClinicPatient clinicPatient, Activation activation);
        Dictionary<string, object> BuildDynamicTemplateData(EmailSendConfig emailSendConfig, string language);
    }
}
