using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class ActivationConfiguration : IEntityTypeConfiguration<Activation>
    {
        public void Configure(EntityTypeBuilder<Activation> builder)
        {
            builder.Property(t => t.PatientId)
                .IsRequired();

            builder.HasKey(t => t.PatientId);

            builder.HasOne(t => t.Patient).WithOne(p => p.Activation).HasForeignKey<Activation>(t => t.PatientId);

            builder.Property(t => t.ActivationCode)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
