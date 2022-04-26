using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.PatientCareManagementPrograms.Commands.EnrollNewPatientFromExternalSystem
{
    public class EnrollNewPatientFromExternalSystemCommand : IRequest
    {
        public Guid ClinicPatientId { get; }
        public IEnumerable<string> CarePrograms { get; }

        public EnrollNewPatientFromExternalSystemCommand(Guid clinicPatientId, IEnumerable<string> carePrograms)
        {
            ClinicPatientId = clinicPatientId;
            CarePrograms = carePrograms;
        }
    }

    public class EnrollNewPatientFromExternalSystemCommandHandler : IRequestHandler<EnrollNewPatientFromExternalSystemCommand>
    {
        private readonly IApplicationDbContext _context;

        public EnrollNewPatientFromExternalSystemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EnrollNewPatientFromExternalSystemCommand request, CancellationToken cancellationToken)
        {
            var clinicPatient = await _context.ClinicPatients.SingleOrDefaultAsync(p => p.ClinicPatientId == request.ClinicPatientId);
            if (clinicPatient is null)
                throw new NotFoundException(nameof(ClinicPatient), request.ClinicPatientId);

            if (request.CarePrograms.Any())
            {
                clinicPatient.PatientCareManagementPrograms.Clear();
                foreach (string programName in request.CarePrograms)
                {
                    CareManagementProgram careManagementProgram = await _context.CareManagementPrograms
                            .FirstOrDefaultAsync(p => p.ShortName == programName);

                    if (careManagementProgram is null)
                        throw new NotFoundException(nameof(CareManagementProgram), programName);

                    var patientCareProgram = new PatientCareManagementProgram
                    {
                        ClinicPatientId = request.ClinicPatientId,
                        CareManagementProgramId = careManagementProgram.Id
                    };

                    clinicPatient.PatientCareManagementPrograms.Add(patientCareProgram);
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
