using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Translations.Queries.GetTranslationByElementByLanguage;
using IUGOCare.Domain.Entities;
using NUnit.Framework;
using static Testing;

namespace IUGOCare.Application.IntegrationTests.Translations.Queries
{
    public class GetTranslationByElementByLanguageTests : TestBase
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
        public async Task ShouldReturnTranslationViewModel()
        {
            await CreateTranslationElement();

            var query = new GetTranslationByElementByLanguageQuery
            {
                ElementName = "about",
                Language = "EN"
            };
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(TranslationVm), result.GetType());
            result.FileContent.Should().NotBeNull();
            result.FileContent.Should().Equals(_fileContent);
        }

        private async Task<Guid> CreateTranslationElement()
        {
            var id = Guid.NewGuid();

            await AddAsync(new Translation
            {
                Id = id,
                ElementName = "about",
                Language = "EN",
                FileContent = _fileContent
            });
            return id;
        }
    }
}
