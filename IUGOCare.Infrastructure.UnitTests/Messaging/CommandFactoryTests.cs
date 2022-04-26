using System;
using System.Text.Json;
using IUGOCare.Application.Patients.Commands.RegisterPatient;
using IUGOCare.Infrastructure.Messaging;
using IUGOCare.Messages.ClinicalToPatient.Patients;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Messaging
{
    public class CommandFactoryTests
    {
        [Test]
        public void CreateCommand_ShouldReturnExpectedCommand()
        {
            // Arrange
            var commandFactory = new CommandFactory();

            var eventName = "PatientCreated";
            var clinicId = Guid.NewGuid();
            var patientId = Guid.NewGuid();

            var externalEvent = new PatientCreatedDto
            {
                ClinicId = clinicId,
                PatientId = patientId,
                ClinicSubdomain = "someSubdomain",
                EmailAddress = "patient@example.com",
                GivenName = "somePatientName",
                PatientLanguage = "en"
            };

            var messageBody = JsonSerializer.Serialize(externalEvent);

            // Act
            var actualCommand = commandFactory.CreateCommand(eventName, messageBody);

            // Assert
            var registerPatientCommand = (RegisterPatientCommand)actualCommand;
            Assert.IsNotNull(actualCommand);
            Assert.AreEqual(actualCommand.GetType(), typeof(RegisterPatientCommand));
            Assert.AreEqual(registerPatientCommand.ClinicId, externalEvent.ClinicId);
            Assert.AreEqual(registerPatientCommand.ClinicPatientId, externalEvent.PatientId);
            Assert.AreEqual(registerPatientCommand.ClinicSubdomain, externalEvent.ClinicSubdomain);
            Assert.AreEqual(registerPatientCommand.EmailAddress, externalEvent.EmailAddress);
            Assert.AreEqual(registerPatientCommand.PatientLanguage, externalEvent.PatientLanguage);
            Assert.AreEqual(registerPatientCommand.GivenName, externalEvent.GivenName);
        }
    }
}
