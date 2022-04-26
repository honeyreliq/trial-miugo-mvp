using FluentValidation;
using IUGOCare.Application.Common.Validators;

namespace IUGOCare.Application.Patients.Commands.AddOrUpdateEmergencyContact
{
    public class AddOrUpdateEmergencyContactCommandValidator : AbstractValidator<AddOrUpdateEmergencyContactCommand>
    {
        public AddOrUpdateEmergencyContactCommandValidator()
        {
            RuleFor(p => p.ClinicPatientId)
                .NotEmpty().WithMessage("The ClinicPatientId must be a valid GUID.");

            RuleFor(p => p.Phone)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .PhoneNumber().WithMessage("The format of the phone number is incorrect.")
                .When(ec => !string.IsNullOrWhiteSpace(ec.Phone), ApplyConditionTo.CurrentValidator);
        }
    }
}
