using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Queries.GetEmailToken
{
    public class GetEmailToken : IRequest<EmailTokenVm>
    {
        public string Token { get; set; }
    }

    public class GetEmailTokenHandler : IRequestHandler<GetEmailToken, EmailTokenVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetEmailTokenHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<EmailTokenVm> Handle(GetEmailToken request, CancellationToken cancellationToken)
        {
            var currentPatientId = await _identityService.GetCurrentPatientId();

            var viewModel = await _context.Patients
                .Include(p => p.UpdateEmailRequest)
                .Include(c => c.Clinics)
                .Select(p => new EmailTokenVm {
                    Token = p.UpdateEmailRequest.Token,
                    TokenExpiration = p.UpdateEmailRequest.ExpirationDate,
                    NewEmailAddress = p.UpdateEmailRequest.EmailAddress,
                    PatientId = p.Id,
                    GivenName = p.PrimaryClinicPatient.GivenName,
                    FamilyName = p.PrimaryClinicPatient.FamilyName,
                })
                .FirstOrDefaultAsync(p => p.Token == request.Token && p.PatientId == currentPatientId);

            if (viewModel is null)
            {
                throw new NotFoundException("UpdateEmailRequest", request.Token);
            }

            if (viewModel.TokenExpiration < DateTimeOffset.UtcNow)
            {
                throw new TokenExpiredException();
            }

            return viewModel;
        }
    }
}
