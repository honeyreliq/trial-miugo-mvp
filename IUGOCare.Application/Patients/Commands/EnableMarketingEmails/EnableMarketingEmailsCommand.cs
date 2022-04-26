using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;

namespace IUGOCare.Application.Patients.Commands.EnableMarketingEmails
{
    public class EnableMarketingEmailsCommand : IRequest
    {
        public Guid PatientId { get; set; }
    }

    public class EnableMarketingEmailsCommandHandler : IRequestHandler<EnableMarketingEmailsCommand>
    {

        private readonly IApplicationDbContext _context;

        public EnableMarketingEmailsCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EnableMarketingEmailsCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients.FindAsync(request.PatientId);
            if (patient is null)
            {
                throw new NotFoundException(nameof(Patient), request.PatientId);
            }
            patient.AllowMarketingEmails = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
