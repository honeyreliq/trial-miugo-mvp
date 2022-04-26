using System;
using System.Collections.Generic;
using System.Text;
using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IUGOCare.Infrastructure.Persistence.Configurations
{
    class PatientTourConfiguration : IEntityTypeConfiguration<PatientTour>
    {
        public void Configure(EntityTypeBuilder<PatientTour> builder)
        {
            builder.Property(pt => pt.PatientId).IsRequired();
            builder.Property(pt => pt.TourKey)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(pt => pt.Completed).IsRequired();
            builder.Property(pt => pt.CompletionReason).IsRequired();

            builder.HasOne(pt => pt.Patient);

            builder.HasIndex(pt => new { pt.PatientId, pt.TourKey })
                .IsUnique();

            builder.HasKey(pt => new { pt.PatientId, pt.TourKey });
        }
    }
}
