using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IUGOCare.Application.Common.Behaviours;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Observations.Commands.UpdateObservation
{
    public class UpdateObservationValidator : AbstractValidator<UpdateObservationCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateObservationValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(o => o.ObservationId).NotEmpty()
            .MustAsync(IdRegistered).WithMessage("The observation id is not registered.");
            RuleFor(o => o.EffectiveDate).LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("Date cannot be in the future.");
            RuleFor(o => o).Must(attributes => SleepValidator.SleepValidationTimesMustNotSurpass24Hours(attributes.ObservationCode, attributes.ObservationDataList))
                .WithMessage("The sum of the sleep times (excluding total) must not surpass 24 hours");
            RuleFor(o => o).Must(attributes => SleepValidator.SleepValidationTimesMustMatch(attributes.ObservationCode, attributes.ObservationDataList))
                .WithMessage("The sum of the sleep times categories must match the total time");

        }

        public async Task<bool> IdRegistered(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Observations.AnyAsync(p => p.Id.Equals(id), cancellationToken);
        }
    }
}
