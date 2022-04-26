using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace IUGOCare.Application.Common.Interfaces
{
    public interface IServiceBusSender : IHostedService
    {
        Task SendMessageAsync(string destinationSubdomain, string eventName, object messageBody, bool expectsAcknowledgement = true);
    }
}
