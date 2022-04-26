using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Observations.Commands.ReviewObservation
{
    public class ReviewObservationCommand : IRequest
    {
        public Guid Id { get; set; }
        public string SourceId { get; set; }
        public DateTimeOffset IsReviewedDate { get; set; }
        public string ReviewedByName { get; set; }
    }

    public class ReviewObservationCommandHandler : IRequestHandler<ReviewObservationCommand>
    {
        private readonly IApplicationDbContext _context;

        public ReviewObservationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ReviewObservationCommand request, CancellationToken cancellationToken)
        {
            Observation observation = null;

            if (Guid.TryParse(request.SourceId, out Guid sourceId))
                observation = await _context.Observations.FirstOrDefaultAsync(o => o.Id == sourceId);

            if (observation is null)
                observation = await _context.Observations.FirstOrDefaultAsync(o => o.Id == request.Id);

            if (observation is null)
                throw new NotFoundException(nameof(Observation), request.Id);

            observation.IsReviewed = true;
            observation.IsReviewedDate = request.IsReviewedDate;
            observation.ReviewedByName = request.ReviewedByName;

            _context.Observations.Update(observation);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
