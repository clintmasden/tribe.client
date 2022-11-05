namespace Tribe.Tests.Models
{
    public class TribeClientConfiguration
    {
        /// <summary>
        /// Community URL
        /// </summary>
        public string NetworkDomain { get; set; }

        /// <summary>
        /// Network access token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The username or email for SetAccessToken
        /// </summary>
        public string UsernameOrEmail { get; set; }

        /// <summary>
        /// The password for SetAccessToken
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The SSO for OpenWebBrowserForSingleSignOnWithJsonWebToken
        /// </summary>
        public string SingleSignOnUrl { get; set; }

        /// <summary>
        /// The JWT secret key for GetSingleSignOnJsonWebToken
        /// </summary>
        public string JwtSecretKey { get; set; }
    }
}