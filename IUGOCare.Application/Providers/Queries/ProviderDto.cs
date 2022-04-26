using System;
using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Providers.Queries
{
    public class ProviderDto : IMapFrom<Provider>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AddressAddressLines { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string AddressZipCode { get; set; }
    }
}
