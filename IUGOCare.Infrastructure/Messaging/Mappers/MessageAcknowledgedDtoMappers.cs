using IUGOCare.Application.Messages.Commands.SetMessageAcknowledged;
using IUGOCare.Messages.ClinicalToPatient.Common;

namespace IUGOCare.Infrastructure.Messaging.Mappers
{
    public static class MessageAcknowledgedDtoMappers
    {
        public static SetMessageAcknowledgedCommand MapToSetMessageAcknowledgedCommand(this MessageAcknowledgedDto dto)
        {
            return new SetMessageAcknowledgedCommand
            {
                MessageId = dto.MessageId
            };
        }
    }
}
