using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.TargetRanges.Commands.SetTargetRanges
{
    public class SetTargetRangesCommand : IRequest
    {
        public Guid ClinicPatientId { get; set; }
        public List<TargetRange> TargetRanges { get; set; }
    }

    public class SetTargetRangesCommandHandler : IRequestHandler<SetTargetRangesCommand>
    {
        private readonly IApplicationDbContext _context;

        public SetTargetRangesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetTargetRangesCommand request, CancellationToken cancellationToken)
        {
            if (!await _context.ClinicPatients.AnyAsync(cp => cp.ClinicPatientId == request.ClinicPatientId))
            {
                throw new NotFoundException($"ClinicPatient not found for ClinicPatientId {request.ClinicPatientId}");
            }                

            request.TargetRanges.ForEach(async (t) =>
            {
                t.ClinicPatientId = request.ClinicPatientId;
                await _context.TargetRanges.AddAsync(t);
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
