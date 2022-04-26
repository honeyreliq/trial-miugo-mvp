using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Messaging.Persistence.Configurations
{
    public class OutboxConfiguration : IEntityTypeConfiguration<Outbox>
    {
        public void Configure(EntityTypeBuilder<Outbox> builder)
        {
            builder.ToTable("Outbox", "staging");

            builder.HasKey(p => p.MessageId);
            builder.Property(p => p.MessageId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.CorrelationId)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Label)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.MessageBody)
                .IsRequired();
        }
    }
}
