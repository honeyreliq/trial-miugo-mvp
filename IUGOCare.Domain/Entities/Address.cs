using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Domain.Entities
{
    [Owned]
    public class Address
    {
        public string AddressLines { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
