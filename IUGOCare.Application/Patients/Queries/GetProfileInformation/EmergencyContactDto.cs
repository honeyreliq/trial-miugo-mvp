using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Patients.Queries.GetProfileInformation
{
    public class EmergencyContactDto : IMapFrom<PatientEmergencyContact>
    {
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Relationship { get; set; }
    }
}
