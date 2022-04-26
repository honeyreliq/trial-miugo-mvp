using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class UpdateEmailRequestConfiguration : IEntityTypeConfiguration<UpdateEmailRequest>
    {
        public void Configure(EntityTypeBuilder<UpdateEmailRequest> builder)
        {
            builder.Property(t => t.PatientId)
                .IsRequired();

            builder.HasKey(t => t.PatientId);

            builder.HasOne(t => t.Patient).WithOne(p => p.UpdateEmailRequest).HasForeignKey<UpdateEmailRequest>(t => t.PatientId);

            builder.Property(t => t.Token)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.EmailAddress)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
