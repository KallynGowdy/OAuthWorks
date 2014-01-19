using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an enum that contains a list of error codes as defined in RFC 6749, Section 4.1.2.1, http://tools.ietf.org/html/rfc6749#section-4.1.2.1.
    /// </summary>
    public enum AuthorizationRequestCodeErrorType
    {
        /// <summary>
        /// Defines that the request is missing a required parameter, includes an invalid parameter value, includes a parameter more than once,
        /// or is otherwise malformed.
        /// </summary>
        InvalidRequest,

        /// <summary>
        /// Defines that the client is not authorized to request an authorization
        /// code using this method.
        /// </summary>
        UnauthorizedClient,

        /// <summary>
        /// Defines that the resource owner or authorization server denied the request.
        /// </summary>
        AccessDenied,

        /// <summary>
        /// Defines that the authorization server does not support obtaining an authorization code using this method.
        /// </summary>
        UnsupportedResponseType,

        /// <summary>
        /// Defines that the requested scope is invalid, unknown, or malformed.
        /// </summary>
        InvalidScope,

        /// <summary>
        /// The authorization server encountered an unexpected condition that prevented it from fufilling the request.
        /// </summary>
        ServerError,

        /// <summary>
        /// Defines that the authorization server is currently unable to handle the request due to a temporary overloading or maintenance of the server.
        /// </summary>
        TemporarilyUnavailable
    }
}
