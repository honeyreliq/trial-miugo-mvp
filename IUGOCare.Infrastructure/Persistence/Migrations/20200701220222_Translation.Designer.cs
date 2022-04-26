﻿// <auto-generated />
using System;
using IUGOCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200701220222_Translation")]
    partial class Translation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IUGOCare.Domain.Entities.Activation", b =>
                {
                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActivationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ExpirationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PatientId");

                    b.ToTable("Activations");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.Clinic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailsEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subdomain")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clinics");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.ClinicPatient", b =>
                {
                    b.Property<Guid>("ClinicPatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClinicId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClinicPatientId");

                    b.HasIndex("PatientId");

                    b.ToTable("ClinicPatients");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.Observation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClinicPatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("EffectiveDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsReviewed")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("IsReviewedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObservationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObservationLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObservationStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReviewedByName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Observations");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.ObservationData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Change")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObservationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ObservationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ObservationId");

                    b.ToTable("ObservationsData");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowMarketingEmails")
                        .HasColumnType("bit");

                    b.Property<string>("Auth0Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FamilyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GivenName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientLanguage")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(2)")
                        .HasMaxLength(2)
                        .HasDefaultValue("EN");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100)
                        .HasDefaultValue("Etc/UTC");

                    b.Property<string>("WindowsTimeZone")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100)
                        .HasDefaultValue("UTC");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.ServiceBusMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CorrelationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<DateTimeOffset>("DateStaged")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("ExpectsAcknowledgement")
                        .HasColumnType("bit");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<bool>("MessageAcknowledged")
                        .HasColumnType("bit");

                    b.Property<string>("MessageBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("MessageSent")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ServiceBusMessages");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.Translation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ElementName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("FileContent")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(2)")
                        .HasMaxLength(2);

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ElementName", "Language")
                        .IsUnique();

                    b.ToTable("Translations");

                    b.HasData(
                        new
                        {
                            Id = new Guid("2db14024-e78d-4f54-b693-107da6a876fe"),
                            Created = new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 773, DateTimeKind.Unspecified).AddTicks(322), new TimeSpan(0, -6, 0, 0, 0)),
                            ElementName = "about",
                            Language = "en"
                        },
                        new
                        {
                            Id = new Guid("37edd58c-72ae-40c2-853c-68e787b98128"),
                            Created = new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4048), new TimeSpan(0, -6, 0, 0, 0)),
                            ElementName = "about",
                            Language = "es"
                        },
                        new
                        {
                            Id = new Guid("8c8a00c9-63aa-4f06-b0f4-30d03163e5e7"),
                            Created = new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4077), new TimeSpan(0, -6, 0, 0, 0)),
                            ElementName = "privacyPolicy",
                            Language = "en"
                        },
                        new
                        {
                            Id = new Guid("a0d5dd16-5fd8-49a3-bcb0-990d6236c86b"),
                            Created = new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4080), new TimeSpan(0, -6, 0, 0, 0)),
                            ElementName = "privacyPolicy",
                            Language = "es"
                        },
                        new
                        {
                            Id = new Guid("561a8240-7acd-4759-ad10-1662ddf51f69"),
                            Created = new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4083), new TimeSpan(0, -6, 0, 0, 0)),
                            ElementName = "announcements",
                            Language = "en"
                        },
                        new
                        {
                            Id = new Guid("805a649a-3433-4a7c-9de4-1ca4a64e7915"),
                            Created = new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4085), new TimeSpan(0, -6, 0, 0, 0)),
                            ElementName = "announcements",
                            Language = "es"
                        });
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.Activation", b =>
                {
                    b.HasOne("IUGOCare.Domain.Entities.Patient", "Patient")
                        .WithOne("Activation")
                        .HasForeignKey("IUGOCare.Domain.Entities.Activation", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.ClinicPatient", b =>
                {
                    b.HasOne("IUGOCare.Domain.Entities.Patient", null)
                        .WithMany("Clinics")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.ObservationData", b =>
                {
                    b.HasOne("IUGOCare.Domain.Entities.Observation", "Observation")
                        .WithMany("ObservationsData")
                        .HasForeignKey("ObservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
