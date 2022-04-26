using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(p => p.PatientLanguage)
                .IsRequired()
                .HasMaxLength(2)
                .HasDefaultValue("EN");

            builder.HasMany(c => c.Clinics);
        }
    }
}
