using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using MediatR;

namespace IUGOCare.Infrastructure.Messaging
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IMediator _mediator;
        private readonly ICommandFactory _commandFactory;

        public MessageHandler(IMediator mediator, ICommandFactory commandFactory)
        {
            _mediator = mediator;
            _commandFactory = commandFactory;
        }

        public async Task Handle(string eventName, string messageBody)
        {
            IRequest command = _commandFactory.CreateCommand(eventName, messageBody);

            if (command is null)
                throw new NotFoundException($"MessageHandler could not find command for {eventName}");

            await _mediator.Send(command);
        }
    }
}
