using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a list of the possible response types that can be requested from an OAuth Provider.
    /// </summary>
    public enum AuthorizationCodeResponseType
    {
        /// <summary>
        /// Defines that the response should be an Authorization Code that the client can exchange for a token.
        /// </summary>
        Code
    }
}
