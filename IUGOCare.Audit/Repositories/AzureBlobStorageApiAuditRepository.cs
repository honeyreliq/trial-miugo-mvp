using System;
using IUGOCare.Audit.Infrastructure;
using IUGOCare.Audit.Models;

namespace IUGOCare.Audit.Repositories
{
    public class AzureBlobStorageApiAuditRepository : IApiAuditRepository
    {

        private readonly IApiAuditAzureBlobStorageStrategy _writer;

        public AzureBlobStorageApiAuditRepository(IApiAuditAzureBlobStorageStrategy writer)
        {
            _writer = writer;
        }

        public ApiAudit Append(ApiAudit apiAudit)
        {
            apiAudit.Id = Guid.NewGuid();
            _writer.Add(apiAudit);
            return apiAudit;
        }
    }
}
