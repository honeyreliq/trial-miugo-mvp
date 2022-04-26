using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using IUGOCare.Messages.PatientToClinical.Patients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IUGOCare.Application.Patients.Commands.PatientAcceptsToS
{
    public class PatientActivated : INotification
    {
        public Guid PatientId { get; set; }
    }

    public class PatientActivatedHandler : INotificationHandler<PatientActivated>
    {
        private readonly ILogger<PatientActivated> _logger;
        private readonly IServiceBusSender _messageSender;
        private readonly IApplicationDbContext _context;

        public PatientActivatedHandler(ILogger<PatientActivated> logger, IServiceBusSender messageSender, IApplicationDbContext context)
        {
            _logger = logger;
            _messageSender = messageSender;
            _context = context;
        }

        public async Task Handle(PatientActivated notification, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .Include(p => p.Clinics)
                .SingleOrDefaultAsync(p => p.Id == notification.PatientId);

            if (patient is null)
            {
                _logger.LogError("Error in PatientActivatedHandler: Could not find patient with Id {0}", notification.PatientId);
                return;
            }
            
            foreach(ClinicPatient clinic in patient.Clinics)
            {
                var dto = new PatientActivatedDto { ClinicPatientId = clinic.ClinicPatientId };
                var clinicAssociated = await _context.Clinics.FindAsync(clinic.ClinicId);

                if (clinicAssociated is null)
                {
                    _logger.LogError("Error in PatientActivatedHandler: Could not find clinic with Id {0}", clinic.ClinicId);
                    return;
                }

                await _messageSender.SendMessageAsync(clinicAssociated.Subdomain, nameof(PatientActivated), dto);
            }
        }
    }
}
