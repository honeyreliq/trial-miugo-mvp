using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Interfaces;

namespace IUGOCare.Application.Patients.Commands.PatientAcceptsToS
{
    public class PatientAcceptsToSCommandValidator : AbstractValidator<PatientAcceptsToSCommand>
    {
        private readonly IApplicationDbContext _context;

        public PatientAcceptsToSCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.PatientId)
                .MustAsync(PatientIsNotActive).WithMessage("Patient is already activated.");
        }

        public async Task<bool> PatientIsNotActive(Guid patientId, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            return patient is null ? true : !patient.Active;
        }
    }
}
