using IUGOCare.Audit.Models;

namespace IUGOCare.Audit.Repositories
{
    public interface IApiAuditRepository
    {
        ApiAudit Append(ApiAudit apiAudit);
    }
}
