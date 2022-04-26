using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Queries.GetProfileInformation
{
    public class GetProfileInformationQuery : IRequest<PatientProfileVm>
    {
    }

    public class GetProfileInformationQueryHandler : IRequestHandler<GetProfileInformationQuery, PatientProfileVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetProfileInformationQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<PatientProfileVm> Handle(GetProfileInformationQuery request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .Include(p => p.Clinics)
                .FirstOrDefaultAsync(p => p.Auth0Id == _currentUserService.UserId);
            if (patient is null)
            {
                throw new NotFoundException(nameof(Patient), _currentUserService.UserId);
            }

            var clinicPatientId = patient.PrimaryClinicPatient.ClinicPatientId;

            var profile = await _context.ClinicPatients
                .Include(c => c.EmergencyContact)
                .Include(c => c.PrimaryCareProvider)
                .ThenInclude(c => c.Organization)
                .Where(cp => cp.ClinicPatientId == clinicPatientId)
                .ProjectTo<PatientProfileVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            var carePrograms = _context.PatientCareManagementPrograms
                .Where(p => p.ClinicPatientId == clinicPatientId)
                .Include(b => b.BillingProvider)
                .ThenInclude(p => p.Organization)
                .Include(p => p.CareManagementProgram)
                .Select(p => new
                {
                    p.BillingProviderId,
                    p.CareManagementProgram
                })
                .ToList()
                .GroupBy(bp => bp.BillingProviderId)
                .Select(bp => new PatientCareManagementProgramDto
                {
                    CareManagementPrograms = bp.Select(cp => _mapper.Map<CareManagementProgramDto>(cp.CareManagementProgram)).ToList()
                })
                .ToList();

            profile.PatientPrograms = carePrograms ?? new List<PatientCareManagementProgramDto>();

            return profile;
        }
    }
}
