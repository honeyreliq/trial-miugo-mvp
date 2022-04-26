using System;
using System.Collections.Generic;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Observations;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging.ExternalSystemEvents
{
    [TestFixture]
    public class ObservationCreatedDtoMappersTests
    {
        [Test]
        public void ShouldMapProperly()
        {
            var ocese = new ObservationCreatedDto
            {
                Id = Guid.NewGuid(),
                PatientId = Guid.NewGuid(),
                ObservationCode = "blood-glucose",
                EffectiveDate = DateTimeOffset.UtcNow,
                IsReviewed = false,
                ObservationData = new List<ObservationCreatedDto.ObservationDataDto>()
            };
            ocese.ObservationData.Add(new ObservationCreatedDto.ObservationDataDto
            {
                ObservationCode = "walk",
                Value = 500,
                Unit = "mi",
            });

            var command = ocese.MapToCreateObservationCommand();

            Assert.AreEqual(ocese.Id, command.ObservationId);
            Assert.AreEqual(ocese.PatientId, command.ClinicPatientId);
            Assert.AreEqual(ocese.ObservationCode, command.ObservationCode);
            Assert.AreEqual(ocese.EffectiveDate, command.EffectiveDate);
            Assert.AreEqual(ocese.IsReviewed, command.IsReviewed);

            Assert.AreEqual(1, command.ObservationDataList.Count);
            Assert.AreEqual("walk", command.ObservationDataList[0].ObservationCode);
            Assert.AreEqual(500, command.ObservationDataList[0].Value);
            Assert.AreEqual("mi", command.ObservationDataList[0].Unit);
        }
    }
}
