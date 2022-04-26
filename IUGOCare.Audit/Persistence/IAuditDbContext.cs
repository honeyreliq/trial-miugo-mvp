using System;
using IUGOCare.Audit.Models;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Audit.Persistence
{
    public interface IAuditDbContext : IDisposable
    {
        DbSet<ApiAudit> ApiAudit { get; set; }
        public int SaveChanges();
    }
}
