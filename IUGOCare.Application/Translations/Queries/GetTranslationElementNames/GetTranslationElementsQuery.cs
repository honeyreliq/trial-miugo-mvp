using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Translations.Queries.GetTranslationElementNames
{
    public class GetTranslationElementsQuery : IRequest<TranslationsVm>
    {
    }

    public class GetTranslationElementsQueryHandler : IRequestHandler<GetTranslationElementsQuery, TranslationsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTranslationElementsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TranslationsVm> Handle(GetTranslationElementsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Translation> translationsQuery = _context.Translations
                .Select(t => new Translation {
                    ElementName = t.ElementName
                })
                .OrderBy(t => t.ElementName)
                .Distinct();

            var translations = await translationsQuery.ToListAsync(cancellationToken);

            var translationsDtos = translations.Select(t => _mapper.Map<TranslationDto>(t)).ToList();

            return new TranslationsVm { Translations = translationsDtos };
        }
    }
}
