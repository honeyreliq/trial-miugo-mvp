using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class ClinicPatientConfiguration : IEntityTypeConfiguration<ClinicPatient>
    {
        public void Configure(EntityTypeBuilder<ClinicPatient> builder)
        {
            builder
                .HasOne(cp => cp.Patient)
                .WithMany(p => p.Clinics)
                .HasForeignKey(cp => cp.PatientId);  

            builder
                .HasOne(cp => cp.Clinic)
                .WithMany(c => c.ClinicPatients)
                .HasForeignKey(cp => cp.ClinicId);

            builder.Property(p => p.TimeZone)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("Etc/UTC");

            builder.Property(p => p.WindowsTimeZone)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("UTC");

            builder.Property(p => p.BirthDate)
                .HasColumnType("DATE");

            builder.Property(p => p.Phone)
                .HasMaxLength(20);

            builder.HasOne(c => c.PrimaryCareProvider)
                .WithMany(p => p.ClinicPatients)
                .HasForeignKey(c => c.PrimaryCareProviderId);

            builder.Property(cp => cp.GivenName).HasMaxLength(200);
            builder.Property(cp => cp.MiddleName).HasMaxLength(200);
            builder.Property(cp => cp.FamilyName).HasMaxLength(200);
            builder.Property(cp => cp.InsuranceNumber).HasMaxLength(100);
            builder.Property(cp => cp.MedicaidNumber).HasMaxLength(100);
            builder.Property(cp => cp.MedicareNumber).HasMaxLength(100);
            builder.Property(cp => cp.MedicalRecordNumber).HasMaxLength(100);
            builder.Property(cp => cp.CreatedBy).HasMaxLength(600);
            builder.Property(cp => cp.LastModifiedBy).HasMaxLength(600);

            builder.OwnsOne(p => p.Address, a =>
            {
                a.Property(ad => ad.AddressLines).HasMaxLength(600);
                a.Property(ad => ad.City).HasMaxLength(200);
                a.Property(ad => ad.State).HasMaxLength(200);
                a.Property(ad => ad.Country).HasMaxLength(200);
                a.Property(ad => ad.ZipCode).HasMaxLength(20);
            });
        }
    }
}
