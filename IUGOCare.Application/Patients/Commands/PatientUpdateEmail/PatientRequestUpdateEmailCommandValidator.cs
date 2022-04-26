using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Commands.UpdateEmailAddress
{
    public class PatientRequestUpdateEmailCommandValidator : AbstractValidator<PatientRequestUpdateEmailCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public PatientRequestUpdateEmailCommandValidator(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;

            RuleFor(v => v.PatientId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(GuidIsValid)
                .MustAsync(PatientIsActive).WithMessage(EmailTemplateConfiguration.GetResourceString("PatientMustBeActive", GetPatientLanguage().Result)); ;

            RuleFor(v => v.EmailAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(EmailTemplateConfiguration.GetResourceString("EmailAddressRequired", GetPatientLanguage().Result))
                .NotEmpty().WithMessage(EmailTemplateConfiguration.GetResourceString("EmailAddressRequired", GetPatientLanguage().Result))
                .EmailAddress().WithMessage(EmailTemplateConfiguration.GetResourceString("EmailAddressWrongFormat", GetPatientLanguage().Result))
                .MustAsync(EmailIsUnused).WithMessage(EmailTemplateConfiguration.GetResourceString("NewEmailInUse", GetPatientLanguage().Result));

            RuleFor(v => v).Must(v => ValidateUserAndPassword(v.PatientId, v.Password)).WithMessage(EmailTemplateConfiguration.GetResourceString("IncorrectPassword", GetPatientLanguage().Result));
        }

        private async Task<bool> EmailIsUnused(string email, CancellationToken arg2)
        {
            var patients = await _context.Patients.CountAsync(p => p.EmailAddress == email);
            var requests = await _context.UpdateEmailRequests.CountAsync(r => r.EmailAddress == email);
            return (patients + requests) == 0;
        }

        private async Task<bool> PatientIsActive(Guid patientId, CancellationToken arg2)
        {
            var p = await _context.Patients.SingleAsync(p => p.Id == patientId);
            return p.Active;
        }

        public bool GuidIsValid(Guid guidValue)
        {
            return ! guidValue.Equals(Guid.Empty);
        }

        public bool ValidateUserAndPassword(Guid? patientId, string password)
        {
            var email = _context.Patients.Find(patientId)?.EmailAddress;
            return _identityService.ValidateUserAndPassword(email, password).Result;
        }

        private async Task<string> GetPatientLanguage()
        {
            var patientId = await _identityService.GetCurrentPatientId();
            return _context.Patients.Find(patientId)?.PatientLanguage;
        }
    }
}
