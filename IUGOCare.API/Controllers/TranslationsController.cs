using System.IO;
using System.Threading.Tasks;
using IUGOCare.Application.Translations.Commands.UpdateHtmlFileCommand;
using IUGOCare.Application.Translations.Queries.GetTranslationByElementByLanguage;
using IUGOCare.Application.Translations.Queries.GetTranslationElementNames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IUGOCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationsController : BaseController
    {
        [HttpPut("updatefile/{elementName}/{language}")]
        [Authorize]
        public async Task<IActionResult> UpdateTranslationFile(string elementName, string language, IFormFile file)
        {
            var command = new UpdateHtmlFileCommand();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                command.ElementName = elementName;
                command.Language = language;
                command.FileContent = stream.ToArray();
            }
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpGet]
        [Route("elements")]
        [Authorize]
        public async Task<ActionResult<TranslationsVm>> GetTranslationElements()
        {
            return await Mediator.Send(new GetTranslationElementsQuery());
        }

        [HttpGet]
        [Route("{elementName}/{language}")]
        [Authorize]
        public async Task<ActionResult<TranslationVm>> GetTranslationByElementByLanguage(string elementName, string language)
        {
            return await Mediator.Send(new GetTranslationByElementByLanguageQuery
            {
                ElementName = elementName,
                Language = language
            });
        }
    }
}
