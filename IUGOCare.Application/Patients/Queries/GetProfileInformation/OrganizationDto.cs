using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Patients.Queries.GetProfileInformation
{
    public class OrganizationDto : IMapFrom<Organization>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string AddressAddressLines { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string AddressZipCode { get; set; }
    }
}
