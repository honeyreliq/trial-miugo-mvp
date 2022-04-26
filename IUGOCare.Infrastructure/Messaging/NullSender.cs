using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;

namespace IUGOCare.Infrastructure.Messaging
{
    public class NullSender : IServiceBusSender
    {
        public Task SendMessageAsync(string destinationSubdomain, string eventName, object messageBody, bool expectsAcknowledgement = true) => Task.CompletedTask;

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
