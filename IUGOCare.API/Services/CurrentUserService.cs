using System.Security.Claims;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace IUGOCare.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId { get; }

        /// <summary>
        /// WARNING: Do not inject IApplicationDbContext into this class. I don't
        /// know why, but it causes the script generation (dotnet-ef migrations script)
        /// to hang forever.
        /// </summary>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
