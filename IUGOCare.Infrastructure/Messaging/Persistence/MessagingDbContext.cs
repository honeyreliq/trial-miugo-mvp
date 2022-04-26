using System.Reflection;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Infrastructure.Messaging.Persistence
{
    public class MessagingDbContext : DbContext, IMessagingDbContext
    {
        public DbSet<Inbox> Inbox { get; set; }
        public DbSet<Outbox> Outbox { get; set; }

        public MessagingDbContext(DbContextOptions<MessagingDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
