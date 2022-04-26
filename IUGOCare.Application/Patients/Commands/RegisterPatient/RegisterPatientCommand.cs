using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using static System.String;

namespace IUGOCare.Application.Patients.Commands.RegisterPatient
{
    public class RegisterPatientCommand : IRequest
    {
        public Guid ClinicPatientId { get; set; }
        public Guid ClinicId { get; set; }

        public string ClinicSubdomain { get; set; }

        public string EmailAddress { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string FamilyName { get; set; }
        public string PatientLanguage { get; set; }
        public string PatientTimeZone { get; set; }
        public string PatientWindowsTimeZone { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public Guid? PrimaryCareProviderId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhoneNumber { get; set; }
        public string EmergencyContactRelationship { get; set; }
    }

    public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly ISendEmailService _sendEmailService;
        private readonly IActivationCode _activationCodeService;
        private readonly ILogger<RegisterPatientCommandHandler> _logger;

        public RegisterPatientCommandHandler(IApplicationDbContext context, ISendEmailService sendEmailService,
            IActivationCode activationCodeService, ILogger<RegisterPatientCommandHandler> logger)
        {
            _context = context;
            _sendEmailService = sendEmailService;
            _activationCodeService = activationCodeService;
            _logger = logger;
    }

        public async Task<Unit> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
        {
            var existingClinicPatient = await _context.ClinicPatients.FindAsync(request.ClinicPatientId);
            if (existingClinicPatient != null)
            {
                _logger.LogWarning("Patient already registered (ClinicPatientId: {0})", request.ClinicPatientId);
                return Unit.Value;
            }

            Patient patient = SetPatient(request);
            ClinicPatient clinicPatient = SetClinicPatient(patient.Id, request);

            var clinicAssociated = await _context.Clinics.FindAsync(request.ClinicId);

            if (clinicAssociated != null && clinicAssociated.EmailsEnabled && !IsNullOrWhiteSpace(request.EmailAddress))
            {
                var activationCode = _activationCodeService.GenerateNewActivationCode();
                var activation = new Activation { PatientId = patient.Id, ActivationCode = activationCode };
                await _context.Activations.AddAsync(activation);

                await _sendEmailService.SendPatientOnboardingEmail(patient, clinicPatient, activation);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private Patient SetPatient(RegisterPatientCommand request)
        {
            Patient patient = new Patient
            {
                Auth0Id = null,
                Active = false,
                AllowMarketingEmails = false,
                EmailAddress = request.EmailAddress,
            };

            patient.SetLanguage(request.PatientLanguage);
            _context.Patients.Add(patient);

            return patient;
        }

        private ClinicPatient SetClinicPatient(Guid patientId, RegisterPatientCommand request)
        {
            ClinicPatient clinicPatient = new ClinicPatient
            {
                PatientId = patientId,
                ClinicPatientId = request.ClinicPatientId,
                ClinicId = request.ClinicId,
                GivenName = request.GivenName,
                MiddleName = request.MiddleName,
                FamilyName = request.FamilyName,
                TimeZone = request.PatientTimeZone,
                WindowsTimeZone = request.PatientWindowsTimeZone,
                PrimaryCareProviderId = request.PrimaryCareProviderId,
                Phone = request.PhoneNumber,
                BirthDate = request.BirthDate,
                Address = new Address
                {
                    AddressLines = $"{request.AddressLine1} {request.AddressLine2}".Trim(),
                    City = request.City,
                    State = request.State,
                    ZipCode = request.ZipCode,
                    Country = request.Country
                },
            };

            clinicPatient.SetEmergencyContact(request.EmergencyContactName, request.EmergencyContactPhoneNumber, request.EmergencyContactRelationship);

            _context.ClinicPatients.Add(clinicPatient);

            return clinicPatient;
        }
    }
}
