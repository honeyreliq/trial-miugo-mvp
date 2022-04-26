using System;
using System.Collections.Generic;

namespace IUGOCare.Application.Common.Exceptions
{
    public class EmailRegisteredException: Exception
    {
        public EmailRegisteredException()
            : base($"Email address is already registered.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
