using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Translations.Queries.GetTranslationElementNames
{
    public class TranslationDto : IMapFrom<Translation>
    {
        public string ElementName { get; set; }
    }
}
