using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Commands.UpdatePatientInformation
{
    public class UpdatePatientInformationCommandValidator : AbstractValidator<UpdatePatientInformationCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdatePatientInformationCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(c => c.ClinicPatientId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("ClinicPatientId is required.") 
                .MustAsync(PatientExists).WithMessage("Patient must be registered.");
        }

        private async Task<bool> PatientExists(Guid clinicPatientId, CancellationToken cancellationToken)
        {
            return await _context.Patients.AnyAsync(p =>
                p.Clinics.FirstOrDefault(c => c.ClinicPatientId == clinicPatientId).PatientId == p.Id);
        }
    }
}
