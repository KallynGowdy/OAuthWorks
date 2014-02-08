using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Factories
{
    /// <summary>
    /// Defines an interface for a factory that creates <see cref="OAuthWorks.IAuthorizationCodeResponse"/> objects.
    /// </summary>
    public interface IAuthorizationCodeResponseFactory<out TAuthorizationCodeResponse, out TAuthorizationCodeResponseError> : IFactory<TAuthorizationCodeResponse>
        where TAuthorizationCodeResponse : IAuthorizationCodeResponse
        where TAuthorizationCodeResponseError : Exception, IAuthorizationCodeResponseError
    {
        /// <summary>
        /// Creates a new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> object given the authorization code and state.
        /// </summary>
        /// <param name="authorizationCode">The authorization code that is granted to the client.</param>
        /// <param name="state">The state that was provided by the client to prevent Cross Site Request Forgery.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> object that contains the given values.</returns>
        TAuthorizationCodeResponse Create(string authorizationCode, string state);

        /// <summary>
        /// Creates a new OAuthWorks.IAuthorizationCodeResponseError object using the given information.
        /// </summary>
        /// <param name="errorCode">The error code that describes the basic problem that occurred. (Required)</param>
        /// <param name="errorDescription">A human-readable string that describes what went wrong. (Optional)</param>
        /// <param name="errorUri">A URI that points to a humar-readable web page that contains information about the error. (Optional)</param>
        /// <param name="state">The state that was provided by the client in the request. (Required if client provided state)</param>
        /// <param name="innerException">The exception that caused this error to occur.</param>
        /// <returns>Returns a new OAuthWorks.IAuthorizationCodeResponseError object.</returns>
        TAuthorizationCodeResponseError CreateError(AuthorizationRequestCodeErrorType errorCode, string errorDescription, Uri errorUri, string state, Exception innerException);
    }
}
