using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;

namespace IUGOCare.Application.Patients.Commands.DisableMarketingEmails
{
    public class DisableMarketingEmailsCommand : IRequest
    {
        public Guid PatientId { get; set; }
    }

    public class DisableMarketingEmailsCommandHandler : IRequestHandler<DisableMarketingEmailsCommand>
    {

        private readonly IApplicationDbContext _context;

        public DisableMarketingEmailsCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DisableMarketingEmailsCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients.FindAsync(request.PatientId);
            if (patient is null)
            {
                throw new NotFoundException(nameof(Patient), request.PatientId);
            }
            patient.AllowMarketingEmails = false;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }
}
