using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class ObservationConfiguration : IEntityTypeConfiguration<Observation>
    {
        public void Configure(EntityTypeBuilder<Observation> builder)
        {
            builder.Property(o => o.ObservationCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(o => o.Source)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(o => o.ObservationStatus)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(o => o.ObservationLevel)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(o => o.ReviewedByName)
                .HasMaxLength(100);

            builder.Property(o => o.Manufacturer)
                .HasMaxLength(100);
        }
    }
}
