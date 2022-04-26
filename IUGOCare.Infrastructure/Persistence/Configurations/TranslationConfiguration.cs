using System;
using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
    {
        public void Configure(EntityTypeBuilder<Translation> builder)
        {
            builder.Property(t => t.ElementName)
                .IsRequired();

            builder.Property(t => t.Language)
                .IsRequired()
                .HasMaxLength(2);

            builder.HasIndex(t => new { t.ElementName, t.Language })
                .IsUnique();

            builder.HasData(
                new Translation { Id = Guid.Parse("48ee8729-8c43-4ca2-95c5-5f5f2c0cfc3b"), ElementName = "about", Language = "en" },
                new Translation { Id = Guid.Parse("7bc5937a-3e33-4abe-8202-86bcee0d555b"), ElementName = "about", Language = "es" },
                new Translation { Id = Guid.Parse("4de11867-663e-4e13-a8cd-1174e7d0e7f1"), ElementName = "privacyPolicy", Language = "en" },
                new Translation { Id = Guid.Parse("ed4e76f8-425b-444e-bc55-2e48b2ec4b05"), ElementName = "privacyPolicy", Language = "es" },
                new Translation { Id = Guid.Parse("f8a902c9-f7fe-4203-a97d-b9fc7f3135ad"), ElementName = "announcements", Language = "en" },
                new Translation { Id = Guid.Parse("7a9c9d5d-d5ef-4380-9f16-5b1b7faf092d"), ElementName = "announcements", Language = "es" });
        }
    }
}
