using System;
using System.Collections.Generic;
using System.Text;

namespace IUGOCare.Domain.Common
{
    public abstract class AuditableEntity
    {
        public string CreatedBy { get; set; }

        public DateTimeOffset Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset? LastModified { get; set; }
    }
}
