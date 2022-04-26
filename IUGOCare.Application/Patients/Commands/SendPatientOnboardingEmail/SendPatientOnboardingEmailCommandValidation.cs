using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Commands.SendPatientOnboardingEmail
{
    public class SendPatientOnboardingEmailCommandValidator : AbstractValidator<SendPatientOnboardingEmailCommand>
    {
        private readonly IApplicationDbContext _context;

        public SendPatientOnboardingEmailCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            
            
            When(m => m.ClinicPatientId == null || m.ClinicPatientId == Guid.Empty, () => {
                RuleFor(m => m.PatientId).NotEmpty().WithMessage("Either Patient ID or Clinic Patient ID is required");
            });
            
            RuleFor(m => m.PatientId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .MustAsync(PatientExistsWithPatientId)
                    .When(m => m.PatientId != Guid.Empty)
                        .WithMessage("Patient not found with the provided Patient ID")
                .MustAsync(PatientIsInactiveWithPatientId)
                    .When(m => m.PatientId != Guid.Empty)
                        .WithMessage("Patient must be inactive");

            RuleFor(m => m.ClinicPatientId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .MustAsync(PatientExistsWithClinicPatientId)
                    .When(m => m.ClinicPatientId != Guid.Empty)
                        .WithMessage("Patient not found with the provided Clinic Patient ID")
                .MustAsync(PatientIsInactiveWithClinicPatientId)
                    .When(m => m.ClinicPatientId != Guid.Empty)
                        .WithMessage("Patient must be inactive");
        }

        public async Task<bool> PatientExistsWithPatientId(Guid patientId, CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AnyAsync(p => p.Id == patientId);
        }

        public async Task<bool> PatientIsInactiveWithPatientId(Guid patientId, CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AnyAsync(p => p.Id == patientId && p.Active == false);
        }


        public async Task<bool> PatientExistsWithClinicPatientId(Guid clinicPatientId, CancellationToken cancellationToken)
        {
            var clinicPatient = await _context.ClinicPatients
                .FindAsync(clinicPatientId);

            if (clinicPatient != null) 
            {
                return await _context.Patients
                    .AnyAsync(p => p.Id == clinicPatient.PatientId);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> PatientIsInactiveWithClinicPatientId(Guid clinicPatientId, CancellationToken cancellationToken)
        {
            var clinicPatient = await _context.ClinicPatients
                .FindAsync(clinicPatientId);

            return await _context.Patients
                .AnyAsync(p => p.Id == clinicPatient.PatientId && p.Active == false);
        }

    }
}
