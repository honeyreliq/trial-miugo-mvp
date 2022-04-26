using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.PatientCareManagementPrograms.Commands.SetPatientCareManagementProgramEnrollment
{
    public class SetPatientCareManagementProgramEnrollmentCommand : IRequest
    {
        public Guid ClinicPatientId { get; set; }
        public string CareProgramShortName { get; set; }
        public bool IsEnrolled { get; set; }
    }

    public class UpdatePatientCareManagementEnrollmentCommandHandler : IRequestHandler<SetPatientCareManagementProgramEnrollmentCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePatientCareManagementEnrollmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetPatientCareManagementProgramEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var clinicPatient = await _context.ClinicPatients
                .Include(p => p.PatientCareManagementPrograms)
                .FirstOrDefaultAsync(p => p.ClinicPatientId == request.ClinicPatientId);

            if (clinicPatient is null)
                throw new NotFoundException($"Clinic Patient not found for ClinicPatientId {request.ClinicPatientId}");

            var careProgram = await _context.CareManagementPrograms
                .FirstOrDefaultAsync(c => c.ShortName.ToLower() == request.CareProgramShortName.ToLower());

            if (careProgram is null)
                throw new NotFoundException($"Care Program not found with Short Name {request.CareProgramShortName}");

            clinicPatient.SetCareManagementProgramEnrollment(careProgram.Id, request.IsEnrolled);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
