using System;
using System.Collections.Generic;

namespace IUGOCare.Application.Common.Exceptions
{
    public class PatientRegisteredException : Exception
    {
        public PatientRegisteredException()
            : base($"This patient is already registered.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
