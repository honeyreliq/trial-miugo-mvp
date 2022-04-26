using System;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using IUGOCare.Application.Patients.Commands.UpdateEmailAddress;
using IUGOCare.Application.UnitTests.Common;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Patients.Validations
{
    public class UpdateEmailAddressCommandValidatorTests : CommandTestBase
    {
        private UpdateEmailAddressCommandValidator _validator;

        [SetUp]
        public void Setup()
        {

            _validator = new UpdateEmailAddressCommandValidator(Context);
            
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("Invalid email")]
        [TestCase("Invalidemail@")]
        [TestCase("Invalidemail@example")]
        [TestCase("Invalidemail@example.")]
        public async Task ShouldHaveErrorWhenEmailAddressIsInvalid(string emailAddress)
        {
            var patient = await Context.Patients.FirstAsync();
            var result = _validator.TestValidate(new UpdateEmailAddressCommand
            {
                PatientId = patient.Id,
                EmailAddress = emailAddress
            });

            result.ShouldHaveValidationErrorFor(r => r.EmailAddress);
        }

        [Test]
        public void ShouldNotHaveErrorWhenEmailIsValid()
        {
            var result = _validator.TestValidate(new UpdateEmailAddressCommand
            {
                PatientId = Guid.Empty,
                EmailAddress = "testuser@example.com"
            });

            result.ShouldNotHaveValidationErrorFor(r => r.EmailAddress);
        }

        [Test]
        public void ShouldHaveErrorsWhenFieldsAreInvalid()
        {
            var result = _validator.TestValidate(new UpdateEmailAddressCommand
            {
                PatientId = Guid.Empty,
                EmailAddress = null
            });

            result.ShouldHaveValidationErrorFor(e => e.PatientId);
            result.ShouldHaveValidationErrorFor(e => e.EmailAddress);
        }
    }
}
