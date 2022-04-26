using System;
using System.Collections.Generic;
using System.Text;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class ObservationData : AuditableEntity
    {
        public ObservationData(): base()
        {

        }
        public Guid Id { get; set; }
        public Observation Observation { get; set; }
        public Guid ObservationId { get; set; }
        public string ObservationCode { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public ObservationChange Change { get; set; }

        public bool SetObservationChange(ObservationData previousData)
        {
            if (previousData is null || ObservationCode != previousData.ObservationCode)
            {
                return false;
            }

            if (Value == previousData.Value)
            {
                Change = ObservationChange.Steady;
                return false;
            }
            else if (Value > previousData.Value)
            {
                Change = ObservationChange.Rising;
            }
            else
            {
                Change = ObservationChange.Falling;
            }

            return true;
        }
    }

    public enum ObservationChange
    {
        Steady, Rising, Falling
    }
}
