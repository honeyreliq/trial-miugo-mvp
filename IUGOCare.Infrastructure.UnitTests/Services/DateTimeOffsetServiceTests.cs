using System;
using IUGOCare.Infrastructure.Services;
using NUnit.Framework;

namespace IUGOCare.Infrastructure.UnitTests.Services
{
    public class DateTimeOffsetServiceTests
    {
        DateTimeOffsetService _dateTimeOffsetService = new DateTimeOffsetService();

        [Test]
        public void ShouldConvertToLocal()
        {
            var expectedDateTime = new DateTimeOffset(2020, 6, 20, 12, 0, 0, new TimeSpan(-5, 0, 0));
            var timeZone = "America/Chicago";
            var originalDateTime = new DateTimeOffset(2020, 6, 20, 17, 0, 0, new TimeSpan(0, 0, 0));

            var actualDateTime = _dateTimeOffsetService.ConvertToLocal(originalDateTime, timeZone);

            Assert.IsNotNull(actualDateTime);
            Assert.AreEqual(expectedDateTime, actualDateTime);
        }
    }
}
