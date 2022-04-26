using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Commands.AddOrUpdateEmergencyContact
{
    public class AddOrUpdateEmergencyContactCommand : IRequest
    {
        public Guid ClinicPatientId { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Relationship { get; set; }
    }

    public class AddOrUpdateEmergencyContactCommandHandler : IRequestHandler<AddOrUpdateEmergencyContactCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddOrUpdateEmergencyContactCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(AddOrUpdateEmergencyContactCommand request, CancellationToken cancellationToken)
        {
            var clinicPatient = await _context.ClinicPatients.Include(cp => cp.EmergencyContact)
                                                             .FirstOrDefaultAsync(cp => cp.ClinicPatientId == request.ClinicPatientId);

            if (clinicPatient is null)
                throw new NotFoundException(nameof(ClinicPatient), request.ClinicPatientId);

            clinicPatient.SetEmergencyContact(request.ContactName, request.Phone, request.Relationship);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
