using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IUGOCare.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.TargetRanges.Queries
{
    public class GetPatientTargetRangesQuery : IRequest<PatientTargetRangesVm>
    {        
    }

    public class GetPatientTargetRangesQueryHandler : IRequestHandler<GetPatientTargetRangesQuery, PatientTargetRangesVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetPatientTargetRangesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PatientTargetRangesVm> Handle(GetPatientTargetRangesQuery request, CancellationToken cancellationToken)
        {
            var patientTargetRanges = await _context.ClinicPatients
                .Include(p => p.Patient)
                .Include(tr => tr.TargetRanges)
                .FirstOrDefaultAsync(p => p.Patient.Auth0Id == _currentUserService.UserId);

            if(patientTargetRanges == null)
            {
                return new PatientTargetRangesVm();
            }

            var targetRangesDtos = patientTargetRanges.TargetRanges.Select(tr => _mapper.Map<TargetRangeDto>(tr)).ToList();
            return new PatientTargetRangesVm { TargetRanges = targetRangesDtos };
        }
    }
}
