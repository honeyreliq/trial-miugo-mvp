using System;
using IUGOCare.Domain.Common.Constants;
using IUGOCare.Domain.Entities;
using NUnit.Framework;

namespace IUGOCare.Domain.UnitTests.Entities
{
    public class PatientTests
    {
        [Test]
        [TestCase("es")]    // Spanish
        [TestCase("ES")]
        [TestCase("Es")]
        [TestCase("en")]    // English
        [TestCase("EN")]
        [TestCase("En")]
        public void SetLanguage_SupportedLanguages_ShouldSetLanguage(string language)
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.SetLanguage(language);

            // Assert
            Assert.That(patient.PatientLanguage.Equals(language.ToUpper()));
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void SetLanguage_NullOrEmpty_ShouldSetLanguageToEnglish(string language)
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.SetLanguage(language);

            // Assert
            Assert.That(patient.PatientLanguage.Equals(Languages.EnglishLanguage));
        }

        [Test]
        [TestCase("ru")]    // Russian
        [TestCase("KO")]    // Korean
        [TestCase("fr")]    // French
        [TestCase("ZH")]    // Chinese
        public void SetLanguage_UnsupportedLanguages_ShouldSetLanguageToEnglish(string language)
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.SetLanguage(language);

            // Assert
            Assert.That(patient.PatientLanguage.Equals(Languages.EnglishLanguage));
        }
    }
}
