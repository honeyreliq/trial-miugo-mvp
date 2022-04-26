using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Interfaces;

namespace IUGOCare.Application.Patients.Commands.UpdatePassword
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdatePasswordCommandValidator(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;

            RuleFor(v => v.PatientId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(GuidIsValid).When(p => p.PatientId is null || p.PatientId.Equals(Guid.Empty)).WithMessage(EmailTemplateConfiguration.GetResourceString("PatientIdRequired", GetPatientLanguage().Result));
            RuleFor(v => v.CurrentPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(EmailTemplateConfiguration.GetResourceString("CurrentPasswordRequired", GetPatientLanguage().Result))
                .NotEmpty().WithMessage(EmailTemplateConfiguration.GetResourceString("CurrentPasswordRequired", GetPatientLanguage().Result));
            RuleFor(v => v.NewPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(EmailTemplateConfiguration.GetResourceString("NewPasswordRequired", GetPatientLanguage().Result))
                .NotEmpty().WithMessage(EmailTemplateConfiguration.GetResourceString("NewPasswordRequired", GetPatientLanguage().Result));
            RuleFor(v => v).Must(v => ValidateUserAndPassword(v.PatientId, v.CurrentPassword)).WithMessage(EmailTemplateConfiguration.GetResourceString("CurrentPasswordWrong", GetPatientLanguage().Result));
        }

        public bool GuidIsValid(Guid? guidValue)
        {
            return guidValue is null ? false : guidValue.Equals(Guid.Empty) ? false : true;
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
