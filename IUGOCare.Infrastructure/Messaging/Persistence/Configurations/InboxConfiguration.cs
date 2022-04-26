using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Messaging.Persistence.Configurations
{
    public class InboxConfiguration : IEntityTypeConfiguration<Inbox>
    {
        public void Configure(EntityTypeBuilder<Inbox> builder)
        {
            builder.ToTable("Inbox", "staging");
            builder.HasKey(i => i.MessageId);

            builder.Property(i => i.MessageId)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(i => i.Label)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.MessageBody)
                .IsRequired();

            builder.Property(i => i.Created)
                .HasDefaultValueSql("sysdatetimeoffset()");

            builder.Property(i => i.Status)
                .HasMaxLength(100);
        }
    }
}
