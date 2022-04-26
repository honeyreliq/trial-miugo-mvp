using System;
using FluentValidation.TestHelper;
using IUGOCare.Application.Observations.Commands.ReviewObservation;
using IUGOCare.Application.Patients.Commands.UpdateEmailAddress;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Observations.Validations
{
    public class ReviewObservationValidatorTests
    {
        private ReviewObservationValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new ReviewObservationValidator();
        }

        [Test]
        public void ShouldPassValidationWhenProperlyFilledIn()
        {
            var command = new ReviewObservationCommand();
            command.Id = Guid.NewGuid();
            command.IsReviewedDate = DateTimeOffset.UtcNow;
            command.ReviewedByName = "Aaron Burr";

            var result = validator.TestValidate(command);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ShouldHaveErrorsWhenEmptyObjectPassed()
        {
            var roc = new ReviewObservationCommand();
            var result = validator.TestValidate(new ReviewObservationCommand());
            result.ShouldHaveValidationErrorFor(o => o.Id);
            result.ShouldHaveValidationErrorFor(o => o.ReviewedByName);
            result.ShouldHaveValidationErrorFor(o => o.IsReviewedDate);
        }
    }
}
