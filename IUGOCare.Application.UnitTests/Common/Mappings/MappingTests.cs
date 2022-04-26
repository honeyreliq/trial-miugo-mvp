using System;
using AutoMapper;
using IUGOCare.Application.Common.Mappings;
using IUGOCare.Application.Observations.Queries.GetPatientObservations;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Test]
        [TestCase(typeof(Observation), typeof(PatientObservationDto))]
        [TestCase(typeof(ObservationData), typeof(PatientObservationDataDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);
            _mapper.Map(instance, source, destination);
        }
    }
}
