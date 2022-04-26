using System;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using IUGOCare.Application.Patients.Commands.UpdateEmailAddress;
using IUGOCare.Application.UnitTests.Common;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using IUGOCare.Application.Common.Interfaces;

namespace IUGOCare.Application.UnitTests.Patients.Validations
{
    public class PatientRequestUpdateEmailCommandValidatorTests : CommandTestBase
    {
        private PatientRequestUpdateEmailCommandValidator validator;

        [SetUp]
        public void Setup()
        {
            var mIdentityService = new Mock<IIdentityService>();
            mIdentityService
                .Setup(i => i.ValidateUserAndPassword("PelleAlmqvist@example.com", "ValidPassword"))
                .Returns(Task.FromResult(true));
            mIdentityService
                .Setup(i => i.ValidateUserAndPassword("PelleAlmqvist@example.com", "BadPassword"))
                .Returns(Task.FromResult(false));
            validator = new PatientRequestUpdateEmailCommandValidator(Context, mIdentityService.Object);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("Invalid email")]
        [TestCase("Invalidemail@")]
        [TestCase("Invalidemail@example")]
        [TestCase("Invalidemail@example.")]
        public void ShouldHaveErrorWhenEmailAddressIsInvalid(string emailAddress)
        {
            validator.ShouldHaveValidationErrorFor(e => e.EmailAddress, emailAddress);
        }

        [Test]
        public void ShouldNotHaveErrorWhenEmailIsValid()
        {
            validator.ShouldNotHaveValidationErrorFor(e => e.EmailAddress, "testuser@example.com");
        }

        [Test]
        public void ShouldHaveErrorWhenGuidAreInvalid()
        {
            var result = validator.TestValidate(new PatientRequestUpdateEmailCommand() { PatientId = Guid.Empty });
            result.ShouldHaveValidationErrorFor(e => e.PatientId);
        }

        [Test]
        public async Task ShouldHaveErrorWhenInactiveUserIsProvided()
        {
            var patient = await Context.Patients.FirstAsync(p => p.Active == false);
            var result = validator.TestValidate(new PatientRequestUpdateEmailCommand() { PatientId = patient.Id, EmailAddress = "test@example.com" });
            result.ShouldHaveValidationErrorFor(e => e.PatientId);
        }

        [Test]
        public async Task ShouldHaveErrorWhenAnInUseEmailAddressIsUsed()
        {
            var patient = await Context.Patients.FirstAsync(p => p.Active == false);
            var result = validator.TestValidate(new PatientRequestUpdateEmailCommand() { PatientId = patient.Id, EmailAddress = "NicholausArson@example.com" });
            result.ShouldHaveValidationErrorFor(e => e.PatientId);
        }

        [Test]
        public async Task ShouldHaveErrorWhenThePasswordIsIncorrect()
        {
            var patient = await Context.Patients.FirstAsync(p => p.Active == true);
            var result = validator.TestValidate(new PatientRequestUpdateEmailCommand() { PatientId = patient.Id, EmailAddress = "test@example.com", Password = "BadPassword" });
            Assert.AreEqual("The password is incorrect.", result.Errors[0].ErrorMessage);
        }

        [Test]
        public async Task ShouldPass()
        {
            var patient = await Context.Patients.FirstAsync(p => p.Active == true);
            var result = validator.TestValidate(new PatientRequestUpdateEmailCommand() { PatientId = patient.Id, EmailAddress = "test@example.com", Password = "ValidPassword" });
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
