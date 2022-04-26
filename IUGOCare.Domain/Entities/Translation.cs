using System;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class Translation : AuditableEntity
    {
        public Guid Id { get; set; }
        public string ElementName { get; set; }
        public string Language { get; set; }
        public byte[] FileContent { get; set; }
    }
}
