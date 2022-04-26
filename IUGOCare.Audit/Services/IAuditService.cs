using System;

namespace IUGOCare.Audit.Services
{
    public interface IAuditService
    {
        Guid AddRequestAudit(Guid? requestId, Uri uri, string method, string headers, string content,
            Guid userId, string userFirstName, string userLastName);
        Guid AddResponseAudit(Guid? requestId, Uri uri, string method, string statusCode, string reasonPhrase, string headers, string content,
            Guid userId, string userFirstName, string userLastName);
    }
}
