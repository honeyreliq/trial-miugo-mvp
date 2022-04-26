using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Application.Observations.Commands.CreateObservation;
using IUGOCare.Domain.Common;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Observations.Commands.CreateManualObservation
{
    public class CreateManualObservationCommand : IRequest
    {
        public string ObservationCode { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public IList<ManualObservationDataItem> ObservationDataList { get; set; }

        public CreateManualObservationCommand()
        {
            ObservationDataList = new List<ManualObservationDataItem>();
        }
    }

    public class ManualObservationDataItem
    {
        public string ObservationCode { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
    }

    public class CreateObservationManuallyHandler : IRequestHandler<CreateManualObservationCommand>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public CreateObservationManuallyHandler(IApplicationDbContext context,
                                                IMediator mediator,
                                                ICurrentUserService currentUserService)
        {
            _context = context;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(CreateManualObservationCommand request, CancellationToken cancellationToken)
        {
            var observationId = Guid.NewGuid();

            var newObservation = new Observation()
            {
                Id = observationId,
                ClinicPatientId = GetClinicPatientIds().FirstOrDefault(),
                ObservationCode = request.ObservationCode,
                EffectiveDate = request.EffectiveDate.ToUniversalTime(),
                Source = "patient-entered",
                ObservationStatus = "Tbd",
                ObservationLevel = "Na",
                IsReviewed = false,
                IsReviewedDate = null,
                ReviewedByName = null
            };

            foreach (var data in request.ObservationDataList)
            {
                var conversion = UnitConversionUtility.ConvertToMetricUnit(data.Unit, data.Value);

                var newObservationData = new ObservationData()
                {
                    Id = Guid.NewGuid(),
                    ObservationId = observationId,
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

            var observationCreated = new ObservationCreated() { ObservationId = newObservation.Id };
            await _mediator.Publish(observationCreated, cancellationToken);

            return Unit.Value;
        }
        private List<Guid> GetClinicPatientIds()
        {
            var patient = _context.Patients
                    .Include(p => p.Clinics)
                    .FirstOrDefault(p => p.Auth0Id == _currentUserService.UserId);
            return patient?.Clinics?.Select(cp => cp.ClinicPatientId).ToList() ?? new List<Guid>();
        }
    }
}
