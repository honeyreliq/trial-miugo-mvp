using System.Collections.Generic;

namespace IUGOCare.Application.Patients.Queries.GetProfileInformation
{
    public class PatientCareManagementProgramDto
    {
        public ProviderDto BillingProvider { get; set; }
        public IList<CareManagementProgramDto> CareManagementPrograms { get; set; }
    }
}
