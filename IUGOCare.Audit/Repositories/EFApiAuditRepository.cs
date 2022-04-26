using IUGOCare.Audit.Models;

namespace IUGOCare.Audit.Repositories
{
    public class EFApiAuditRepository : IApiAuditFailoverRepository
    {
        private readonly IAuditDbContextFactory _contextFactory;

        public EFApiAuditRepository(IAuditDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public ApiAudit Append(ApiAudit apiAudit)
        {
            using (var auditDbContext = _contextFactory.AuditDbContext)
            {
                auditDbContext.ApiAudit.Add(apiAudit);
                auditDbContext.SaveChanges();
                return apiAudit;
            }
        }
    }
}
