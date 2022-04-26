using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using IUGOCare.Application.Common.Validators;

namespace IUGOCare.Application.Organizations.Commands
{
    public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrganizationCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(p => p.Id)
                .NotNull().WithMessage("The organization id is required.")
                .NotEmpty().WithMessage("The organization id is required.");

            RuleFor(p => p.Name)
                .NotNull().WithMessage("The organization name is required.")
                .NotEmpty().WithMessage("The organization name is required.")
                .MustAsync(NameIsUnregistered).WithMessage("The organization name is already registered.");

            RuleFor(p => p.Phone)
                .NotNull().WithMessage("The phone number is required.")
                .NotEmpty().WithMessage("The phone number is required.")
                .PhoneNumber().WithMessage("The format of the phone number is incorrect.");

            RuleFor(p => p.ZipCode)
                .ZipCode().When(p => !string.IsNullOrWhiteSpace(p.ZipCode)).WithMessage("The format of the zip code is incorrect.");
        }

        public async Task<bool> NameIsUnregistered(string name, CancellationToken cancellationToken)
        {
            return !await _context.Organizations.AnyAsync(p => p.Name.Equals(name), cancellationToken);
        }
    }
}
