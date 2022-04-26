using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class PatientEmergencyContactConfiguration : IEntityTypeConfiguration<PatientEmergencyContact>
    {
        public void Configure(EntityTypeBuilder<PatientEmergencyContact> builder)
        {
            builder.HasKey(c => c.ClinicPatientId);

            builder.HasOne(t => t.ClinicPatient)
                .WithOne(ec => ec.EmergencyContact)
                .HasForeignKey<PatientEmergencyContact>(t => t.ClinicPatientId);

            builder.Property(p => p.ContactName)
                .HasMaxLength(100);

            builder.Property(p => p.Phone)
                .HasMaxLength(20);

            builder.Property(p => p.Relationship)
                .HasMaxLength(50);

            builder.Property(p => p.CreatedBy).HasMaxLength(600);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(600);
        }
    }
}
