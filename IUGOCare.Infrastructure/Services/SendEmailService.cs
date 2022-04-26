using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Common.Models;
using IUGOCare.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IUGOCare.Infrastructure.Services
{
    public class SendEmailService : ISendEmailService
    {
        private readonly ILogger<SendEmailService> _logger;
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;
        private readonly string _sendGridSender;
        private readonly string _appUrl;

        public SendEmailService(ILogger<SendEmailService> logger, ISendGridClient sendGridClient, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _sendGridClient = sendGridClient;
            _sendGridSender = _configuration.GetSection("SendGrid:Sender").Value;
            _appUrl = _configuration.GetSection("AppUrl").Value;
        }

        public async Task SendEmail(EmailSendConfig emailSendConfig, string emailTemplate, object dynamicTemplateData)
        {
            try
            {
                string sendGridTemplateId = _configuration.GetSection(emailTemplate).Value;
                SendGridMessage sendGridMessage = BuildMessage(emailSendConfig, sendGridTemplateId, dynamicTemplateData);
                Response response = await _sendGridClient.SendEmailAsync(sendGridMessage);
                if (response.StatusCode != HttpStatusCode.Accepted)
                    _logger.LogError($"Error sending email to: {emailSendConfig.ToEmail}. Response status code: {response.StatusCode}",
                        emailSendConfig.ToEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error sending the email to {ToEmail}", emailSendConfig.ToEmail);
            }
        }

        public async Task SendEmail(EmailSendConfig emailSendConfig)
        {
            try
            {
                SendGridMessage sendGridMessage = MailHelper.CreateSingleEmail(
                    new EmailAddress(_sendGridSender), 
                    new EmailAddress(emailSendConfig.ToEmail), 
                    emailSendConfig.Subject, 
                    emailSendConfig.BodyPlainText, 
                    emailSendConfig.BodyHtmlText);
                Response response = await _sendGridClient.SendEmailAsync(sendGridMessage);
                if (response.StatusCode != HttpStatusCode.Accepted)
                    _logger.LogError($"Error sending email to: {emailSendConfig.ToEmail}. Response status code: {response.StatusCode}",
                        emailSendConfig.ToEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error sending the email to {ToEmail}", emailSendConfig.ToEmail);
            }
        }

        public async Task SendPatientOnboardingEmail(Patient patient, ClinicPatient clinicPatient, Activation activation)
        {
            var greeting = EmailTemplateConfiguration.GetResourceString("GenericEmailGreeting", patient.PatientLanguage);
            greeting = greeting.Replace("{{PatientName}}", clinicPatient.FullName);

            var emailConfig = new EmailSendConfig
            {
                Subject = EmailTemplateConfiguration.GetResourceString("SendPatientOnboardingEmailSubject", patient.PatientLanguage),
                ToEmail = patient.EmailAddress,
                Url = $"{_appUrl}/activate-account/{activation.ActivationCode}",
                EmailLines = EmailTemplateConfiguration.GetResourceStrings("SendPatientOnboardingEmailBody", patient.PatientLanguage),
                EmailGreeting = greeting,
                IncludeButton = true,
                ButtonText = EmailTemplateConfiguration.GetResourceString("SendPatientOnboardingEmailButtonText", patient.PatientLanguage),
            };

            var dynamicData = BuildDynamicTemplateData(emailConfig, patient.PatientLanguage);
            
            await SendEmail(emailConfig, EmailTemplateConfiguration.EmailTemplate, dynamicData);
        }

        public Dictionary<string, object> BuildDynamicTemplateData(EmailSendConfig emailSendConfig, string language)
        {
            var culture = new CultureInfo(language);

            IEnumerable<string> lines = new List<string>();
            var now = DateTimeOffset.UtcNow;
            if (emailSendConfig.EmailLines != null)
            {
                lines = emailSendConfig.EmailLines
                    .Select(l => {
                        return l.Replace("{{Url}}", emailSendConfig.Url)
                            .Replace("{{DateStamp}}", now.ToString("MMMM dd, yyyy", culture))
                            .Replace("{{TimeStamp}}", now.ToString("HH:mm", culture));
                    });
            }

            var dynamicTemplateData = new Dictionary<string, object>
                {
                    {"Subject", emailSendConfig.Subject },
                    {"Url", emailSendConfig.Url},
                    {"EmailLines", lines},
                    {"EmailGreeting", emailSendConfig.EmailGreeting},
                    {"IncludeButton", emailSendConfig.IncludeButton},
                    {"ButtonText", emailSendConfig.ButtonText},
                };
            return dynamicTemplateData;
        }

        private SendGridMessage BuildMessage(EmailSendConfig emailSendConfig, string sendGridTemplateId, object dynamicTemplateData)
        {
            SendGridMessage msg = MailHelper.CreateSingleTemplateEmail(new EmailAddress(_sendGridSender), new EmailAddress(emailSendConfig.ToEmail), sendGridTemplateId, dynamicTemplateData);
            return msg;
        }
    }
}
