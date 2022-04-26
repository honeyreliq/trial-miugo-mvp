using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Providers.Queries
{
    public class GetProvidersQuery : IRequest<ProvidersVm>
    {
    }

    public class GetProvidersQueriesHandler : IRequestHandler<GetProvidersQuery, ProvidersVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProvidersQueriesHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProvidersVm> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Provider> query = _context.Providers;

            var providers = await query.OrderBy(p => p.Name)
                .Select(p => _mapper.Map<ProviderDto>(p))
                .ToListAsync(cancellationToken);

            return new ProvidersVm { Providers = providers };
        }
    }
}
