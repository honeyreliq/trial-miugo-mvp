using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;

namespace IUGOCare.Application.Patients.Commands.SendPatientOnboardingEmail
{
    public class SendPatientOnboardingEmailCommand : IRequest
    {
        public Guid PatientId { get; set; }
        public Guid ClinicPatientId { get; set; }
        public string Language { get; set; }
    }

    public class SendPatientOnboardingEmailCommandHandler : IRequestHandler<SendPatientOnboardingEmailCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ISendEmailService _emailService;
        private readonly IActivationCode _activationCodeService;

        public SendPatientOnboardingEmailCommandHandler(IApplicationDbContext context, ISendEmailService emailService
            , IActivationCode activationCodeService)
        {
            _context = context;
            _emailService = emailService;
            _activationCodeService = activationCodeService;
        }

        public async Task<Unit> Handle(SendPatientOnboardingEmailCommand request, CancellationToken cancellationToken)
        {
            var patientId = request.PatientId;
            ClinicPatient clinicPatient = null;
            Guid clinicId = Guid.Empty;

            if (request.PatientId.Equals(Guid.Empty))
            {
                clinicPatient = await _context.ClinicPatients.FindAsync(request.ClinicPatientId);
                clinicId = clinicPatient.ClinicId;
                patientId = clinicPatient.PatientId;
            } else
            {
                clinicPatient = _context.ClinicPatients.Where(cp => cp.PatientId == patientId).FirstOrDefault();
                clinicId = clinicPatient.ClinicId;
            }
            
            var patient = await _context.Patients.FindAsync(patientId);
            var clinic  = clinicId != Guid.Empty ? await _context.Clinics.FindAsync(clinicId) : null;

            if(clinic != null && clinic.EmailsEnabled)
            { 
                var activation = await _context.Activations.FindAsync(patientId);
                var activationCode = activation?.ActivationCode ?? string.Empty;
                if (activation is null)
                {
                    activationCode = _activationCodeService.GenerateNewActivationCode();
                    await _context.Activations.AddAsync(new Activation { PatientId = patientId, ActivationCode = activationCode });
                }

                await _emailService.SendPatientOnboardingEmail(patient, clinicPatient, activation);
            }

            return Unit.Value;
        }
    }
}
