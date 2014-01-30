using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for a request for an access token by a client.
    /// </summary>
    public interface IAccessTokenRequest
    {
        /// <summary>
        /// Gets the authorization that was given to the client.
        /// </summary>
        string AuthorizationCode
        {
            get;
        }

        /// <summary>
        /// Gets the Id of the client.
        /// </summary>
        string ClientId
        {
            get;
        }

        /// <summary>
        /// Gets the redirect uri that was provided in getting the authorization code.
        /// </summary>
        string RedirectUri
        {
            get;
        }

        /// <summary>
        /// Gets the secret (password) that was provided by the client.
        /// </summary>
        string ClientSecret
        {
            get;
        }
    }
}
