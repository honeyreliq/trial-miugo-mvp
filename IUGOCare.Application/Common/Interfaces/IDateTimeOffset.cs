using System;

namespace IUGOCare.Application.Common.Interfaces
{
    public interface IDateTimeOffset
    {
        DateTimeOffset UtcNow { get; }
        DateTimeOffset ConvertToLocal(DateTimeOffset originalDateTime, string timeZone);
    }
}
