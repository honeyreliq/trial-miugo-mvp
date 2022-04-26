using System.Collections.Generic;

namespace IUGOCare.Application.Common.Models
{
    public class EmailSendConfig
    {
        public EmailSendConfig()
        {
            EmailLines = new List<string>();
        }

        public string Subject { get; set; }

        public string ToEmail { get; set; }

        public string Url { get; set; }

        public string BodyPlainText { get; set; }

        public string BodyHtmlText { get; set; }

        public bool IncludeButton { get; set; }

        public string ButtonText { get; set; }

        public string EmailGreeting { get; set; }

        public IList<string> EmailLines { get; set; }
    }
}
