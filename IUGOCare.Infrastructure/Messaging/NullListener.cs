using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;

namespace IUGOCare.Infrastructure.Messaging
{
    public class NullListener : IServiceBusListener
    {
        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
