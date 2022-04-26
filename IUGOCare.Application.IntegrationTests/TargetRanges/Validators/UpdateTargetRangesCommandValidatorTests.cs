using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.TargetRanges.Commands.UpdateTargetRanges;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.TargetRanges.Validators
{
    using static Testing;
    [TestFixture]
    public class UpdateTargetRangesCommandValidatorTests : TestBase
    {
        [Test]
        public void ShouldRequireNonNullTargetRanges()
        {
            var command = new UpdateTargetRangesCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("TargetRanges"))
                    .And.Errors["TargetRanges"].Should().Contain("TargetRanges are required.");
        }

        [Test]
        public void ShouldRequireNonEmptyTargetRanges()
        {
            var targetRanges = new List<TargetRange>();
            var command = new UpdateTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("TargetRanges"))
                    .And.Errors["TargetRanges"].Should().Contain("TargetRanges are required.");
        }

        [Test]
        public void ShouldRequireClinicPatientId()
        {
            var targetRanges = new List<TargetRange>
            {
                new TargetRange
                {
                    ObservationCode = "someObservationCode",
                    Unit = "someUnit",
                    CriticalHigh = 1,
                    AtRiskHigh = 2,
                    AtRiskLow = 3,
                    CriticalLow = 4
                }
            };
            var command = new UpdateTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("ClinicPatientId"))
                    .And.Errors["ClinicPatientId"].Should().Contain("ClinicPatientId is required.");
        }

        [Test]
        public void ShouldRequireObservationCode()
        {
            var targetRanges = new List<TargetRange>
            {
                new TargetRange
                {
                    ObservationCode = string.Empty
                }
            };
            var command = new UpdateTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("TargetRanges[0].ObservationCode"))
                    .And.Errors["TargetRanges[0].ObservationCode"].Should().Contain("Observation Code is required.");
        }

        [Test]
        public void ShouldRequireUnit()
        {
            var targetRanges = new List<TargetRange>
            {
                new TargetRange
                {
                    ObservationCode = "someObservationCode",
                    Unit = string.Empty
                }
            };
            var command = new UpdateTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("TargetRanges[0].Unit"))
                    .And.Errors["TargetRanges[0].Unit"].Should().Contain("Unit is required.");
        }

        [Test]
        public void ShouldRequireValidValues()
        {
            var expectedErrors = new Dictionary<string, string>
            {
                { "TargetRanges[0].CriticalHigh", "CriticalHigh must be greater than or equal to 0." },
                { "TargetRanges[0].AtRiskHigh", "AtRiskHigh must be greater than or equal to 0." },
                { "TargetRanges[0].AtRiskLow", "AtRiskLow must be greater than or equal to 0." },
                { "TargetRanges[0].CriticalLow", "CriticalLow must be greater than or equal to 0." }
            };

            var targetRanges = new List<TargetRange>
            {
                new TargetRange
                {
                    ObservationCode = "someObservationCode",
                    Unit = "someUnit",
                    CriticalHigh = -1,
                    AtRiskHigh = (decimal)-0.1,
                    AtRiskLow = -10,
                    CriticalLow = -100
                }
            };
            var command = new UpdateTargetRangesCommand
            {
                TargetRanges = targetRanges
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>().Where(ex => ContainsErrors(expectedErrors, ex.Errors));
        }

        private bool ContainsErrors(IDictionary<string, string> expectedErrors, IDictionary<string, string[]> actualErrors)
        {
            foreach (var error in expectedErrors)
            {
                if (!actualErrors.ContainsKey(error.Key) || !actualErrors[error.Key].Contains(error.Value))
                    return false;
            }

            return true;
        }
    }
}
