using System;

namespace IUGOCare.Audit.Infrastructure
{
    public interface IAzureBlobNameGenerator
    {
        string Generate(string client, DateTime dateTime);
        int IncrementFileNumber();
    }
}
