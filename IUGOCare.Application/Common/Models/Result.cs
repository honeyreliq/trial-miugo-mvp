using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace IUGOCare.Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public static Result Success() => new Result(true, new string[] { });

        public static Result Failure(IEnumerable<string> errors) => new Result(false, errors);

        public static Result Failure(ValidationException vex) => new Result(false, vex.Errors.Select(e => e.ErrorMessage));
    }
}
