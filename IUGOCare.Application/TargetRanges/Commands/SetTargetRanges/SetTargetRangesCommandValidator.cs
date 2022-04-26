using FluentValidation;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.TargetRanges.Commands.SetTargetRanges
{
    public class SetTargetRangesCommandValidator : AbstractValidator<SetTargetRangesCommand>
    {
        public SetTargetRangesCommandValidator()
        {
            RuleFor(p => p.ClinicPatientId)
                .NotNull().WithMessage("ClinicPatientId is required.")
                .NotEmpty().WithMessage("ClinicPatientId is required.");

            RuleFor(p => p.TargetRanges)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("TargetRanges are required.")
                .Must(t => t.Count > 0).WithMessage("TargetRanges are required.");

            RuleForEach(p => p.TargetRanges).SetValidator(new NewTargetRangeValidator());
        }
    }

    public class NewTargetRangeValidator : AbstractValidator<TargetRange>
    {
        public NewTargetRangeValidator()
        {
            RuleFor(t => t.ObservationCode)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Observation Code is required.")
                .NotNull().WithMessage("Observation Code is required.");

            RuleFor(t => t.Unit)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Unit is required.")
                .NotNull().WithMessage("Unit is required.");

            RuleFor(t => t.CriticalHigh)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("CriticalHigh cannot be null.")
                    .GreaterThanOrEqualTo(0).WithMessage("CriticalHigh must be greater than or equal to 0.");

            RuleFor(t => t.AtRiskHigh)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("AtRiskHigh cannot be null.")
                    .GreaterThanOrEqualTo(0).WithMessage("AtRiskHigh must be greater than or equal to 0.");

            RuleFor(t => t.AtRiskLow)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("AtRiskLow cannot be null.")
                    .GreaterThanOrEqualTo(0).WithMessage("AtRiskLow must be greater than or equal to 0.");

            RuleFor(t => t.CriticalLow)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("CriticalLow cannot be null.")
                    .GreaterThanOrEqualTo(0).WithMessage("CriticalLow must be greater than or equal to 0.");
        }
    }
}
