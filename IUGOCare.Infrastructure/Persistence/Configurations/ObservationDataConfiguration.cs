using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class ObservationDataConfiguration : IEntityTypeConfiguration<ObservationData>
    {
        public void Configure(EntityTypeBuilder<ObservationData> builder)
        {
            builder.Property(o => o.Unit)
                .HasMaxLength(20);
        }
    }
}
