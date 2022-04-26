using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.TargetRanges.Queries
{
    public class TargetRangeDto : IMapFrom<TargetRange>
    {
        public string ObservationCode { get; set; }
        public string Unit { get; set; }
        public decimal CriticalHigh { get; set; }
        public decimal AtRiskHigh { get; set; }
        public decimal AtRiskLow { get; set; }
        public decimal CriticalLow { get; set; }
    }
}
