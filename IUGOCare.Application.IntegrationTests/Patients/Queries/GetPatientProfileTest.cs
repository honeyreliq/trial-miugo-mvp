using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Patients.Queries.GetProfileInformation;
using IUGOCare.Application.UnitTests.Common;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Queries
{
    public class GetPatientProfileTest : TestBase
    {
        [Test]
        public async Task ShouldReturnPatientProfileViewModel()
        {
            await CreatePatient();

            var query = new GetProfileInformationQuery();
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(PatientProfileVm), result.GetType());
        }

        [Test]
        public async Task ShouldReturnPersonalInformation()
        {
            await CreatePatient();

            var query = new GetProfileInformationQuery();
            var result = await Testing.SendAsync(query);

            result.PatientName.Should().Be("Monique Alicia Caron");
            result.Phone.Should().Be("506 8820 2020");
            result.BirthDate.Should().Be(DateTime.Now.AddDays(-360).Date);
        }

        [Test]
        public async Task ShouldReturnPatientEmergencyContact()
        {
            var patient = await CreatePatient();

            await Testing.AddAsync(new PatientEmergencyContact {
                ClinicPatientId = patient.Clinics.First().ClinicPatientId,
                ContactName = "Deborah Caron",
                Relationship = "Daughter",
                Phone = "345 888 7733"
            });

            var query = new GetProfileInformationQuery();
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.EmergencyContact);
            result.EmergencyContact.ContactName.Should().Be("Deborah Caron");
            result.EmergencyContact.Phone.Should().Be("345 888 7733");
            result.EmergencyContact.Relationship.Should().Be("Daughter");
        }

        [Test]
        public async Task ShouldReturnPrimaryCareProvider()
        {
            var patientId = TestConstants.AllOnesGuid;
            var providerId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();

            Testing.RunAsUser(patientId);

            await Testing.AddAsync(new Organization
            {
                Id = organizationId,
                Name = "Good Health Clinic",
                Phone = "555 989 1234"
            });

            await Testing.AddAsync(new Provider
            {
                Id = providerId,
                Name = "Dr. David Smith",
                Phone = "555 989 1234",
                Type = "Physician",
                OrganizationId = organizationId
            });

            var patient = new Patient
            {
                Id = patientId,
                Auth0Id = patientId.ToString(),
                Active = true,
                AllowMarketingEmails = false,
            };
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patientId,
                ClinicPatientId = Guid.NewGuid(),
                TimeZone = "Australia/Melbourne",
                PrimaryCareProviderId = providerId
            });
            await Testing.AddAsync(patient);

            var query = new GetProfileInformationQuery();
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task ShouldReturnPatientCareManagementPrograms()
        {
            Assume.That((await Testing.GetAll<CareManagementProgram>()).Any(c => c.ShortName.Equals("RPM")),
                "The testing database needs an RPM care program for this test to run.");

            var clinicPatientId = Guid.NewGuid();
            await CreatePatient(clinicPatientId);
            var providerId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();

            var careManagementProgramId = Testing.GetAll<CareManagementProgram>()
                .Result
                .FirstOrDefault(c => c.ShortName.Equals("RPM"))
                .Id;

            await Testing.AddAsync(new Organization
            {
                Id = organizationId,
                Name = "Good Health Clinic",
                Phone = "555 989 1234"
            });

            await Testing.AddAsync(new Provider
            {
                Id = providerId,
                Name = "Dr. David Smith",
                Phone = "555 989 1234",
                Type = "Physician",
                OrganizationId = organizationId
            });

            await Testing.AddAsync(new PatientCareManagementProgram
            {
                ClinicPatientId = clinicPatientId,
                CareManagementProgramId = careManagementProgramId,
                BillingProviderId = providerId
            });

            var query = new GetProfileInformationQuery();
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PatientPrograms);
            result.PatientPrograms.Should().HaveCount(1);
            result.PatientPrograms.FirstOrDefault().CareManagementPrograms.Should().HaveCount(1);
            result.PatientPrograms.FirstOrDefault().CareManagementPrograms.FirstOrDefault().Name.Should().Be("Remote Patient Monitoring");
        }

        private async Task<Patient> CreatePatient(Guid? clinicPatientId = null)
        {
            var patientId = TestConstants.AllOnesGuid;
            Testing.RunAsUser(patientId);

            var patient = new Patient
            {
                Id = patientId,
                Auth0Id = patientId.ToString(),
                Active = true,
                AllowMarketingEmails = false,
            };
            patient.Clinics.Add(new ClinicPatient
            {
                PatientId = patientId,
                ClinicPatientId = clinicPatientId ?? Guid.NewGuid(),
                TimeZone = "Australia/Melbourne",
                Phone = "506 8820 2020",
                BirthDate = DateTime.Now.AddDays(-360),
                GivenName = "Monique",
                MiddleName = "Alicia",
                FamilyName = "Caron"
            });
            await Testing.AddAsync(patient);

            return patient;
        }
    }
}
