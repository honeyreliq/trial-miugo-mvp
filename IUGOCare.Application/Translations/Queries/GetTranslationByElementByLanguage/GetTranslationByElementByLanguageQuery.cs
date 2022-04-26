using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Translations.Queries.GetTranslationByElementByLanguage
{
    public class GetTranslationByElementByLanguageQuery : IRequest<TranslationVm>
    {
        public string ElementName { get; set; }
        public string Language { get; set; }
    }

    public class GetTranslationByElementByLanguageQueryHandler : IRequestHandler<GetTranslationByElementByLanguageQuery, TranslationVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTranslationByElementByLanguageQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TranslationVm> Handle(GetTranslationByElementByLanguageQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Translation> translationsQuery = _context.Translations
                .Where(t => t.ElementName.Equals(request.ElementName) && t.Language.Equals(request.Language));

            var translation = await translationsQuery
                .ProjectTo<TranslationVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return translation;
        }
    }
}
