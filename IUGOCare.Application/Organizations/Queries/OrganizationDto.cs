using System;
using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Organizations.Queries
{
    public class OrganizationDto : IMapFrom<Organization>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AddressAddressLines { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressCountry { get; set; }
    }
}
