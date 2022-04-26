using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Observations.Commands.CreateObservation;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Observations.Commands.UpdateObservation
{
    public class UpdateObservationCommand : IRequest<Guid>
    {
        public Guid ObservationId { get; set; }
        public string ObservationCode { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public IList<ObservationDataItem> ObservationDataList { get; set; }

        public UpdateObservationCommand()
        {
            ObservationDataList = new List<ObservationDataItem>();
        }
    }

    public class UpdateObservationCommandHandler : IRequestHandler<UpdateObservationCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public UpdateObservationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(UpdateObservationCommand request, CancellationToken cancellationToken)
        {
            var observation = await _context.Observations.FindAsync(request.ObservationId);

            if (observation is null)
            {
                throw new NotFoundException(nameof(Observation), request.ObservationId);
            }

            observation.EffectiveDate = request.EffectiveDate.ToUniversalTime();

            await _context.SaveChangesAsync(cancellationToken);

            IQueryable<ObservationData> toDelete = _context.ObservationsData
                        .Where(o => o.ObservationId == request.ObservationId);

            _context.ObservationsData.RemoveRange(toDelete);
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var rData in request.ObservationDataList)
            {
                var newObservationData = new ObservationData()
                {
                    Id = rData.Id,
                    ObservationId = request.ObservationId,
                    ObservationCode = rData.ObservationCode,
                    Unit = rData.Unit,
                    Value = rData.Value
                };
                _context.ObservationsData.Add(newObservationData);
            }

            var previousObservation = _context.Observations.Include(o => o.ObservationsData)
               .OrderByDescending(o => o.EffectiveDate)
               .ThenByDescending(o => o.Created)
               .FirstOrDefault(o => o.ObservationCode == observation.ObservationCode && o.EffectiveDate <= observation.EffectiveDate);

            var futureObservation = _context.Observations.Include(o => o.ObservationsData)
                .OrderBy(o => o.EffectiveDate)
                .ThenBy(o => o.Created)
                .FirstOrDefault(o => o.ObservationCode == observation.ObservationCode && o.EffectiveDate > observation.EffectiveDate);

            observation.SetObservationChange(previousObservation);
            var futureDataToUpdate = futureObservation?.SetObservationChange(observation) ?? new List<ObservationData>();
            foreach (var toUpdate in futureDataToUpdate)
            {
                _context.ObservationsData.Update(toUpdate);
            }

            _context.Observations.Update(observation);
            await _context.SaveChangesAsync(cancellationToken);

            return observation.Id;
        }
    }
}
