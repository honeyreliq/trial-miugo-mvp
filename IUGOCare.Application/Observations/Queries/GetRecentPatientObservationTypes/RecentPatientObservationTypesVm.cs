using System.Collections.Generic;

namespace IUGOCare.Application.Observations.Queries.GetRecentPatientObservationTypes
{
    public class RecentPatientObservationTypesVm
    {
        public IList<ObservationTypeDto> ObservationTypes { get; set; }
    }
}
