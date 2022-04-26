using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Patients.Queries.GetProfileInformation
{
    public class CareManagementProgramDto : IMapFrom<CareManagementProgram>
    {
        public string Name { get; set; }
    }
}
