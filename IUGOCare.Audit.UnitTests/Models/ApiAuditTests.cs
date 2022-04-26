using System;
using NUnit.Framework;
using IUGOCare.Audit.Models;

namespace IUGOCare.Audit.UnitTests.Models
{
    [TestFixture]
    public class ApiAuditTests
    {
        [Test]
        public void ToDelimitedString_ReturnsExpectedResult()
        {
            var delimiter = "|";
            var model = new ApiAudit
            {
                Id = Guid.NewGuid(),
                RequestId = Guid.NewGuid(),
                Uri = "someUri",
                Method = "some method",
                StatusCode = "some status code",
                ReasonPhrase = "some reason phrase",
                Headers = "some headers",
                Content = "some content",
                RequestReliqUserId = Guid.NewGuid(),
                CreateDate = DateTimeOffset.MinValue,
                FirstName = "some first name",
                LastName = "some last name"
            };

            var expectedResult = string.Join(delimiter,
                model.Id,
                model.Uri,
                model.RequestId,
                model.Method,
                model.StatusCode,
                model.ReasonPhrase,
                model.Headers,
                model.Content,
                model.RequestReliqUserId,
                model.CreateDate.ToString("yyyy-MM-dd HH:mm:ss.fffffff K"),
                model.FirstName,
                model.LastName) + Environment.NewLine;

            var result = model.ToDelimitedString(delimiter);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
