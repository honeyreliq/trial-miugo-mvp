using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Common.Models;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IUGOCare.Application.Patients.Commands.UpdateEmailAddress
{
    public class UpdateEmailAddressCommand : IRequest
    {
        public Guid PatientId { get; set; }

        public string EmailAddress { get; set; }
    }

    public class UpdateEmailAddressCommandHandler : IRequestHandler<UpdateEmailAddressCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISendEmailService _sendEmailService;
        private readonly IActivationCode _activationCodeService;
        private readonly ILogger<UpdateEmailAddressCommand> _logger;

        public UpdateEmailAddressCommandHandler(IApplicationDbContext context, 
            IIdentityService identityService, 
            ISendEmailService sendEmailService, 
            IActivationCode activationCodeService,
            ILogger<UpdateEmailAddressCommand> logger)
        {
            _context = context;
            _identityService = identityService;
            _sendEmailService = sendEmailService;
            _activationCodeService = activationCodeService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateEmailAddressCommand request, CancellationToken cancellationToken)
        {
            var patientId = request.PatientId;
            
            // Update email address for Patients table
            var patient = await _context.Patients.Include(p => p.Clinics).FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
            {
                throw new NotFoundException(nameof(Patient), patientId);
            }

            var patientWithEmailAddress = await _context.Patients.Where(p => p.EmailAddress == request.EmailAddress && p.Id != patientId)
                                                                 .ToListAsync();

            if (patientWithEmailAddress.Count > 0)
            {
                _logger.LogError("Error in {0}: Email address is already registered. " +
                    "ClinicPatientId: {1}, EmailAddress: {2}",
                    nameof(UpdateEmailAddressCommand),
                    request.PatientId,
                    request.EmailAddress);
                return Unit.Value;
            }

            var previousEmailAddress = patient.EmailAddress;
            patient.EmailAddress = request.EmailAddress;

            if (patient.Auth0Id != null)
            {
                var auth0User = await _identityService.GetUserAsync(patient.Auth0Id);
                if (auth0User == null)
                {
                    throw new NotFoundException(nameof(User), patient.Auth0Id);
                }
                // Update the Auth0 email address
                await _identityService.UpdateUserAsync(auth0User.UserId, request.EmailAddress);
            }

            if (patient.Active)
            {
                await NotifyPatientOfEmailChange(patient, patient.PrimaryClinicPatient, previousEmailAddress);
            }
            else
            {
                // Invalidate existing activation code
                var activation = await _context.Activations.FindAsync(patient.Id);

                if (activation != null)
                    _context.Activations.Remove(activation);

                // Generate new activation code
                var activationCode = _activationCodeService.GenerateNewActivationCode();
                activation = new Activation { PatientId = patient.Id, ActivationCode = activationCode };
                await _context.Activations.AddAsync(activation);

                await _sendEmailService.SendPatientOnboardingEmail(patient, patient.PrimaryClinicPatient, activation);
            }

            // Remove any existing "change email" tokens
            var requests = _context.UpdateEmailRequests.Where(p => p.PatientId == patientId);
            _context.UpdateEmailRequests.RemoveRange(requests);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task NotifyPatientOfEmailChange(Patient patient, ClinicPatient clinicPatient, string previousEmailAddress)
        {
            var subject = EmailTemplateConfiguration.GetResourceString("EmailUpdatedSubject", patient.PatientLanguage);

            var greeting = EmailTemplateConfiguration.GetResourceString("GenericEmailGreeting", patient.PatientLanguage);
            greeting = greeting.Replace("{{PatientName}}", clinicPatient.FullName);

            var config = new EmailSendConfig
            {
                Subject = subject,
                ToEmail = previousEmailAddress,
                EmailGreeting = greeting,
                EmailLines = EmailTemplateConfiguration.GetResourceStrings("EmailUpdatedBody", patient.PatientLanguage),
            };

            var templateData = _sendEmailService.BuildDynamicTemplateData(config, patient.PatientLanguage);
            await _sendEmailService.SendEmail(config, EmailTemplateConfiguration.EmailTemplate, templateData);
        }
    }
}
