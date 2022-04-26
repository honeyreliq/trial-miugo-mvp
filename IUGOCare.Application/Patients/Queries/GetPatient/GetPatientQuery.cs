using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Constants;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Common.Constants;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Queries.GetPatient
{
    public class GetPatientQuery : IRequest<PatientVm>
    {
    }

    public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, PatientVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public GetPatientQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<PatientVm> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Patient> patients = _context.Patients;
            var auth0Id = _currentUserService.UserId;
            var patient = patients
                .Include(p => p.Clinics)
                .Where(p => p.Auth0Id == auth0Id).FirstOrDefault();
            if (patient is null)
            {
                // This should only happen in dev
                var auth0User = await _identityService.GetUserAsync(auth0Id);
                return new PatientVm
                {
                    Id = new Guid(),
                    GivenName = null,
                    MiddleName = null,
                    FamilyName = null,
                    EmailAddress = auth0User.Email,
                    PatientLanguage = Languages.EnglishLanguage,
                    PatientTheme = DefaultPreferences.DefaultTheme,
                    Tooltips = DefaultPreferences.DefaultTooltips,
                    TimeZone = DefaultPreferences.DefaultTimeZone,
                    WindowsTimeZone = DefaultPreferences.DefaultWindowsTimeZone,
                    DateFormat = DefaultPreferences.DefaultTimeFormat,
                    TimeFormat = DefaultPreferences.DefaultTimeFormat,
                    MedicaidNumber = null,
                    MedicareNumber = null,
                    MedicalRecordNumber = null,
                    InsuranceNumber = null
                };
            }

            var patientVm = new PatientVm
            {
                Id = patient.Id,
                GivenName = patient.PrimaryClinicPatient.GivenName,
                MiddleName = patient.PrimaryClinicPatient.MiddleName,
                FamilyName = patient.PrimaryClinicPatient.FamilyName,
                EmailAddress = patient.EmailAddress,
                PatientLanguage = patient.PatientLanguage,
                PatientTheme = patient.PatientTheme,
                Tooltips = patient.Tooltips,
                TimeZone = patient.PrimaryClinicPatient.TimeZone,
                WindowsTimeZone = patient.PrimaryClinicPatient.WindowsTimeZone,
                DateFormat = patient.DateFormat,
                TimeFormat = patient.TimeFormat,
                Phone = patient.PrimaryClinicPatient.Phone,
                BirthDate = patient.PrimaryClinicPatient.BirthDate,
                MedicaidNumber = patient.PrimaryClinicPatient.MedicaidNumber,
                MedicareNumber = patient.PrimaryClinicPatient.MedicareNumber,
                MedicalRecordNumber = patient.PrimaryClinicPatient.MedicalRecordNumber,
                InsuranceNumber = patient.PrimaryClinicPatient.InsuranceNumber,
                ClinicPatientIds = patient.Clinics.Select(p => p.ClinicPatientId).ToList()
            };

            return patientVm;
        }
    }
}
