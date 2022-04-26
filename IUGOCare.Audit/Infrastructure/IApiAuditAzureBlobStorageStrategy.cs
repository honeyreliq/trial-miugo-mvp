using IUGOCare.Audit.Models;

namespace IUGOCare.Audit.Infrastructure
{
    public interface IApiAuditAzureBlobStorageStrategy
    {
        bool Add(ApiAudit apiAudit);
        bool Flush();
    }
}
