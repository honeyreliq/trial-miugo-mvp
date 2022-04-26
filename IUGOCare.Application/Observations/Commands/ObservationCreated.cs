using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Messages.PatientToClinical.Observations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IUGOCare.Application.Observations.Commands.CreateObservation
{
    public class ObservationCreated : INotification
    {
        public Guid ObservationId { get; set; }
    }

    public class ObservationCreatedHandler : INotificationHandler<ObservationCreated>
    {
        private readonly ILogger<ObservationCreated> _logger;
        private readonly IServiceBusSender _messageSender;
        private readonly IApplicationDbContext _context;

        public ObservationCreatedHandler(ILogger<ObservationCreated> logger, IServiceBusSender messageSender, IApplicationDbContext context)
        {
            _logger = logger;
            _messageSender = messageSender;
            _context = context;
        }

        public async Task Handle(ObservationCreated notification, CancellationToken cancellationToken)
        {
            var observation = await _context.Observations
               .Include(p => p.ObservationsData)
               .SingleOrDefaultAsync(p => p.Id == notification.ObservationId);

            if (observation is null)
            {
                _logger.LogError("Error in ObservationCreatedHandler: Could not find observation with Id {0}", notification.ObservationId);
                return;
            }

            var clinicPatient = await _context.ClinicPatients.FindAsync(observation.ClinicPatientId);
            if (clinicPatient is null)
            {
                _logger.LogError("Error in ObservationCreatedHandler: Could not find clinicPatient with Id {0}", observation.ClinicPatientId);
                return;
            }

            var clinicAssociated = await _context.Clinics.FindAsync(clinicPatient.ClinicId);
            if (clinicAssociated is null)
            {
                _logger.LogError("Error in ObservationCreatedHandler: Could not find clinic with Id {0}", clinicPatient.ClinicId);
                return;
            }

            var observationDto = new ObservationCreatedDto
            {
                Id = observation.Id,
                ClinicPatientId = observation.ClinicPatientId,
                ObservationCode = observation.ObservationCode,
                EffectiveDate = observation.EffectiveDate
            };

            foreach (var datum in observation.ObservationsData)
            {
                observationDto.ObservationData.Add(new ObservationCreatedDto.ObservationDataDto
                {
                    ObservationCode = datum.ObservationCode,
                    Value = datum.Value,
                    Unit = datum.Unit
                });
            }

            await _messageSender.SendMessageAsync(clinicAssociated.Subdomain, nameof(ObservationCreated), observationDto);
        }
    }
}
