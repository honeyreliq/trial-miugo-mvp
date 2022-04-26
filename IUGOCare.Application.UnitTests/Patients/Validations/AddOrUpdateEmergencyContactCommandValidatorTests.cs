using FluentValidation;
using FluentValidation.TestHelper;
using IUGOCare.Application.Patients.Commands.AddOrUpdateEmergencyContact;
using IUGOCare.Application.UnitTests.Common;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Patients.Validations
{
    [TestFixture]
    public class AddOrUpdateEmergencyContactCommandValidatorTests
    {
        private AbstractValidator<AddOrUpdateEmergencyContactCommand> _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new AddOrUpdateEmergencyContactCommandValidator();
        }

        [Test]
        public void Validate_NullClinicPatientId_ShouldThrowValidationException()
        {
            var command = new AddOrUpdateEmergencyContactCommand();

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(r => r.ClinicPatientId);
        }

        [TestCase("123")]
        [TestCase("   4")]
        [TestCase("5-*")]
        [TestCase("()")]
        [TestCase("(123)")]
        [TestCase("(123) 1")]
        [TestCase("()")]
        [TestCase("12 12 12")]
        [TestCase("123456789")]
        public void Validate_InvalidPhoneFormat_ShouldThrowValidationException(string phone)
        {
            var clinicPatientId = TestConstants.AllOnesGuid;

            var command = new AddOrUpdateEmergencyContactCommand
            {
                ClinicPatientId = clinicPatientId,
                ContactName = "Todd Rundgren",
                Relationship = "Father",
                Phone = phone
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(r => r.Phone);
        }

        [TestCase("", "", "")]
        [TestCase("   ", "   ", "   ")]
        [TestCase(null, null, null)]
        public void Validate_NullOrEmptyProperties_ShouldNotThrowAnyValidationExceptions(string name, string phone, string relationship)
        {
            var clinicPatientId = TestConstants.AllOnesGuid;

            var command = new AddOrUpdateEmergencyContactCommand
            {
                ClinicPatientId = clinicPatientId,
                ContactName = name,
                Relationship = relationship,
                Phone = phone
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validate_ValidProperties_ShouldNotThrowAnyValidationExceptions()
        {
            var clinicPatientId = TestConstants.AllOnesGuid;

            var command = new AddOrUpdateEmergencyContactCommand
            {
                ClinicPatientId = clinicPatientId,
                ContactName = "Steven Tyler",
                Relationship = "Father",
                Phone = "555-123-4567"
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
