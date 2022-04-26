using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Common;
using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace IUGOCare.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private IDbContextTransaction _currentTransaction;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeOffset _dateTimeOffset;

        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicPatient> ClinicPatients { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public DbSet<ObservationData> ObservationsData { get; set; }
        public DbSet<Activation> Activations { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<TargetRange> TargetRanges { get; set; }
        public DbSet<UpdateEmailRequest> UpdateEmailRequests { get; set; }
        public DbSet<CareManagementProgram> CareManagementPrograms { get; set; }
        public DbSet<PatientEmergencyContact> PatientsEmergencyContact { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<PatientCareManagementProgram> PatientCareManagementPrograms { get; set; }
        public DbSet<PatientTour> PatientTours { get; set; }
        public DbSet<Provider> Providers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDateTimeOffset dateTimeOffset) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeOffset = dateTimeOffset;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTimeOffset.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTimeOffset.UtcNow;
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<Activation>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.ExpirationDate = _dateTimeOffset.UtcNow.AddDays(7);
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await base.Database.BeginTransactionAsync().ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                await RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransaction()
        {
            try
            {
                await _currentTransaction?.RollbackAsync();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            base.OnModelCreating(builder);
        }
    }
}
