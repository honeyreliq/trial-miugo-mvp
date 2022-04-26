using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IUGOCare.Domain.Common;

namespace IUGOCare.Application.Observations.Commands.CreateObservation
{
    public class CreateObservationCommand : IRequest
    {
        public Guid ObservationId { get; set; }
        public Guid ClinicPatientId { get; set; }
        public string ObservationCode { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public string Source { get; set; }
        public string ObservationStatus { get; set; }
        public string ObservationLevel { get; set; }
        public bool IsReviewed { get; set; }
        public DateTimeOffset? IsReviewedDate { get; set; }
        public string ReviewedByName { get; set; }
        public string Manufacturer { get; set; }
        public IList<ObservationDataItem> ObservationDataList { get; set; }

        public CreateObservationCommand()
        {
            ObservationDataList = new List<ObservationDataItem>();
        }
    }

    public class ObservationDataItem
    {
        public Guid Id { get; set; }
        public string ObservationCode { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
    }

    public class CreateObservationCommandHandler : IRequestHandler<CreateObservationCommand>
    {

        private readonly IApplicationDbContext _context;

        public CreateObservationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateObservationCommand request, CancellationToken cancellationToken)
        {
            var newObservation = new Observation()
            {
                Id = request.ObservationId,
                ClinicPatientId = request.ClinicPatientId,
                ObservationCode = request.ObservationCode,
                EffectiveDate = request.EffectiveDate.ToUniversalTime(),
                Source = request.Source ?? "",
                ObservationStatus = request.ObservationStatus ?? "Tbd",
                ObservationLevel = request.ObservationLevel ?? "Na",
                IsReviewed = request.IsReviewed,
                IsReviewedDate = request.IsReviewedDate,
                ReviewedByName = request.ReviewedByName,
            };

            foreach (var data in request.ObservationDataList)
            {
                var conversion = UnitConversionUtility.ConvertToMetricUnit(data.Unit, data.Value);

                var newObservationData = new ObservationData()
                {
                    Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id,
                    ObservationId = request.ObservationId,
                    ObservationCode = data.ObservationCode,
                    Unit = conversion.DestinationUnit,
                    Value = conversion.ConvertedValue
                };

                newObservation.ObservationsData.Add(newObservationData);
            }

            var previousObservationQuery = _context.Observations.Include(o => o.ObservationsData)
                .OrderByDescending(o => o.EffectiveDate)
                .ThenByDescending(o => o.Created);

            var previousObservation = previousObservationQuery.FirstOrDefault(o => o.ObservationCode == newObservation.ObservationCode && o.EffectiveDate <= newObservation.EffectiveDate);

            var futureObservationQuery = _context.Observations.Include(o => o.ObservationsData)
                .OrderBy(o => o.EffectiveDate)
                .ThenBy(o => o.Created);

            var futureObservation = futureObservationQuery.FirstOrDefault(o => o.ObservationCode == newObservation.ObservationCode && o.EffectiveDate > newObservation.EffectiveDate);

            string[] observationCodes = new string[] { "blood-pressure", "oxygen-saturation", "heart-rate" };

            if (observationCodes.Contains(newObservation.ObservationCode))
            {
                previousObservation = previousObservationQuery.FirstOrDefault(o => observationCodes.Contains(o.ObservationCode) && o.EffectiveDate <= newObservation.EffectiveDate);
                futureObservation = futureObservationQuery.FirstOrDefault(o => observationCodes.Contains(o.ObservationCode) && o.EffectiveDate > newObservation.EffectiveDate);
            }

            newObservation.SetObservationChange(previousObservation);
            var futureDataToUpdate = futureObservation?.SetObservationChange(newObservation) ?? new List<ObservationData>();
            foreach (var toUpdate in futureDataToUpdate)
            {
                _context.ObservationsData.Update(toUpdate);
            }

            _context.Observations.Add(newObservation);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        
    }
}
