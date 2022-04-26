using MediatR;

namespace IUGOCare.Application.Common.Interfaces
{
    public interface ICommandFactory
    {
        IRequest CreateCommand(string eventName, string messageBody);
    }
}
