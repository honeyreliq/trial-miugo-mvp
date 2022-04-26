using FluentValidation;

namespace IUGOCare.Application.Patients.Commands.EnableMarketingEmails
{
    public class EnableMarketingEmailsCommandValidator : AbstractValidator<EnableMarketingEmailsCommand>
    {
        public EnableMarketingEmailsCommandValidator()
        {
            RuleFor(p => p.PatientId).NotEmpty();
        }
    }
}
