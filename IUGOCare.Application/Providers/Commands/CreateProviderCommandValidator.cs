using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using IUGOCare.Application.Common.Validators;
using System;

namespace IUGOCare.Application.Providers.Commands
{
    public class CreateProviderCommandValidator : AbstractValidator<CreateProviderCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateProviderCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(p => p.Id)
                .NotNull().WithMessage("The provider id is required.")
                .NotEmpty().WithMessage("The provider id is required.");

            RuleFor(p => p.Name)
                .NotNull().WithMessage("The provider name is required.")
                .NotEmpty().WithMessage("The provider name is required.")
                .MustAsync(NameIsUnregistered).WithMessage("The provider name is already registered.");

            RuleFor(p => p.Type)
                .NotNull().WithMessage("The provider type is required.")
                .NotEmpty().WithMessage("The provider type is required.");

            RuleFor(p => p.Phone)
                .NotNull().WithMessage("The phone number is required.")
                .NotEmpty().WithMessage("The phone number is required.")
                .PhoneNumber().WithMessage("The format of the phone number is incorrect.");

            RuleFor(p => p.OrganizationId)
                .NotNull().WithMessage("The organization id is required.")
                .NotEmpty().WithMessage("The organization id is required.")
                .MustAsync(OrganizationIsRegistered).WithMessage("The organization id is not registered.");

            RuleFor(p => p.ZipCode)
                .ZipCode().When(p => !string.IsNullOrWhiteSpace(p.ZipCode)).WithMessage("The format of the zip code is incorrect.");
        }

        public async Task<bool> NameIsUnregistered(string name, CancellationToken cancellationToken)
        {
            return !await _context.Providers.AnyAsync(p => p.Name.Equals(name), cancellationToken);
        }

        public async Task<bool> OrganizationIsRegistered(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Organizations.FindAsync(id) != null;
        }
    }
}
