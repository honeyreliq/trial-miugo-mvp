using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.String;


namespace IUGOCare.Application.Patients.Commands.UpdateEmailAddress
{
    public class UpdateEmailAddressCommandValidator : AbstractValidator<UpdateEmailAddressCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateEmailAddressCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.PatientId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("The PatientId must be a valid GUID.");
            RuleFor(v => v.EmailAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("The email address is required.")
                .NotEmpty().WithMessage("The email address is required.")
                .EmailAddress().WithMessage("The format of the email address is incorrect.");
            RuleFor(v => v.EmailAddress)
                .MustAsync(EmailAddressIsUnregistered).When(e => !IsNullOrWhiteSpace(e.EmailAddress)).WithMessage("The email address is already registered.");
        }

        public async Task<bool> EmailAddressIsUnregistered(string emailAddress, CancellationToken cancellationToken)
        {
            return !await _context.Patients.AnyAsync(p => p.EmailAddress.Equals(emailAddress), cancellationToken);
        }
    }
}
