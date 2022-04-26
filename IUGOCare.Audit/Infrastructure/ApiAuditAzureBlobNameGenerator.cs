using System;

namespace IUGOCare.Audit.Infrastructure
{
    public class ApiAuditAzureBlobNameGenerator : IAzureBlobNameGenerator
    {
        private string _dateString = "";
        private int _fileNumber = 1;

        public string Generate(string client, DateTime dateTime)
        {
            var currentDateString = $"{dateTime.ToString("yyyy.MM.dd.HH")}00";
            if (_dateString != currentDateString) // create a new blob every hour
            {
                _dateString = currentDateString;
                _fileNumber = 1;
            }

            string paddedFileNumber = _fileNumber.ToString("000000");

            return $"{client}.{_dateString}.{paddedFileNumber}.csv";
        }

        public int IncrementFileNumber()
        {
            _fileNumber++;
            return _fileNumber;
        }
    }
}
