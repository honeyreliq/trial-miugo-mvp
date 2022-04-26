using IUGOCare.Audit.Models;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Audit.Persistence
{
    public class AuditDbContext : DbContext, IAuditDbContext
    {
        public DbSet<ApiAudit> ApiAudit { get; set; }

        public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options){}
    }
}
