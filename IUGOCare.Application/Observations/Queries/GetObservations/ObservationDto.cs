using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;
using System;

namespace IUGOCare.Application.Observations.Queries.GetObservations
{
    public class ObservationDto : IMapFrom<Observation>
    {
        public Guid Id { get; set; }
        public Guid ClinicPatientId { get; set; }
        public string ObservationCode { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public string Source { get; set; }
        public string ObservationStatus { get; set; }
        public string ObservationLevel { get; set; }
        public bool IsReviewed { get; set; }
        public DateTimeOffset IsReviewedDate { get; set; }
        public string ReviewedByName { get; set; }
        public string Manufacturer { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTimeOffset? LastModified { get; set; }
    }
}
