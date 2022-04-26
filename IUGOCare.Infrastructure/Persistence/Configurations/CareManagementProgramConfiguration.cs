using System;
using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class CareManagementProgramConfiguration : IEntityTypeConfiguration<CareManagementProgram>
    {
        public void Configure(EntityTypeBuilder<CareManagementProgram> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(p => p.ShortName)
                .HasMaxLength(4)
                .IsRequired();

            builder.HasData(
                new CareManagementProgram { Id = Guid.Parse("0ac9cd3c-e8fa-49bb-9b12-011b2ac87f4c"), ShortName = "CCM", Name = "Chronic Care Management" },
                new CareManagementProgram { Id = Guid.Parse("4758ee46-f738-4c9e-83af-6598f451128a"), ShortName = "RPM", Name = "Remote Patient Monitoring" },
                new CareManagementProgram { Id = Guid.Parse("bb10362b-a9d9-4249-ae1c-f9a3b0bee797"), ShortName = "BHI", Name = "Behavioral Health Integration" },
                new CareManagementProgram { Id = Guid.Parse("fc56c18a-038f-43d9-a83f-d4cf7bc9cb67"), ShortName = "PCM", Name = "Principal Care Management" },
                new CareManagementProgram { Id = Guid.Parse("0dabb363-e803-4e99-8f22-52f61a6960a6"), ShortName = "CoCM", Name = "Psychiatric Collaborative Care Model" });
        }
    }
}
