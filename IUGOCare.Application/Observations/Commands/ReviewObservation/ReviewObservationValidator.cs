using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace IUGOCare.Application.Observations.Commands.ReviewObservation
{
    public class ReviewObservationValidator : AbstractValidator<ReviewObservationCommand>
    {
        public ReviewObservationValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.IsReviewedDate).NotNull().NotEmpty();
            RuleFor(r => r.ReviewedByName).NotNull().NotEmpty();
        }
    }
}
