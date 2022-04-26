using System.Threading.Tasks;

namespace IUGOCare.Application.Common.Interfaces
{
    public interface IMessageHandler
    {
        Task Handle(string eventName, string messageBody);
    }
}
