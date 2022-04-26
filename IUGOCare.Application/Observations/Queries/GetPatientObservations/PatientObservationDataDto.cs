using System;
using AutoMapper;
using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Common;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Observations.Queries.GetPatientObservations
{
    public class PatientObservationDataDto : IMapFrom<ObservationData>
    {
        public string ObservationCode { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
    }
}
