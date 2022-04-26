using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Models;
using IUGOCare.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IUGOCare.Application.IntegrationTests.Services
{
    public class SendEmailServiceTest
    {
        private Mock<ILogger<SendEmailService>> _mockLogger;

        private SendEmailService _sendEmailService;

        private EmailSendConfig _emailSendConfig;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<SendEmailService>>();

            _emailSendConfig = new EmailSendConfig
            {
                Subject = "Testing email service",
                ToEmail = "jflorez@reliqhealth.com",
                BodyPlainText = "Testing email service"
            };
        }

        [Test]
        public void SendEmailTest()
        {
            string apiKey = Testing.Configuration.GetValue<string>("SendGridApiKey");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Assert.Ignore("The SendGridApiKey environment variable must be set.");
            }

            var sendGridClient = new SendGridClient(apiKey);
            _sendEmailService = new SendEmailService(_mockLogger.Object, sendGridClient, Testing.Configuration);
            Assert.DoesNotThrowAsync(() => _sendEmailService.SendEmail(this._emailSendConfig));
        }

        [Test]
        public void SendEmailTestWithBadRequestResponse()
        {
            var response = new Response(HttpStatusCode.BadRequest, null, null);
            var sendGridClient =  new Mock<ISendGridClient>();
            sendGridClient.Setup(i => i.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>())).
                Returns(Task.FromResult(response));

            _sendEmailService = new SendEmailService(_mockLogger.Object, sendGridClient.Object, Testing.Configuration);
            Assert.DoesNotThrowAsync(() => _sendEmailService.SendEmail(this._emailSendConfig));
        }
    }
}
