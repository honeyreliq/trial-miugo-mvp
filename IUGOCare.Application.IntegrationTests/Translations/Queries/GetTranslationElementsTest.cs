using System;
using System.Threading.Tasks;
using IUGOCare.Application.Translations.Commands.UpdateHtmlFileCommand;
using IUGOCare.Application.Translations.Queries.GetTranslationElementNames;
using IUGOCare.Domain.Entities;
using NUnit.Framework;
using static Testing;

namespace IUGOCare.Application.IntegrationTests.Translations.Queries
{
    public class GetTranslationElementsTest : TestBase
    {
        private byte[] _fileContent;

        [SetUp]
        public void SetUp()
        {
            var rnd = new Random();
            _fileContent = new byte[10];
            rnd.NextBytes(_fileContent);
        }

        [Test]
        public async Task ShouldReturnTranslationElements()
        {
            var id = Guid.NewGuid();

            await AddAsync(new Translation
            {
                Id = id,
                ElementName = "about",
                Language = "EN"
            });

            var command = new UpdateHtmlFileCommand
            {
                ElementName = "about",
                Language = "EN",
                FileContent = _fileContent
            };

            await SendAsync(command);

            var query = new GetTranslationElementsQuery();
            var result = await Testing.SendAsync(query);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Translations.Count);
        }
    }
}
