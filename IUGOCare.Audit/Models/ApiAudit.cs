using System;

namespace IUGOCare.Audit.Models
{
    public class ApiAudit
    {
        public Guid Id { get; set; }

        public Guid? RequestId { get; set; }

        public string Uri { get; set; }

        public string Method { get; set; }

        public string StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public string Headers { get; set; }

        public string Content { get; set; }

        public Guid? RequestReliqUserId { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ToDelimitedString(string delimiter)
        {
            var delimitedString = string.Join(delimiter,
                Id,
                Uri,
                RequestId,
                Method,
                StatusCode,
                ReasonPhrase,
                Headers,
                Content,
                RequestReliqUserId,
                CreateDate.ToString("yyyy-MM-dd HH:mm:ss.fffffff K"),
                FirstName,
                LastName);

            return $"{delimitedString}{Environment.NewLine}";
        }
    }
    }
