using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Queries.ShouldStartTour
{
    public class ShouldStartTourQuery : IRequest<bool>
    {
        public string TourKey { get; set; }
    }

    public class ShouldStartTourQueryHandler : IRequestHandler<ShouldStartTourQuery, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ShouldStartTourQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<bool> Handle(ShouldStartTourQuery request, CancellationToken cancellationToken)
        {
            var patientId = await _identityService.GetCurrentPatientId();
            var count = await _context.PatientTours.CountAsync(pt => pt.PatientId == patientId && pt.TourKey == request.TourKey);
            return count == 0;
        }
    }
}
