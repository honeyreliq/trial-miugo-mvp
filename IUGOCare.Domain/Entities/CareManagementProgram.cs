using System;
using System.Collections.Generic;
using IUGOCare.Domain.Common;

namespace IUGOCare.Domain.Entities
{
    public class CareManagementProgram : AuditableEntity
    {
        public CareManagementProgram()
        {
            PatientCareManagementPrograms = new List<PatientCareManagementProgram>();
        }

        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }

        public IList<PatientCareManagementProgram> PatientCareManagementPrograms { get; }
    }
}
