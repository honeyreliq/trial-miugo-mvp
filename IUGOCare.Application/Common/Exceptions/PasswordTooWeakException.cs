using System;
using System.Collections.Generic;
using System.Text;

namespace IUGOCare.Application.Common.Exceptions
{
    public class PasswordTooWeakException : Exception
    {
        public PasswordTooWeakException()
            : base("Password too weak.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public PasswordTooWeakException(string message)
            :base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public PasswordTooWeakException(string message, IDictionary<string, string[]> errors)
            : base(message)
        {
            Errors = errors;
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
