using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Auth0.Core.Exceptions;
using Auth0.ManagementApi.Models;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Common.Models;
using MediatR;

namespace IUGOCare.Application.Patients.Commands.UpdatePassword
{
    public class UpdatePasswordCommand : IRequest
    {
        public Guid? PatientId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISendEmailService _sendEmailService;

        public UpdatePasswordCommandHandler(IApplicationDbContext context,
                                            IIdentityService identityService,
                                            ICurrentUserService currentUserService,
                                            ISendEmailService sendEmailService)
        {
            _context = context;
            _identityService = identityService;
            _sendEmailService = sendEmailService;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients.FindAsync(request.PatientId);
            var clinicPatient = _context.ClinicPatients.OrderBy(cp => cp.Created).FirstOrDefault(cp => cp.PatientId == request.PatientId);
            if (patient is null || clinicPatient is null)
            {
                throw new NotFoundException(nameof(UpdatePasswordCommand), request.PatientId);
            }

            if (patient.Auth0Id != null)
            {
                var auth0User = await _identityService.GetUserAsync(patient.Auth0Id);
                if (auth0User == null)
                {
                    throw new NotFoundException(nameof(User), patient.Auth0Id);
                }

                var passwordUpdated = false;

                try
                {
                    // Update the Auth0 password
                    passwordUpdated = await _identityService.UpdatePasswordAsync(_currentUserService.UserId, request.NewPassword);
                }
                catch (ErrorApiException ex)
                {
                    var errors = new Dictionary<string, string[]>();

                    if (ex.Message.StartsWith("PasswordStrengthError"))
                    {
                        errors.Add("PasswordRequirements", EmailTemplateConfiguration.GetResourceStrings("PasswordRequirement_", patient.PatientLanguage).ToArray());
                    }

                    throw new PasswordTooWeakException(EmailTemplateConfiguration.GetResourceString("PasswordStrengthError", patient.PatientLanguage), errors);
                }

                if (passwordUpdated)
                {
                    var emailAddress = patient.EmailAddress;

                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        var greeting = EmailTemplateConfiguration.GetResourceString("GenericEmailGreeting", patient.PatientLanguage);
                        greeting = greeting.Replace("{{PatientName}}", clinicPatient.FullName);

                        var emailConfig = new EmailSendConfig
                        {
                            Subject = EmailTemplateConfiguration.GetResourceString("PasswordChangedSubject", patient.PatientLanguage),
                            ToEmail = emailAddress,
                            EmailGreeting = greeting,
                            EmailLines = EmailTemplateConfiguration.GetResourceStrings("PasswordChangedBody", patient.PatientLanguage),
                        };

                        var dynamicData = _sendEmailService.BuildDynamicTemplateData(emailConfig, patient.PatientLanguage);            
                        await _sendEmailService.SendEmail(emailConfig, EmailTemplateConfiguration.EmailTemplate, dynamicData);
                    }
                }
            }

           return Unit.Value;
        }
    }
}
