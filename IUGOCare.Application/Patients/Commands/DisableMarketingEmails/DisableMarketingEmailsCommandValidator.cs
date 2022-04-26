using FluentValidation;

namespace IUGOCare.Application.Patients.Commands.DisableMarketingEmails
{
    public class DisableMarketingEmailsCommandValidator : AbstractValidator<DisableMarketingEmailsCommand>
    {
        public DisableMarketingEmailsCommandValidator()
        {
            RuleFor(p => p.PatientId).NotEmpty();
        }
    }
}
