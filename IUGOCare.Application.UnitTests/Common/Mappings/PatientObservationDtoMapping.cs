using AutoMapper;
using IUGOCare.Application.Common.Mappings;
using IUGOCare.Application.Observations.Queries.GetPatientObservations;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Common.Mappings
{
    public class PatientObservationDtoMapping
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public PatientObservationDtoMapping()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        [TestCase(ObservationChange.Rising, ObservationChange.Rising, "Rising,Rising,Steady")]
        [TestCase(ObservationChange.Rising, ObservationChange.Falling, "Rising,Falling,Steady")]
        [TestCase(ObservationChange.Steady, ObservationChange.Steady, "Steady,Steady,Steady")]
        public void ShouldMapBloodPressure(ObservationChange systolic, ObservationChange diastolic, string expected)
        {
            var o = new Observation { ObservationCode = "blood-pressure" };
            o.ObservationsData.Add(new ObservationData { ObservationCode = "systolic", Change = systolic });
            o.ObservationsData.Add(new ObservationData { ObservationCode = "diastolic", Change = diastolic });
            o.ObservationsData.Add(new ObservationData { ObservationCode = "heart-rate", Change = ObservationChange.Steady });

            var dto = _mapper.Map<PatientObservationDto>(o);
            Assert.AreEqual(expected, dto.Change);
        }

        [Test]
        [TestCase(ObservationChange.Rising, "Rising")]
        [TestCase(ObservationChange.Falling, "Falling")]
        [TestCase(ObservationChange.Steady, "Steady")]
        public void ShouldMapPulseOx(ObservationChange pulseOx, string expected)
        {
            var o = new Observation { ObservationCode = "oxygen-saturation" };
            o.ObservationsData.Add(new ObservationData { ObservationCode = "oxygen-saturation", Change = pulseOx });
            
            var dto = _mapper.Map<PatientObservationDto>(o);
            Assert.AreEqual(expected, dto.Change);
        }
    }
}
