using IUGOCare.Application.Common.Mappings;
using IUGOCare.Domain.Entities;

namespace IUGOCare.Application.Translations.Queries.GetTranslationByElementByLanguage
{
    public class TranslationVm : IMapFrom<Translation>
    {
        public string ElementName { get; set; }
        public string Language { get; set; }
        public byte[] FileContent { get; set; }
    }
}
