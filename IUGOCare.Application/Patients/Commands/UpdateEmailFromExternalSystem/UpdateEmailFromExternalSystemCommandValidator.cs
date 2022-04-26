using System;
using FluentValidation;

namespace IUGOCare.Application.Patients.Commands.UpdateEmailFromExternalSystem
{
    public class UpdateEmailFromExternalSystemCommandValidator : AbstractValidator<UpdateEmailFromExternalSystemCommand>
    {
        public UpdateEmailFromExternalSystemCommandValidator()
        {
            RuleFor(v => v.ClinicPatientId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("The ClinicPatientId must be a valid GUID.");

            RuleFor(v => v.EmailAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("The email address cannot be null.")
                .When(v => string.IsNullOrWhiteSpace(v.EmailAddress), ApplyConditionTo.CurrentValidator)
                .EmailAddress().WithMessage("The format of the email address is incorrect.")
                .When(v => !string.IsNullOrWhiteSpace(v.EmailAddress), ApplyConditionTo.CurrentValidator);
        }
    }
}
