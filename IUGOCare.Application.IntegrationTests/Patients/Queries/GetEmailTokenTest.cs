using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Patients.Queries.GetEmailToken;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.Patients.Queries
{
    [TestFixture]
    public class GetEmailTokenTest
    {
        private Guid _patientId2;

        [SetUp]
        public async Task Setup()
        {
            var patientId1 = Guid.NewGuid();
            _patientId2 = Guid.NewGuid();
            Testing.RunAsUser(patientId1);
            var patient1 = new Patient
            {
                Id = patientId1,
                Auth0Id = patientId1.ToString(),
                Active = true,
                AllowMarketingEmails = false,
            };
            patient1.Clinics.Add(new ClinicPatient {
                TimeZone = "Australia/Melbourne",
                GivenName = "Chris",
                FamilyName = "Dangerous",
            });
            var patient2 = new Patient
            {
                Id = _patientId2,
                Auth0Id = _patientId2.ToString(),
                Active = true,
                AllowMarketingEmails = false,
            };
            patient2.Clinics.Add(new ClinicPatient {
                TimeZone = "Australia/Melbourne",
                GivenName = "Vigilante",
                FamilyName = "Carlstroem",
            });
            await Testing.AddAsync(patient1);
            await Testing.AddAsync(patient2);

            var validToken = new UpdateEmailRequest
            {
                Token = "a-valid-token",
                PatientId = patientId1,
                ExpirationDate = DateTimeOffset.UtcNow.AddHours(2),
                EmailAddress = "an-updated-email-address@example.com",
            };

            var invalidToken = new UpdateEmailRequest
            {
                Token = "an-expired-token",
                PatientId = _patientId2,
                ExpirationDate = DateTimeOffset.UtcNow.AddHours(-2),
                EmailAddress = "an-updated-email-address@example.com",
            };
            await Testing.AddAsync(validToken);
            await Testing.AddAsync(invalidToken);
        }

        [Test]
        public async Task ShouldReturnTheViewModel()
        {
            var query = new GetEmailToken { Token = "a-valid-token" };
            EmailTokenVm result = await Testing.SendAsync(query);
            Assert.IsNotNull(result);
            Assert.AreEqual("Chris", result.GivenName);
            Assert.AreEqual("Dangerous", result.FamilyName);
        }

        [Test]
        public void ShouldThrowAnErrorForWrongTokens()
        {
            var query = new GetEmailToken { Token = "a-valid-token-for-no-one" };
            Assert.ThrowsAsync<NotFoundException>(async () => await Testing.SendAsync(query));
        }

        [Test]
        public void ShouldThrowAnErrorForOtherPeoplesTokens()
        {
            var query = new GetEmailToken { Token = "an-expired-token" };
            Assert.ThrowsAsync<NotFoundException>(async () => await Testing.SendAsync(query));
        }

        [Test]
        public void ShouldThrowAnErrorForExpiredTokens()
        {
            Testing.RunAsUser(_patientId2);
            var query = new GetEmailToken { Token = "an-expired-token" };
            Assert.ThrowsAsync<TokenExpiredException>(async () => await Testing.SendAsync(query));
        }
    }
}
