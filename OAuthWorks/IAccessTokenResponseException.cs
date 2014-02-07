using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for an object that defines that an error/exception occurred in a client's access token request.
    /// </summary>
    public interface IAccessTokenResponseException
    {
        /// <summary>
        /// Gets the error code which defines what happened in the request.
        /// </summary>
        AccessTokenRequestError ErrorCode
        {
            get;
        }

        /// <summary>
        /// Gets the basic description of the error that contains sugesstions on how the client developer should fix the problem.
        /// </summary>
        string ErrorDescription
        {
            get;
        }

        /// <summary>
        /// Gets a URI that points to a web page that the developer can visit to find information about the error.
        /// </summary>
        string ErrorUri
        {
            get;
        }
    }
}
