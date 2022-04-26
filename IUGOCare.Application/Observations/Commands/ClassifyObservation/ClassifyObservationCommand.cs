using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IUGOCare.Application.Observations.Commands.ClassifyObservation
{
    public class ClassifyObservationCommand : IRequest
    {
        public Guid Id { get; set; }
        public string SourceId { get; set; }
        public string ObservationStatus { get; set; }
        public string ObservationLevel { get; set; }
    }

    public class ClassifyObservationCommandHandler : IRequestHandler<ClassifyObservationCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<ClassifyObservationCommand> _logger;

        public ClassifyObservationCommandHandler(IApplicationDbContext context, ILogger<ClassifyObservationCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(ClassifyObservationCommand request, CancellationToken cancellationToken)
        {
            Observation observation = null;

            if (Guid.TryParse(request.SourceId, out Guid sourceId))
                observation = await _context.Observations.FirstOrDefaultAsync(o => o.Id == sourceId);

            if (observation is null)
                observation = await _context.Observations.FirstOrDefaultAsync(o => o.Id == request.Id);

            if (observation is null)
                throw new NotFoundException(nameof(Observation), request.Id);

            observation.ObservationStatus = request.ObservationStatus;
            observation.ObservationLevel = request.ObservationLevel;

            _context.Observations.Update(observation);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
