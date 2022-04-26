using System;
using System.Linq;
using System.Threading.Tasks;
using IUGOCare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Auth0User = Auth0.ManagementApi.Models.User;

namespace IUGOCare.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IAuth0ManagementApiWrapper _auth0Management;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _context;

        public IdentityService(
            IAuth0ManagementApiWrapper auth0Management,
            IApplicationDbContext context,
            ICurrentUserService currentUserService
        )
        {
            _auth0Management = auth0Management;
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task<Guid> GetCurrentPatientId()
        {
            var auth0Id = _currentUserService.UserId;
            var patientId = await _context.Patients
               .Where(p => p.Auth0Id == auth0Id)
               .Select(p => p.Id)
               .FirstAsync();
            return patientId;
        }

        public async Task<Auth0User> GetUserAsync(string auth0Id)
        {
            return await _auth0Management.GetUserById(auth0Id);
        }

        public async Task<Auth0User> GetUserByEmailAsync(string userId)
        {
            return await _auth0Management.GetUserByEmail(userId);
        }

        public async Task<string> GetUserNameAsync(string auth0Id)
        {
            var user = await _auth0Management.GetUserById(auth0Id);
            return user.UserName ?? user.Email;
        }

        public async Task<string> CreateUserAsync(Guid patientId, string email, string password)
        {
            return await _auth0Management.CreateUser(patientId, email, password);
        }

        public async Task<bool> UpdatePasswordAsync(string auth0Id, string password)
        {
            return await _auth0Management.UpdatePassword(auth0Id, password);
        }

        /// <summary>
        /// For now, we're only allowing the update of emails, but can expand as needed
        /// </summary>
        public Task<bool> UpdateUserAsync(string auth0Id, string email)
        {
            return _auth0Management.UpdateUser(auth0Id, email);
        }

        public Task<bool> ValidateUserAndPassword(string userId, string password)
        {
            return _auth0Management.ValidateUserAndPassword(userId, password);
        }
    }
}
