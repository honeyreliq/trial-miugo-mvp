using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Common.Validators;
using Microsoft.EntityFrameworkCore;
using static System.String;

namespace IUGOCare.Application.Patients.Commands.RegisterPatient
{
    public class RegisterPatientCommandValidator : AbstractValidator<RegisterPatientCommand>
    {
        private readonly IApplicationDbContext _context;

        public RegisterPatientCommandValidator(IApplicationDbContext context)
        {
            _context = context;

           RuleFor(v => v.ClinicSubdomain)
                .NotNull()
                .NotEmpty().WithMessage("The Clinic Subdomain is required.");
            RuleFor(v => v.ClinicId)
                .NotNull()
                .NotEmpty().WithMessage("The Clinic Id is required.");
            RuleFor(v => v.ClinicPatientId)
                .NotNull()
                .NotEmpty().WithMessage("The Clinic Patient Id is required.")
                .MustAsync(ClinicPatientIdIsUnregistered).WithMessage("The clinic patient id is already registered.");
            RuleFor(v => v.EmailAddress)
                .EmailAddress().When(e => !IsNullOrWhiteSpace(e.EmailAddress)).WithMessage("The format of the email address is incorrect.")
                .MustAsync(EmailAddressIsUnregistered).When(e => !IsNullOrWhiteSpace(e.EmailAddress)).WithMessage("The email address is already registered.");
            RuleFor(v => v.PatientTimeZone)
                .NotNull()
                .NotEmpty()
                .WithMessage("The patient's time zone is required.");
            RuleFor(v => v.PatientWindowsTimeZone)
                .NotNull()
                .NotEmpty()
                .WithMessage("The patient's Windows time zone is required.");
            RuleFor(p => p.PhoneNumber)
                .PhoneNumber().When(p => !IsNullOrWhiteSpace(p.PhoneNumber)).WithMessage("The format of the phone number is incorrect.");
            RuleFor(p => p.BirthDate)
                .LessThanOrEqualTo(DateTime.UtcNow).When(p => p.BirthDate != null).WithMessage("Birth date cannot be in the future.");
            RuleFor(p => p.PrimaryCareProviderId)
                .MustAsync(ExistsProvider).When(p => p.PrimaryCareProviderId != null).WithMessage("The primary care provider is not registered.");
        }

        public async Task<bool> EmailAddressIsUnregistered(string emailAddress, CancellationToken cancellationToken)
        {
            return !await _context.Patients.AnyAsync(p => p.EmailAddress.Equals(emailAddress), cancellationToken);
        }

        public async Task<bool> ClinicPatientIdIsUnregistered(Guid clinicPatientId, CancellationToken cancellationToken)
        {
            return !await _context.ClinicPatients.AnyAsync(p => p.ClinicPatientId.Equals(clinicPatientId), cancellationToken);
        }

        public async Task<bool> ExistsProvider(Guid? providerId, CancellationToken cancellationToken)
        {
            return await _context.Providers.FindAsync(providerId) != null;
        }
    }
}
