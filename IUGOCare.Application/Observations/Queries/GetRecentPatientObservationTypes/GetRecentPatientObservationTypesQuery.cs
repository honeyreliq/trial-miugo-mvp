using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Observations.Queries.GetRecentPatientObservationTypes
{
    public class GetRecentPatientObservationTypesQuery : IRequest<RecentPatientObservationTypesVm>
    {
        
    }
    public class GetRecentPatientObservationTypesQueryHandler : IRequestHandler<GetRecentPatientObservationTypesQuery, RecentPatientObservationTypesVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetRecentPatientObservationTypesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<RecentPatientObservationTypesVm> Handle(GetRecentPatientObservationTypesQuery request, CancellationToken cancellationToken)
        {
            var clinicPatientIds = GetClinicPatientIds();

            var patientObservationsQuery = _context.Observations
                .Where(o => clinicPatientIds.Contains(o.ClinicPatientId))
                .GroupBy(o => o.ObservationCode)
                .Select(o => new {
                    ObservationCode = o.Key,
                    CreatedDate = o.Max(b => b.Created)
                    }
                ).OrderByDescending(n => n.CreatedDate)
                .Take(3);

            var observations = await patientObservationsQuery
                    .ToListAsync(cancellationToken);

            return new RecentPatientObservationTypesVm { ObservationTypes = observations
                .Select(o => new ObservationTypeDto {
                    ObservationType = o.ObservationCode,
                    ObservationOrder = observations.IndexOf(o)
                })
                .ToList() };
        }

        private List<Guid> GetClinicPatientIds()
        {
            var patient = _context.Patients
                    .Include(p => p.Clinics)
                    .FirstOrDefault(p => p.Auth0Id == _currentUserService.UserId);
            return patient?.Clinics?.Select(cp => cp.ClinicPatientId).ToList() ?? new List<Guid>();
        }
    }
}
