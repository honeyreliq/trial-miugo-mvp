using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Common.Models;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace IUGOCare.Application.Patients.Commands.UpdateEmailAddress
{
    public class PatientRequestUpdateEmailCommand : IRequest
    {
        public Guid PatientId { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
    }

    public class PatientRequestUpdateEmailCommandHandler : IRequestHandler<PatientRequestUpdateEmailCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ISendEmailService _sendEmailService;
        private readonly IActivationCode _activationCodeService;
        private readonly IConfiguration _configuration;

        public PatientRequestUpdateEmailCommandHandler(IApplicationDbContext context, 
            ISendEmailService sendEmailService, 
            IActivationCode activationCodeService,
            IConfiguration configuration)
        {
            _context = context;
            _sendEmailService = sendEmailService;
            _activationCodeService = activationCodeService;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(PatientRequestUpdateEmailCommand request, CancellationToken cancellationToken)
        {
            // Create update email "ticket" request
            string token = await CreateUpdateEmailRequest(request, request.PatientId);
            // Email Patient at new email address
            await NotifyPatientOfPendingEmailChange(request, token);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task NotifyPatientOfPendingEmailChange(PatientRequestUpdateEmailCommand request, string token)
        {
            var patient = _context.Patients.Where(p => p.Id == request.PatientId).FirstOrDefault();
            var clinicPatient = _context.ClinicPatients.Where(p => p.PatientId == request.PatientId).OrderBy(cp => cp.Created).FirstOrDefault();
            var subject = EmailTemplateConfiguration.GetResourceString("EmailUpdateRequestSubject", patient.PatientLanguage);

            var greeting = EmailTemplateConfiguration.GetResourceString("GenericEmailGreeting", patient.PatientLanguage);
            greeting = greeting.Replace("{{PatientName}}", clinicPatient.FullName);

            string appUrl = _configuration.GetSection("AppUrl").Get<string>();
            
            var config = new EmailSendConfig
            {
                Subject = subject,
                ToEmail = request.EmailAddress,
                EmailGreeting = greeting,
                EmailLines = EmailTemplateConfiguration.GetResourceStrings("EmailUpdateRequestBody", patient.PatientLanguage),
                Url = $"{appUrl}/UpdateEmailAddress/{token}",
            };

            var templateData = _sendEmailService.BuildDynamicTemplateData(config, patient.PatientLanguage);
            await _sendEmailService.SendEmail(config, EmailTemplateConfiguration.EmailTemplate, templateData);
        }

        private async Task<string> CreateUpdateEmailRequest(PatientRequestUpdateEmailCommand request, Guid patientId)
        {
            _context.UpdateEmailRequests.RemoveRange(
                _context.UpdateEmailRequests.Where(u => u.PatientId == patientId)
            );

            var uer = new UpdateEmailRequest()
            {
                PatientId = patientId,
                EmailAddress = request.EmailAddress,
                ExpirationDate = DateTimeOffset.UtcNow.AddDays(1),
                Token = _activationCodeService.GenerateNewActivationCode(),
            };
            await _context.UpdateEmailRequests.AddAsync(uer);

            return uer.Token;
        }
    }
}
