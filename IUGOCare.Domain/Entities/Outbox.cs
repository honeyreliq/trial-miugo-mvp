using System;

namespace IUGOCare.Domain.Entities
{
    public class Outbox
    {
        public Guid MessageId { get; set; }
        public string CorrelationId { get; set; }
        public string Label { get; set; }
        public string MessageBody { get; set; }
        public DateTimeOffset DateStaged { get; set; }
        public bool ExpectsAcknowledgement { get; set; }
        public bool MessageSent { get; set; }
        public bool MessageAcknowledged { get; set; }
        public string Errors { get; set; }
    }
}
