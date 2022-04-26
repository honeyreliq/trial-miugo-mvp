using System;
using System.Collections.Generic;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class Organization : AuditableEntity
    {
        public Organization()
        {
            Providers = new List<Provider>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }

        public IList<Provider> Providers { get; }
    }
}
