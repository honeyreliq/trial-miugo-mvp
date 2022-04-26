using System;
using System.Linq;
using System.Threading.Tasks;
using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Logging;
using RestSharp;
using Auth0User = Auth0.ManagementApi.Models.User;

namespace IUGOCare.Infrastructure.Identity
{
    public class Auth0ManagementApiWrapper : IAuth0ManagementApiWrapper
    {
        private readonly ILogger<Auth0ManagementApiWrapper> _logger;
        private readonly string _auth0Domain;
        private readonly string _auth0Connection;
        private readonly string _auth0ManagementClientId;
        private readonly string _auth0ManagementSecret;
        private ManagementApiClient _managementApi;

        private ManagementApiClient ManagementApi
        {
            get
            {
                if (_managementApi is null)
                    _managementApi = GetManagementApi();
                return _managementApi;
            }
        }

        internal Auth0ManagementApiWrapper(ILogger<Auth0ManagementApiWrapper> logger, string auth0Domain, string auth0Connection, string auth0ManagementClientId, string auth0ManagementSecret)
        {
            _logger = logger;
            _auth0Domain = auth0Domain;
            _auth0Connection = auth0Connection;
            _auth0ManagementClientId = auth0ManagementClientId;
            _auth0ManagementSecret = auth0ManagementSecret;
        }

        public async Task<string> CreateUser(Guid id, string email, string password)
        {
            _logger.LogInformation($"Creating user {email} (Id: {id}) in Auth0.");
            var ucr = new UserCreateRequest
            {
                Connection = _auth0Connection,
                UserId = id.ToString(),
                VerifyEmail = true,
                Email = email,
                EmailVerified = true,
                Blocked = false,
                Password = password,
            };

            try
            {
                var user = await ManagementApi.Users.CreateAsync(ucr);
                _logger.LogInformation("User create successful!");
                return user.UserId;
            }
            catch (ErrorApiException eaex)
            {
                _logger.LogError("User create failed.", eaex);
                throw eaex;
            }
        }

        public async Task<Auth0User> GetUserByEmail(string email)
        {
            var users = await ManagementApi.Users.GetUsersByEmailAsync(email);
            return users.FirstOrDefault(u => u.Identities.Any(i => i.Connection == _auth0Connection));
        }

        public Task<Auth0User> GetUserById(string id)
        {
            _logger.LogInformation("AUTH0WRAP: GetUserById({0})", id);
            return ManagementApi.Users.GetAsync(id);
        }

        public async Task<bool> UpdatePassword(string userId, string password)
        {
            _logger.LogInformation($"Updating password for Auth0 user {userId}.");
            try
            {
                var uur = new UserUpdateRequest
                {
                    Password = password
                };
                var u = await ManagementApi.Users.UpdateAsync(userId, uur);
                _logger.LogInformation("Update successful.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Update failed.", ex);
                throw ex;
            }
        }

        public async Task<bool> UpdateUser(string userId, string email)
        {
            _logger.LogInformation($"Updating Auth0 user {email}.");
            try
            {
                var user = await GetUserByEmail(email);
                if (user != null)
                {
                    // User with this email already exists
                    return false;
                }

                var uur = new UserUpdateRequest
                {
                    Email = email
                };
                var u = ManagementApi.Users.UpdateAsync(userId, uur);
                _logger.LogInformation("Update successful.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Update failed.", ex);
                return false;
            }
        }

        public async Task<bool> ValidateUserAndPassword(string userId, string password)
        {
            var clientId = _auth0ManagementClientId;
            var clientSecret = _auth0ManagementSecret;
            var audience = $"https://{_auth0Domain}/api/v2/";

            var client = new RestClient($"https://{_auth0Domain}/oauth/token");
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=password&username={userId}&password={password}&audience={audience}&scope=openid profile email&client_id={clientId}&client_secret={clientSecret}", ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);
            return response.IsSuccessful;
        }

        private ManagementApiClient GetManagementApi()
        {
            _logger.LogInformation("AUTH0WRAP: GetManagementApi");
            var authToken = GetManagementAuthToken();
            _logger.LogInformation("AUTH0WRAP: Getting client for {0}.", _auth0Domain);
            return new ManagementApiClient(authToken, new Uri($"https://{_auth0Domain}/api/v2/"));
        }

        private string GetManagementAuthToken()
        {
            var clientId = _auth0ManagementClientId;
            var clientSecret = _auth0ManagementSecret;
            var audience = $"https://{_auth0Domain}/api/v2/";

            var client = new RestClient($"https://{_auth0Domain}/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}&audience={audience}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JsonObject json = SimpleJson.DeserializeObject<JsonObject>(response.Content);
            return json["access_token"].ToString();
        }
    }

    public interface IAuth0ManagementApiWrapper
    {
        Task<string> CreateUser(Guid id, string email, string password);
        Task<Auth0User> GetUserByEmail(string email);
        Task<Auth0User> GetUserById(string id);
        Task<bool> UpdatePassword(string userId, string password);
        Task<bool> UpdateUser(string userId, string email);
        Task<bool> ValidateUserAndPassword(string userId, string password);
    }
}
