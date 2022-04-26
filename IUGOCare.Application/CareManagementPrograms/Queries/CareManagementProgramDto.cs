using System;
using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.CareManagementPrograms.Queries
{
    public class CareManagementProgramDto : IMapFrom<CareManagementProgram>
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
}
