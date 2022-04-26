using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Commands.UpdatePatientPreferences
{
    public class UpdatePatientPreferencesCommandValidator : AbstractValidator<UpdatePatientPreferencesCommand>
    {

        private readonly IApplicationDbContext _context;
        public UpdatePatientPreferencesCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(c => c.PatientId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Patient Id is required.")
                .MustAsync(PatientExists).WithMessage("Patient must be registered.");
        }

        private async Task<bool> PatientExists(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Patients.AnyAsync(p => p.Id == id);
        }
    }
}
