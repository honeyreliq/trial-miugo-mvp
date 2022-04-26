using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IUGOCare.Application.Patients.Commands.UpdateEmailFromExternalSystem
{
    public class UpdateEmailFromExternalSystemCommand : IRequest
    {
        public Guid ClinicPatientId { get; }
        public string EmailAddress { get; }

        public UpdateEmailFromExternalSystemCommand(Guid clinicPatientId, string emailAddress)
        {
            ClinicPatientId = clinicPatientId;
            EmailAddress = emailAddress;
        }
    }

    public class UpdateEmailFromExternalSystemCommandHandler : IRequestHandler<UpdateEmailFromExternalSystemCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ISendEmailService _sendEmailService;
        private readonly IActivationCode _activationCodeService;
        private readonly ILogger<UpdateEmailFromExternalSystemCommand> _logger;

        public UpdateEmailFromExternalSystemCommandHandler(IApplicationDbContext context,
            ISendEmailService sendEmailService,
            IActivationCode activationCodeService,
            ILogger<UpdateEmailFromExternalSystemCommand> logger)
        {
            _context = context;
            _sendEmailService = sendEmailService;
            _activationCodeService = activationCodeService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateEmailFromExternalSystemCommand request, CancellationToken cancellationToken)
        {
            // Validate
            var clinicPatient = await _context.ClinicPatients.FindAsync(request.ClinicPatientId);

            if (clinicPatient is null)
                throw new NotFoundException(nameof(ClinicPatient), request.ClinicPatientId);

            var patientId = clinicPatient.PatientId;

            var patient = await _context.Patients.FindAsync(patientId);

            if (patient.Active)
            {
                _logger.LogWarning("Warning in {0}: Active patients cannot update their email address " +
                    "from an external system. ClinicPatientId: {1}, PatientId: {2}",
                    nameof(UpdateEmailFromExternalSystemCommand),
                    request.ClinicPatientId,
                    patientId);
                return Unit.Value;
            }

            var patientWithEmailAddress = await _context.Patients.Where(p => p.EmailAddress == request.EmailAddress && p.Id != patientId)
                                                                 .ToListAsync();

            if (patientWithEmailAddress.Count > 0)
            {
                _logger.LogError("Error in {0}: Email address is already registered. " +
                    "ClinicPatientId: {1}, EmailAddress: {2}",
                    nameof(UpdateEmailFromExternalSystemCommand),
                    request.ClinicPatientId,
                    request.EmailAddress);
                return Unit.Value;
            }

            // Update email
            patient.EmailAddress = request.EmailAddress;

            // Invalidate existing activation code
            var activation = await _context.Activations.FindAsync(patient.Id);

            if (activation != null)
                _context.Activations.Remove(activation);

            // Generate new activation code
            var activationCode = _activationCodeService.GenerateNewActivationCode();
            activation = new Activation { PatientId = patient.Id, ActivationCode = activationCode };
            await _context.Activations.AddAsync(activation);

            // Send new onboarding email
            await _sendEmailService.SendPatientOnboardingEmail(patient, clinicPatient, activation);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
