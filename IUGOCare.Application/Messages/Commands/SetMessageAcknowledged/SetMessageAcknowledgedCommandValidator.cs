using FluentValidation;

namespace IUGOCare.Application.Messages.Commands.SetMessageAcknowledged
{
    public class SetMessageAcknowledgedCommandValidator : AbstractValidator<SetMessageAcknowledgedCommand>
    {
        public SetMessageAcknowledgedCommandValidator()
        {
            RuleFor(c => c.MessageId)
                .NotNull()
                .NotEmpty().WithMessage("Message Id must not be null or empty");
        }
    }
}
