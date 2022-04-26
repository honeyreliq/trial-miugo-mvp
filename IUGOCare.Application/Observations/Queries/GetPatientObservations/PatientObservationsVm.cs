using System.Collections.Generic;

namespace IUGOCare.Application.Observations.Queries.GetPatientObservations
{
    public class PatientObservationsVm
    {
        public IList<PatientObservationDto> Observations { get; set; }
    }
}
