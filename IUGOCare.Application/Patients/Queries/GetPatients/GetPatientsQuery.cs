using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Patients.Queries.GetPatients
{
    public enum PatientQueryFilter
    {
        AllPatients, ActiveOnly, InactiveOnly
    }

    public class GetPatientsQuery : IRequest<PatientsVm>
    {
        public PatientQueryFilter Filter { get; set; }
    }

    public class GetPatientsQueryHandler : IRequestHandler<GetPatientsQuery, PatientsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPatientsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PatientsVm> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Patient> patientsQuery = _context.Patients;

            if (request.Filter == PatientQueryFilter.ActiveOnly)
            {
                patientsQuery = patientsQuery.Where(p => p.Active == true);
            }
            else if (request.Filter == PatientQueryFilter.InactiveOnly)
            {
                patientsQuery = patientsQuery.Where(p => p.Active == false);
            }

            var patients = await patientsQuery
                    .Include(ac => ac.Activation)
                    .Include(c => c.Clinics)
                    .OrderBy(p => p.EmailAddress)
                    .ToListAsync(cancellationToken);

            var patientDtos = patients.Select(p => _mapper.Map<PatientDto>(p)).ToList();
            return new PatientsVm { Patients = patientDtos };
        }
    }
}
