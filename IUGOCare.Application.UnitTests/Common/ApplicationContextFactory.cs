using System;
using System.Collections.Generic;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using IUGOCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IUGOCare.Application.UnitTests.Common
{
    public class ApplicationContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var currentUserService = new Mock<ICurrentUserService>();
            var dateTimeOffset = new Mock<IDateTimeOffset>();

            var context = new ApplicationDbContext(options, currentUserService.Object, dateTimeOffset.Object);

            context.Database.EnsureCreated();

            Populate(context);

            context.SaveChanges();

            return context;
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }

        private static void Populate(ApplicationDbContext context)
        {
            var patientOneId = TestConstants.AllOnesGuid;

            var clinicOne = new Clinic
            {
                Id = TestConstants.AllOnesGuid,
                Subdomain = "someClinicSubdomain"
            };

            var clinicTwo = new Clinic
            {
                Id = TestConstants.AllTwosGuid,
                Subdomain = "someOtherClinicSubdomain"
            };

            var patientWithClinics = new Patient
            {
                Id = patientOneId,
            };

            patientWithClinics.Clinics.Add(new ClinicPatient
            {
                PatientId = patientOneId,
                ClinicId = clinicOne.Id
            });

            patientWithClinics.Clinics.Add(new ClinicPatient
            {
                PatientId = patientOneId,
                ClinicId = clinicTwo.Id
            });


            var pelleAlmqvist = new Patient
            {
                Active = true,
                EmailAddress = "PelleAlmqvist@example.com",
            };
            pelleAlmqvist.Clinics.Add(new ClinicPatient
            {
                GivenName = "Per",
                MiddleName = "Howlin' Pelle",
                FamilyName = "Almqvist",
            });

            var nicholausArson = new Patient
            {
                Active = true,
                EmailAddress = "NicholausArson@example.com",
            };
            nicholausArson.Clinics.Add(new ClinicPatient
            {
                GivenName = "Niklas",
                MiddleName = "Arson",
                FamilyName = "Almqvist",
            });

            var patients = new List<object>
            {
                patientWithClinics,
                clinicOne,
                clinicTwo,
                pelleAlmqvist,
                nicholausArson,
            };            

            context.AddRange(patients);
        }
    }
}
