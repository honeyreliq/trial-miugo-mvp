using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Common.Interfaces
{
    public interface IMessagingDbContext : IDisposable
    {
        DbSet<Inbox> Inbox { get; set; }
        DbSet<Outbox> Outbox { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
