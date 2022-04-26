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
    [Migration("20200714132848_AddressOwnedEntity")]
    partial class AddressOwnedEntity
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

            modelBuilder.Entity("IUGOCare.Domain.Entities.CareManagementProgram", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)")
                        .HasMaxLength(4);

                    b.HasKey("Id");

                    b.ToTable("CareManagementPrograms");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0ac9cd3c-e8fa-49bb-9b12-011b2ac87f4c"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Name = "Chronic Care Management",
                            ShortName = "CCM"
                        },
                        new
                        {
                            Id = new Guid("4758ee46-f738-4c9e-83af-6598f451128a"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Name = "Remote Patient Monitoring",
                            ShortName = "RPM"
                        },
                        new
                        {
                            Id = new Guid("bb10362b-a9d9-4249-ae1c-f9a3b0bee797"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Name = "Behavioral Health Integration",
                            ShortName = "BHI"
                        },
                        new
                        {
                            Id = new Guid("fc56c18a-038f-43d9-a83f-d4cf7bc9cb67"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Name = "Principal Care Management",
                            ShortName = "PCM"
                        },
                        new
                        {
                            Id = new Guid("0dabb363-e803-4e99-8f22-52f61a6960a6"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Name = "Psychiatric Collaborative Care Model",
                            ShortName = "CoCM"
                        });
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
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

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

                    b.HasIndex("ClinicId");

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
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ObservationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("ObservationLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ObservationStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ReviewedByName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

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
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ObservationId");

                    b.ToTable("ObservationsData");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Organizations");
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

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("DATE");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateFormat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FamilyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GivenName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InsuranceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedicaidNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedicalRecordNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedicareNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientLanguage")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(2)")
                        .HasMaxLength(2)
                        .HasDefaultValue("EN");

                    b.Property<string>("PatientTheme")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<Guid?>("PrimaryCareProviderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TimeFormat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100)
                        .HasDefaultValue("Etc/UTC");

                    b.Property<bool>("Tooltips")
                        .HasColumnType("bit");

                    b.Property<string>("WindowsTimeZone")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100)
                        .HasDefaultValue("UTC");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryCareProviderId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.PatientCareManagementProgram", b =>
                {
                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CareManagementProgramId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BillingProviderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PatientId", "CareManagementProgramId");

                    b.HasIndex("BillingProviderId");

                    b.HasIndex("CareManagementProgramId");

                    b.ToTable("PatientCareManagementPrograms");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.PatientEmergencyContact", b =>
                {
                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Relationship")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("PatientId");

                    b.ToTable("PatientsEmergencyContact");
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.Provider", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Providers");
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

            modelBuilder.Entity("IUGOCare.Domain.Entities.TargetRange", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("AtRiskHigh")
                        .HasColumnType("decimal(7,1)");

                    b.Property<decimal>("AtRiskLow")
                        .HasColumnType("decimal(7,1)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("CriticalHigh")
                        .HasColumnType("decimal(7,1)");

                    b.Property<decimal>("CriticalLow")
                        .HasColumnType("decimal(7,1)");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObservationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("TargetRanges");
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
                            Id = new Guid("48ee8729-8c43-4ca2-95c5-5f5f2c0cfc3b"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            ElementName = "about",
                            Language = "en"
                        },
                        new
                        {
                            Id = new Guid("7bc5937a-3e33-4abe-8202-86bcee0d555b"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            ElementName = "about",
                            Language = "es"
                        },
                        new
                        {
                            Id = new Guid("4de11867-663e-4e13-a8cd-1174e7d0e7f1"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            ElementName = "privacyPolicy",
                            Language = "en"
                        },
                        new
                        {
                            Id = new Guid("ed4e76f8-425b-444e-bc55-2e48b2ec4b05"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            ElementName = "privacyPolicy",
                            Language = "es"
                        },
                        new
                        {
                            Id = new Guid("f8a902c9-f7fe-4203-a97d-b9fc7f3135ad"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            ElementName = "announcements",
                            Language = "en"
                        },
                        new
                        {
                            Id = new Guid("7a9c9d5d-d5ef-4380-9f16-5b1b7faf092d"),
                            Created = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            ElementName = "announcements",
                            Language = "es"
                        });
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.UpdateEmailRequest", b =>
                {
                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<DateTimeOffset>("ExpirationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("PatientId");

                    b.ToTable("UpdateEmailRequests");
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
                    b.HasOne("IUGOCare.Domain.Entities.Clinic", "Clinic")
                        .WithMany("ClinicPatients")
                        .HasForeignKey("ClinicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IUGOCare.Domain.Entities.Patient", "Patient")
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

            modelBuilder.Entity("IUGOCare.Domain.Entities.Organization", b =>
                {
                    b.OwnsOne("IUGOCare.Domain.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("OrganizationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("AddressLines")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(200)")
                                .HasMaxLength(200);

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(2)")
                                .HasMaxLength(2);

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(2)")
                                .HasMaxLength(2);

                            b1.Property<string>("ZipCode")
                                .HasColumnType("nvarchar(20)")
                                .HasMaxLength(20);

                            b1.HasKey("OrganizationId");

                            b1.ToTable("Organizations");

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");
                        });
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.Patient", b =>
                {
                    b.HasOne("IUGOCare.Domain.Entities.Provider", "PrimaryCareProvider")
                        .WithMany("Patients")
                        .HasForeignKey("PrimaryCareProviderId");

                    b.OwnsOne("IUGOCare.Domain.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("PatientId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("AddressLines")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(200)")
                                .HasMaxLength(200);

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(2)")
                                .HasMaxLength(2);

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(2)")
                                .HasMaxLength(2);

                            b1.Property<string>("ZipCode")
                                .HasColumnType("nvarchar(20)")
                                .HasMaxLength(20);

                            b1.HasKey("PatientId");

                            b1.ToTable("Addresses");

                            b1.WithOwner()
                                .HasForeignKey("PatientId");
                        });
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.PatientCareManagementProgram", b =>
                {
                    b.HasOne("IUGOCare.Domain.Entities.Provider", "BillingProvider")
                        .WithMany("PatientCareManagementPrograms")
                        .HasForeignKey("BillingProviderId");

                    b.HasOne("IUGOCare.Domain.Entities.CareManagementProgram", "CareManagementProgram")
                        .WithMany("PatientCareManagementPrograms")
                        .HasForeignKey("CareManagementProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IUGOCare.Domain.Entities.Patient", null)
                        .WithMany("PatientCareManagementPrograms")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.PatientEmergencyContact", b =>
                {
                    b.HasOne("IUGOCare.Domain.Entities.Patient", "Patient")
                        .WithOne("EmergencyContact")
                        .HasForeignKey("IUGOCare.Domain.Entities.PatientEmergencyContact", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.Provider", b =>
                {
                    b.HasOne("IUGOCare.Domain.Entities.Organization", "Organization")
                        .WithMany("Providers")
                        .HasForeignKey("OrganizationId");

                    b.OwnsOne("IUGOCare.Domain.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("ProviderId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("AddressLines")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(200)")
                                .HasMaxLength(200);

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(2)")
                                .HasMaxLength(2);

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(2)")
                                .HasMaxLength(2);

                            b1.Property<string>("ZipCode")
                                .HasColumnType("nvarchar(20)")
                                .HasMaxLength(20);

                            b1.HasKey("ProviderId");

                            b1.ToTable("Providers");

                            b1.WithOwner()
                                .HasForeignKey("ProviderId");
                        });
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.TargetRange", b =>
                {
                    b.HasOne("IUGOCare.Domain.Entities.Patient", "Patient")
                        .WithMany("TargetRanges")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IUGOCare.Domain.Entities.UpdateEmailRequest", b =>
                {
                    b.HasOne("IUGOCare.Domain.Entities.Patient", "Patient")
                        .WithOne("UpdateEmailRequest")
                        .HasForeignKey("IUGOCare.Domain.Entities.UpdateEmailRequest", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
