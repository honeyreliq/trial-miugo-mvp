using System;
using System.Collections.Generic;
using System.Linq;
using IUGOCare.Audit.Models;
using IUGOCare.Audit.Repositories;
using log4net;

namespace IUGOCare.Audit.Services
{
    public class AuditService : IAuditService
    {
        private readonly IApiAuditRepository _apiAuditRepository;

        private static readonly ILog _log = LogManager.GetLogger(typeof(AuditService));

        private static readonly IList<string> _ignoredPaths = new List<string> {
            "/api/v1/version".ToLower(),
            "/api/v1/user/validatesession".ToLower()
        };

        private static readonly IList<string> _ignoredContentPaths = new List<string>
        {
            "ChangePassword".ToLower(),
            "PasswordReset".ToLower()
        };

        public AuditService(IApiAuditRepository apiAuditRepository)
        {
            //_environment = environment;
            _apiAuditRepository = apiAuditRepository;
        }

        public Guid AddRequestAudit(Guid? requestId, Uri uri, string method, string headers, string content,
            Guid userId, string userFirstName, string userLastName)
        {
            Guid id = Guid.Empty;
            if (_ignoredPaths.Contains(uri.AbsolutePath.ToLower()))
                return id;

            if (_ignoredContentPaths.Any(x => uri.AbsolutePath.ToLower().Contains(x)))
            {
                content = null;
            }

            try
            {
                ApiAudit apiAuditModel = new ApiAudit
                {
                    RequestId = requestId,
                    Uri = uri.ToString(),
                    Method = method,
                    Headers = headers,
                    Content = content,
                    RequestReliqUserId = userId,
                    CreateDate = DateTimeOffset.UtcNow,
                    FirstName = userFirstName,
                    LastName = userLastName,
                };
                var response = _apiAuditRepository.Append(apiAuditModel);
                id = response.Id;
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
            }
            return id;
        }

        public Guid AddResponseAudit(Guid? requestId, Uri uri, string method, string statusCode, string reasonPhrase, string headers, string content,
            Guid userId, string userFirstName, string userLastName)
        {
            Guid id = Guid.Empty;
            if (_ignoredPaths.Contains(uri.AbsolutePath.ToLower()))
                return id;

            try
            {
                ApiAudit apiAuditModel = new ApiAudit
                {
                    RequestId = requestId,
                    Uri = uri.ToString(),
                    Method = method,
                    StatusCode = statusCode,
                    ReasonPhrase = reasonPhrase,
                    Headers = headers,
                    Content = content,
                    RequestReliqUserId = userId,
                    CreateDate = DateTimeOffset.UtcNow,
                    FirstName = userFirstName,
                    LastName = userLastName,
                };
                var response = _apiAuditRepository.Append(apiAuditModel);
                id = apiAuditModel.Id;
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
            }
            return id;
        }
    }
}
