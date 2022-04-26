using System;
using System.Collections.Generic;
using System.Linq;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class Observation : AuditableEntity
    {
        public Observation(): base()
        {
            ObservationsData = new List<ObservationData>();
        }
        public Guid Id { get; set; }
        public Guid ClinicPatientId { get; set; }
        public string ObservationCode { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public string Source { get; set; }
        public string ObservationStatus { get; set; }
        public string ObservationLevel { get; set; }        
        public bool IsReviewed { get; set; }
        public DateTimeOffset? IsReviewedDate { get; set; }
        public string ReviewedByName { get; set; }
        public string Manufacturer { get; set; }

        public IList<ObservationData> ObservationsData { get; }

        /// <summary>
        /// When an Observation's data is updated, we need to set its Change
        /// property. This indicates if the value is rising, falling, or steady.
        /// </summary>
        /// <returns>A list of observation data which needs to be saved</returns>
        public IList<ObservationData> SetObservationChange(Observation previous)
        {
            var list = new List<ObservationData>();
            if (previous is null)
            {
                return list;
            }

            foreach (var data in ObservationsData)
            {
                var previousData = previous.ObservationsData.FirstOrDefault(od => od.ObservationCode == data.ObservationCode);
                bool dataChanged = data.SetObservationChange(previousData);

                if (dataChanged)
                {
                    list.Add(data);
                }
            }

            return list;
        }
    }
}
