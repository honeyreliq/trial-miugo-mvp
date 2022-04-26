using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.CareManagementPrograms.Queries
{
    public class GetCareManagementProgramsQuery : IRequest<CareManagementProgramsVm>
    {
    }

    public class GetCareManagementProgramsQueryHandler : IRequestHandler<GetCareManagementProgramsQuery, CareManagementProgramsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCareManagementProgramsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CareManagementProgramsVm> Handle(GetCareManagementProgramsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<CareManagementProgram> query = _context.CareManagementPrograms;

            var programs = await query.OrderBy(p => p.Name)
                .Select(p => _mapper.Map<CareManagementProgramDto>(p))
                .ToListAsync(cancellationToken);

            return new CareManagementProgramsVm { CareManagementPrograms = programs };
        }
    }
}
