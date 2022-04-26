using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;

namespace IUGOCare.Application.Messages.Commands.SetMessageAcknowledged
{
    public class SetMessageAcknowledgedCommand : IRequest
    {
        public Guid MessageId { get; set; }
        public string Errors { get; set; }
    }

    public class SetMessageAcknowledgedCommandHandler : IRequestHandler<SetMessageAcknowledgedCommand, Unit>
    {
        private readonly IMessagingDbContext _dbContext;

        public SetMessageAcknowledgedCommandHandler(IMessagingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(SetMessageAcknowledgedCommand command, CancellationToken cancellationToken)
        {
            var message = await _dbContext.Outbox.FindAsync(command.MessageId);

            if (message is null)
                throw new ValidationException(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure(nameof(Outbox), $"Error in SetMessageAcknowledgedCommandHandler: Message not found in Outbox with MessageId {command.MessageId}")
                    });

            message.MessageAcknowledged = true;
            message.Errors = command.Errors;

            _dbContext.Outbox.Update(message);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
