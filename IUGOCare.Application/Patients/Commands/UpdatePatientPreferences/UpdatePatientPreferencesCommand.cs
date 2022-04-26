using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Commands.UpdatePatientPreferences
{
    public class UpdatePatientPreferencesCommand : IRequest
    {
        public Guid PatientId { get; set; }
        public string PatientLanguage { get; set; }
        public string TimeZone { get; set; }
        public string WindowsTimeZone { get; set; }
        public string PatientTheme { get; set; }
        public bool Tooltips { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
    }

    public class UpdatePatientPreferencesCommandHandler : IRequestHandler<UpdatePatientPreferencesCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePatientPreferencesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePatientPreferencesCommand command, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .Include(p => p.Clinics)
                .FirstOrDefaultAsync(p => p.Id == command.PatientId);

            patient.PrimaryClinicPatient.TimeZone = command.TimeZone ?? patient.PrimaryClinicPatient.TimeZone;
            patient.PrimaryClinicPatient.WindowsTimeZone = command.WindowsTimeZone ?? patient.PrimaryClinicPatient.WindowsTimeZone;
            patient.PatientTheme = command.PatientTheme ?? patient.PatientTheme;
            patient.Tooltips = command.Tooltips;
            patient.DateFormat = command.DateFormat ?? patient.DateFormat;
            patient.TimeFormat = command.TimeFormat ?? patient.TimeFormat;

            patient.SetLanguage(command.PatientLanguage);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
