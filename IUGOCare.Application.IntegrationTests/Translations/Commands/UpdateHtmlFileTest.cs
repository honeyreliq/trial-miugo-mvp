using System;
using System.Threading.Tasks;
using FluentAssertions;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Translations.Commands.UpdateHtmlFileCommand;
using IUGOCare.Domain.Entities;
using NUnit.Framework;
using static Testing;

namespace IUGOCare.Application.IntegrationTests.Translations.Commands
{
    public class UpdateHtmlFileTest : TestBase
    {
        private byte[] _fileContent;

        [SetUp]
        public void SetUp()
        {
            var rnd = new Random();
            _fileContent = new byte[10];
            rnd.NextBytes(_fileContent);
        }
        
        [TestCase("E")]
        [TestCase("EM")]
        [TestCase("ES1")]
        public async Task ShouldThowNotFoundExceptionWithWrongElementName(string elementName)
        {
            await AddAsync(new Translation
            {
                Id = Guid.NewGuid(),
                ElementName = "about2",
                Language = "ES"
            });

            var command = new UpdateHtmlFileCommand
            {
                ElementName = elementName,
                Language = "ES",
                FileContent = _fileContent
            };

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<NotFoundException>();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task ShouldThowValidationExceptionWithWrongElementName(string elementName)
        {
            await AddAsync(new Translation
            {
                Id = Guid.NewGuid(),
                ElementName = "about2",
                Language = "ES"
            });

            var command = new UpdateHtmlFileCommand
            {
                ElementName = elementName,
                Language = "ES",
                FileContent = _fileContent
            };

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("E")]
        [TestCase("EM")]
        [TestCase("ES1")]
        public async Task ShouldNotUpdateWithWrongLanguage(string language)
        {
            await AddAsync(new Translation
            {
                Id = Guid.NewGuid(),
                ElementName = "about2",
                Language = "EN"
            });

            var command = new UpdateHtmlFileCommand
            {
                ElementName = "about2",
                Language = language,
                FileContent = _fileContent
            };

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldThowValidationExceptionWithWrongFileContent()
        {
            await AddAsync(new Translation
            {
                Id = Guid.NewGuid(),
                ElementName = "about2",
                Language = "EN"
            });

            var command = new UpdateHtmlFileCommand
            {
                ElementName = "about2",
                Language = "EN",
                FileContent = new byte[0]
            };

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldUpdateFile()
        {
            var id = Guid.NewGuid();
            string userId = RunAsDefaultUser();

            await AddAsync(new Translation
            {
                Id = id,
                ElementName = "about2",
                Language = "EN"
            });

            var command = new UpdateHtmlFileCommand
            {
                ElementName = "about2",
                Language = "EN",
                FileContent = _fileContent
            };

            await SendAsync(command);

            var up = await Testing.FindAsync<Translation>(id);

            up.FileContent.Should().NotBeNull();
            up.LastModifiedBy.Should().NotBeNull();
            up.LastModifiedBy.Should().Be(userId);
            up.LastModified.Should().NotBeNull();
            up.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}
