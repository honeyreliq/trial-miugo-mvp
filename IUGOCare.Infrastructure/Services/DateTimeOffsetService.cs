using IUGOCare.Application.Common.Interfaces;
using NodaTime;
using NodaTime.Extensions;
using System;

namespace IUGOCare.Infrastructure.Services
{
    public class DateTimeOffsetService : IDateTimeOffset
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        public DateTimeOffset ConvertToLocal(DateTimeOffset originalDateTime, string timeZone)
        {
            DateTimeZone nodaTimeZone = DateTimeZoneProviders.Tzdb[timeZone];
            return originalDateTime.ToInstant().InZone(nodaTimeZone).ToDateTimeOffset();
        }
    }
}
