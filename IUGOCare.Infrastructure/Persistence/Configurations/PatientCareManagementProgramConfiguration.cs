using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class PatientCareManagementProgramConfiguration : IEntityTypeConfiguration<PatientCareManagementProgram>
    {
        public void Configure(EntityTypeBuilder<PatientCareManagementProgram> builder)
        {
            builder.HasKey(c => new { c.ClinicPatientId, c.CareManagementProgramId } );

            builder.HasOne(c => c.BillingProvider)
                .WithMany(p => p.PatientCareManagementPrograms)
                .HasForeignKey(c => c.BillingProviderId);

            builder.HasOne(c => c.CareManagementProgram)
                .WithMany(p => p.PatientCareManagementPrograms)
                .HasForeignKey(c => c.CareManagementProgramId);

            builder.HasOne(pcmp => pcmp.ClinicPatient)
                .WithMany(cp => cp.PatientCareManagementPrograms)
                .HasForeignKey(pcmp => pcmp.ClinicPatientId);
        }
    }
}
