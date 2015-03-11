using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.DataTransferObjects
{
    /// <summary>
    /// Defines a DTO that represents a given client ID and client secret.
    /// </summary>
    public class ClientAuthorizationDto
    {
        /// <summary>
        /// Gets or sets the client ID that should be used for client authorization.
        /// </summary>
        /// <returns></returns>
        public string ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client secret/password that should be used for client authorization.
        /// </summary>
        /// <returns></returns>
        public string ClientSecret
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new <see cref="ClientAuthorizationDto"/> from the given HTTP Basic Authorization scheme string.
        /// </summary>
        /// <param name="auth">The string that represents the base-64 encoded authorization header parameter.</param>
        /// <returns>Returns a new <see cref="ClientAuthorizationDto"/> that represents the given authorization string.</returns>
        public static ClientAuthorizationDto FromBasicScheme(string auth)
        {
            auth = auth.Trim(); // Normalize string
            string[] values = Encoding.UTF8.GetString(Convert.FromBase64String(auth)).Split(':');
            return new ClientAuthorizationDto
            {
                ClientId = values.FirstOrDefault(),
                ClientSecret = values.LastOrDefault()
            };
        }
    }
}
