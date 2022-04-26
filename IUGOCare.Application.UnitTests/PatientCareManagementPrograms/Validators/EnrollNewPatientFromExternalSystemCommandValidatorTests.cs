using System;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using IUGOCare.Application.PatientCareManagementPrograms.Commands.EnrollNewPatientFromExternalSystem;
using IUGOCare.Application.UnitTests.Common;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.PatientCareManagementPrograms.Validators
{
    [TestFixture]
    public class EnrollNewPatientFromExternalSystemCommandValidatorTests : CommandTestBase
    {
        private EnrollNewPatientFromExternalSystemCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new EnrollNewPatientFromExternalSystemCommandValidator();
        }

        [Test]
        public void GivenANullCareProgramList_ThrowsValidationException()
        {
            // Arrange
            var command = new EnrollNewPatientFromExternalSystemCommand(Guid.NewGuid(), null);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CarePrograms);
        }

        [Test]
        public void GivenAnEmptyClinicPatientId_ThrowsValidationException()
        {
            // Arrange
            var command = new EnrollNewPatientFromExternalSystemCommand(Guid.Empty, new List<string>());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ClinicPatientId);
        }

        [Test]
        public void GivenAnEmptyCareProgramList_ValidatesSuccessfully()
        {
            // Arrange
            var command = new EnrollNewPatientFromExternalSystemCommand(Guid.NewGuid(), new List<string>());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.CarePrograms);
        }

        [Test]
        public void GivenAllValidProperties_ValidatesSuccessfully()
        {
            // Arrange
            var command = new EnrollNewPatientFromExternalSystemCommand(Guid.NewGuid(),
                new List<string>
                {
                    "CCM",
                    "RPM"
                });

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
