using System;
using System.Threading.Tasks;
using Auth0User = Auth0.ManagementApi.Models.User;

namespace IUGOCare.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Guid> GetCurrentPatientId();
        Task<Auth0User> GetUserAsync(string auth0Id);
        Task<Auth0User> GetUserByEmailAsync(string email);
        Task<string> GetUserNameAsync(string auth0Id);
        Task<string> CreateUserAsync(Guid id, string email, string password);
        Task<bool> UpdatePasswordAsync(string auth0Id, string password);
        Task<bool> UpdateUserAsync(string auth0Id, string email);
        Task<bool> ValidateUserAndPassword(string userId, string password);
    }
}
