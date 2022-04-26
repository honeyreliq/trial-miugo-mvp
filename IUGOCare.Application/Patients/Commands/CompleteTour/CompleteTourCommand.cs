using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;

namespace IUGOCare.Application.Patients.Commands.CompleteTour
{
    public class CompleteTourCommand : IRequest
    {
        public string TourKey { get; set; }
        public CompletionReason CompletionReason { get; set; }
    }

    public class CompleteTourCommandHandler : IRequestHandler<CompleteTourCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;

        public CompleteTourCommandHandler(IIdentityService identityService, IApplicationDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        public async Task<Unit> Handle(CompleteTourCommand request, CancellationToken cancellationToken)
        {
            var currentPatientId = await _identityService.GetCurrentPatientId();

            var pt = new PatientTour
            {
                PatientId = currentPatientId,
                TourKey = request.TourKey,
                CompletionReason = request.CompletionReason,
                Completed = DateTimeOffset.UtcNow,
            };
            _context.PatientTours.Add(pt);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
