using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Clinic> Clinics { get; set; }
        DbSet<ClinicPatient> ClinicPatients { get; set; }
        DbSet<Patient> Patients { get; set; }
        DbSet<Observation> Observations { get; set; }
        DbSet<ObservationData> ObservationsData { get; set; }
        DbSet<Activation> Activations { get; set; }
        DbSet<Translation> Translations { get; set; }
        DbSet<TargetRange> TargetRanges { get; set; }
        DbSet<UpdateEmailRequest> UpdateEmailRequests { get; set; }
        DbSet<CareManagementProgram> CareManagementPrograms { get; set; }
        DbSet<PatientEmergencyContact> PatientsEmergencyContact { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<PatientCareManagementProgram> PatientCareManagementPrograms { get; set; }
        DbSet<PatientTour> PatientTours { get; set; }
        DbSet<Provider> Providers { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
