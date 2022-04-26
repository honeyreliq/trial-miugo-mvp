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

namespace IUGOCare.Application.TargetRanges.Commands.UpdateTargetRanges
{
    public class UpdateTargetRangesCommand : IRequest
    {
        public Guid ClinicPatientId { get; set; }
        public List<TargetRange> TargetRanges { get; set; }
    }

    public class UpdateTargetRangesCommandHandler : IRequestHandler<UpdateTargetRangesCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTargetRangesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTargetRangesCommand request, CancellationToken cancellationToken)
        {
            if (await _context.ClinicPatients.AnyAsync(cp => cp.ClinicPatientId == request.ClinicPatientId) is false)
                throw new NotFoundException($"ClinicPatientId not found for ClinicPatientId {request.ClinicPatientId}");

            List<TargetRange> savedRanges = await _context.TargetRanges
                .Where(t => t.ClinicPatientId == request.ClinicPatientId)
                .ToListAsync();

            // Remove existing target ranges not in request
            foreach (TargetRange targetRange in savedRanges)
            {
                if (!request.TargetRanges.Any(tr => tr.ObservationCode.Equals(targetRange.ObservationCode)))
                    _context.TargetRanges.Remove(targetRange);
            }

            // Add new or update existing target ranges from request
            foreach (TargetRange targetRange in request.TargetRanges)
            {
                TargetRange matchedRange = savedRanges.FirstOrDefault(s => s.ObservationCode.Equals(targetRange.ObservationCode));
                if (matchedRange is null)
                {
                    targetRange.ClinicPatientId = request.ClinicPatientId;
                    await _context.TargetRanges.AddAsync(targetRange);
                }
                else
                {
                    matchedRange.CriticalHigh = targetRange.CriticalHigh;
                    matchedRange.AtRiskHigh = targetRange.AtRiskHigh;
                    matchedRange.AtRiskLow = targetRange.AtRiskLow;
                    matchedRange.CriticalLow = targetRange.CriticalLow;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
