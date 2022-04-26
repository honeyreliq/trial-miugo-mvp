using IUGOCare.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IUGOCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : BaseController
    {
        /// <summary>
        /// The auth0 configuration
        /// </summary>
        private readonly Auth0Setting auth0Config;
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public AuthenticationController(Auth0Setting config)
        {
            auth0Config = config;
        }

        /// <summary>
        /// Gets the auth0 authentication configuration.
        /// </summary>
        /// <returns>The configuration to authenticate</returns>
        [HttpGet("settings")]
        [AllowAnonymous]
        public IActionResult GetAuth0AuthenticationSettings()
        {
            return Ok(auth0Config);
        }
    }
}