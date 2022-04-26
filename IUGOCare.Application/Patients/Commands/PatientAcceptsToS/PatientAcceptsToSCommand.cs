using System;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;

namespace IUGOCare.Application.Patients.Commands.PatientAcceptsToS
{
    public class PatientAcceptsToSCommand : IRequest
    {
        public Guid PatientId { get; set; }
        public string Password { get; set; }
    }

    public class PatientAcceptsToSCommandHandler : IRequestHandler<PatientAcceptsToSCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public PatientAcceptsToSCommandHandler(IApplicationDbContext context, IMediator mediator, IIdentityService identityService)
        {
            _context = context;
            _mediator = mediator;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(PatientAcceptsToSCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients.FindAsync(request.PatientId);
            if (patient is null)
            {
                throw new NotFoundException(nameof(Patient), request.PatientId);
            }

            var auth0Id = await RegisterUserInAuth0(patient, request.Password, cancellationToken);

            patient.Auth0Id = auth0Id;
            patient.Active = true;
            await _context.SaveChangesAsync(cancellationToken);

            var patientActivated = new PatientActivated { PatientId = patient.Id };
            await _mediator.Publish(patientActivated, cancellationToken);

            return Unit.Value;
        }

        private async Task<string> RegisterUserInAuth0(Patient patient, string password, CancellationToken cancellationToken)
        {
            var auth0Id = "";
            var user = await _identityService.GetUserByEmailAsync(patient.EmailAddress);
            if (user is null)
            {
                auth0Id = await _identityService.CreateUserAsync(patient.Id, patient.EmailAddress, password);
            }
            else
            {
                await _identityService.UpdatePasswordAsync(user.UserId, password);
                auth0Id = user.UserId;
            }

            return auth0Id;
        }
    }
}
