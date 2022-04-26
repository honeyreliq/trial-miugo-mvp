using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class TargetRangeConfiguration : IEntityTypeConfiguration<TargetRange>
    {
        public void Configure(EntityTypeBuilder<TargetRange> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ClinicPatientId)
                .IsRequired();

            builder.Property(p => p.ObservationCode)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Unit)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.CriticalHigh)
                .HasColumnType("decimal(7,1)")
                .IsRequired();

            builder.Property(p => p.AtRiskHigh)
                .HasColumnType("decimal(7,1)")
                .IsRequired();

            builder.Property(p => p.AtRiskLow)
                .HasColumnType("decimal(7,1)")
                .IsRequired();

            builder.Property(p => p.CriticalLow)
                .HasColumnType("decimal(7,1)")
                .IsRequired();

            builder.HasOne(p => p.ClinicPatient)
                .WithMany(p => p.TargetRanges)
                .HasForeignKey(tr => tr.ClinicPatientId);
        }
    }
}
