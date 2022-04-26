using System;
using FluentValidation;

namespace IUGOCare.Application.Observations.Commands.CreateManualObservation
{
    public class CreateManualObservationValildator : AbstractValidator<CreateManualObservationCommand>
    {
        public CreateManualObservationValildator()
        {
            RuleFor(o => o.EffectiveDate).LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("Date cannot be in the future.");
        }
    }
}
