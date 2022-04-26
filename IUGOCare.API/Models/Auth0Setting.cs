using System.Text.Json.Serialization;

namespace IUGOCare.API.Models
{
    public sealed class Auth0Setting
    {
        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public string Domain { get; set; }
        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        /// <value>
        /// The audience.
        /// </value>
        public string Audience { get; set; }
        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Auth0Setting"/> class.
        /// </summary>
        public Auth0Setting()
        {
        }
    }
}
