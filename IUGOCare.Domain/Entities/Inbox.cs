using System;
using IUGOCare.Domain.Common.Constants;

namespace IUGOCare.Domain.Entities
{
    public class Inbox
    {
        /// <summary>
        /// This Id is set by the sender's database.
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// The event name.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The JSON serialized DTO.
        /// </summary>
        public string MessageBody { get; set; }

        /// <summary>
        /// Any errors generated during message handling.
        /// </summary>
        public string Errors { get; private set; }

        /// <summary>
        /// Indicates whether the message was successfully "Processed" or if processing "Failed".
        /// See <see cref="InboxLiterals"/> for the constants used in this field.
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// The date this message was received.
        /// </summary>
        public DateTimeOffset Created { get; private set; }

        /// <summary>
        /// Sets the message's status based on any errors generated during processing.
        /// </summary>
        /// <param name="errors"></param>
        public void SetStatus(string errors)
        {
            if (string.IsNullOrEmpty(errors))
            {
                Errors = null;
                Status = InboxLiterals.Processed;
            }
            else
            {
                Errors = errors;
                Status = InboxLiterals.Failed;
            }
        }
    }
}
