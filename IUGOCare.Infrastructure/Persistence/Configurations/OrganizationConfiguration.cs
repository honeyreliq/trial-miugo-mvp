using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(p => p.Phone)
                .HasMaxLength(20)
                .IsRequired();

            builder.OwnsOne(p => p.Address, a =>
            {
                a.Property(ad => ad.AddressLines).HasMaxLength(600);
                a.Property(ad => ad.City).HasMaxLength(200);
                a.Property(ad => ad.State).HasMaxLength(200);
                a.Property(ad => ad.Country).HasMaxLength(200);
                a.Property(ad => ad.ZipCode).HasMaxLength(20);
            });

            builder.Property(p => p.CreatedBy).HasMaxLength(600);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(600);
        }
    }
}
