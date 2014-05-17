using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for a response to an access token request.
    /// </summary>
    public interface IAccessTokenResponse
    {
        /// <summary>
        /// Gets whether the request was successful.
        /// </summary>
        /// <returns>Returns whether the request by the client was successful.</returns>
        bool IsSuccessful
        {
            get;
        }
    }
}
