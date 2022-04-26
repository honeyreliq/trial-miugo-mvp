using System;
using System.Collections.Generic;
using System.Text;

namespace IUGOCare.Domain.Entities
{
    public class PatientTour
    {
        public Guid PatientId { get; set; }
        public string TourKey { get; set; }
        public DateTimeOffset Completed { get; set; }
        public CompletionReason CompletionReason { get; set; }

        public Patient Patient { get; set; }
    }

    public enum CompletionReason
    {
        Finished = 1,
        Skipped = 2,
    }
}
