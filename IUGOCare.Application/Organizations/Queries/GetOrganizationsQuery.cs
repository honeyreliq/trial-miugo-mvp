using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Organizations.Queries
{
    public class GetOrganizationsQuery : IRequest<OrganizationsVm>
    {
    }

    public class GetOrganizationsQueryHandler : IRequestHandler<GetOrganizationsQuery, OrganizationsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetOrganizationsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OrganizationsVm> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Organization> query = _context.Organizations;

            var organizations = await query.OrderBy(p => p.Name)
                .Select(p => _mapper.Map<OrganizationDto>(p))
                .ToListAsync(cancellationToken);

            return new OrganizationsVm { Organizations = organizations };
        }
    }
}
