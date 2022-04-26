using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Observations.Queries.GetPatientObservations
{
    public class PatientObservationDto : IMapFrom<Observation>
    {
        public PatientObservationDto()
        {
            ObservationsData = new List<PatientObservationDataDto>();
        }

        public Guid Id { get; set; }
        public string ObservationCode { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public string Source { get; set; }
        public string ObservationStatus { get; set; }
        public string ObservationLevel { get; set; }
        public bool IsReviewed { get; set; }
        public DateTimeOffset IsReviewedDate { get; set; }
        public string ReviewedByName { get; set; }
        public string Manufacturer { get; set; }
        public string Change { get; set; }

        public IList<PatientObservationDataDto> ObservationsData { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Observation, PatientObservationDto>()
                    .ForMember(d => d.Change, opt => opt.MapFrom<ObservationChangeResolver>())
            ;
        }
    }

    public class ObservationChangeResolver : IValueResolver<Observation, PatientObservationDto, string>
    {
        public string Resolve(Observation source, PatientObservationDto destination, string member, ResolutionContext context)
        {
            switch (source.ObservationCode)
            {
                case "blood-pressure":
                    var systolic = source.ObservationsData.FirstOrDefault(d => d.ObservationCode == "systolic");
                    var diastolic = source.ObservationsData.FirstOrDefault(d => d.ObservationCode == "diastolic");
                    var heartRate = source.ObservationsData.FirstOrDefault(d => d.ObservationCode == "heart-rate");
                    return $"{GetChangeAsString(systolic)},{GetChangeAsString(diastolic)},{GetChangeAsString(heartRate)}";
                case "oxygen-saturation":
                    var pulseOx = source.ObservationsData.FirstOrDefault(d => d.ObservationCode == "oxygen-saturation");
                    return GetChangeAsString(pulseOx);
                case "sleep":
                    var sleep = source.ObservationsData.FirstOrDefault(d => d.ObservationCode == "total");
                    return GetChangeAsString(sleep);
                case "workout":
                    var duration = source.ObservationsData.FirstOrDefault(d => d.ObservationCode == "fitness-duration");
                    return GetChangeAsString(duration);
                case "blood-glucose":
                    var bloodGlucose = source.ObservationsData.FirstOrDefault(d => d.ObservationCode == "blood-glucose");
                    return GetChangeAsString(bloodGlucose);
                default:
                    return GetChangeAsString(source.ObservationsData.FirstOrDefault());
            }
        }

        private string GetChangeAsString(ObservationData data)
        {
            var change = data?.Change ?? ObservationChange.Steady;
            switch (change)
            {
                case ObservationChange.Rising:
                    return "Rising";
                case ObservationChange.Falling:
                    return "Falling";
                case ObservationChange.Steady:
                default:
                    return "Steady";
            }
        }
    }
}
