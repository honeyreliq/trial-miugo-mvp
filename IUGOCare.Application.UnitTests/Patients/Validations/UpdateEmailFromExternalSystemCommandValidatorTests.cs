using System;
using FluentValidation.TestHelper;
using IUGOCare.Application.Patients.Commands.UpdateEmailFromExternalSystem;
using IUGOCare.Application.UnitTests.Common;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Patients.Validations
{
    [TestFixture]
    public class UpdateEmailFromExternalSystemCommandValidatorTests
    {
        [Test]
        public void Validate_AllNullProperties_ShouldThrowValidationExceptions()
        {
            // Arrange
            var command = new UpdateEmailFromExternalSystemCommand(Guid.Empty, null);
            var validator = new UpdateEmailFromExternalSystemCommandValidator();

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.ClinicPatientId);
            result.ShouldHaveValidationErrorFor(r => r.EmailAddress);
        }

        [Test]
        public void Validate_EmptyClinicPatientId_ShouldThrowValidationException()
        {
            // Arrange
            var command = new UpdateEmailFromExternalSystemCommand(
                Guid.Empty, "validemail@example.com");
            var validator = new UpdateEmailFromExternalSystemCommandValidator();

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.ClinicPatientId);
            result.ShouldNotHaveValidationErrorFor(r => r.EmailAddress);
        }

        [Test]
        [TestCase("invalid")]
        [TestCase("invalid@email")]
        [TestCase("invalid@email.")]
        public void Validate_InvalidEmailFormat_ShouldThrowValidationException(string emailAddress)
        {
            // Arrange
            var command = new UpdateEmailFromExternalSystemCommand(
                TestConstants.AllOnesGuid, emailAddress);
            var validator = new UpdateEmailFromExternalSystemCommandValidator();

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(r => r.ClinicPatientId);
            result.ShouldHaveValidationErrorFor(r => r.EmailAddress);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        public void Validate_EmptyOrWhitespaceEmail_ShouldBeValid(string emailAddress)
        {
            // Arrange
            var command = new UpdateEmailFromExternalSystemCommand(
                TestConstants.AllOnesGuid, emailAddress);
            var validator = new UpdateEmailFromExternalSystemCommandValidator();

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        [TestCase("validemail@example.com")]
        [TestCase("validemail@example.co.uk")]
        [TestCase("valid_email@example.com")]
        [TestCase("valid.email@example.com")]
        public void Validate_AllValidProperties_ShouldBeValid(string emailAddress)
        {
            // Arrange
            var command = new UpdateEmailFromExternalSystemCommand(
                TestConstants.AllOnesGuid, emailAddress);
            var validator = new UpdateEmailFromExternalSystemCommandValidator();

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
