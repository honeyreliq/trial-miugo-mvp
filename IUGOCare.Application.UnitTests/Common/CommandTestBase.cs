using System;
using IUGOCare.Infrastructure.Persistence;

namespace IUGOCare.Application.UnitTests.Common
{
    public abstract class CommandTestBase : IDisposable
    {
        protected readonly ApplicationDbContext Context;

        public CommandTestBase()
        {
            Context = ApplicationContextFactory.Create();
        }

        public void Dispose()
        {
            ApplicationContextFactory.Destroy(Context);
        }
    }
}
