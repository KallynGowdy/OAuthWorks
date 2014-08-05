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
    public interface IAuthorizationCodeResponseFactory : IFactory<ISuccessfulAuthorizationCodeResponse>
    {
        /// <summary>
        /// Creates a new <see cref="OAuthWorks.ISuccessfulAuthorizationCodeResponse"/> object given the authorization code and state.
        /// </summary>
        /// <param name="authorizationCode">The authorization code that is granted to the client.</param>
        /// <param name="state">The state that was provided by the client to prevent Cross Site Request Forgery.</param>
        /// <param name="redirect">The <see cref="Uri"/> provided by the client that the user should be redirected to.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.ISuccessfulAuthorizationCodeResponse"/> object that contains the given values.</returns>
        ISuccessfulAuthorizationCodeResponse Create(string authorizationCode, string state, Uri redirect);

        /// <summary>
        /// Creates a new <see cref="IUnsuccessfulAuthorizationCodeResponse"/> object using the given information.
        /// </summary>
        /// <param name="errorCode">The error code that describes the problem that occurred. (Required)</param>
        /// <param name="client">The client that made the request. (Nullable)</param>
        /// <param name="user">The user that the client was requesting access for.</param>
        /// <param name="scopes">The list of scopes that the client requested access to.</param>
        /// <param name="errorDescription">A human-readable string that describes what went wrong. (Optional)</param>
        /// <param name="errorUri">A URI that points to a human-readable web page that contains information about the error. (Optional)</param>
        /// <param name="state">The state that was provided by the client in the request. (Required if client provided state)</param>
        /// <param name="innerException">The exception that caused this error to occur.</param>
        /// <param name="redirectUri">The <see cref="Uri" /> provided by the client that the user should be redirected to.</param>
        /// <returns>Returns a new <see cref="IUnsuccessfulAuthorizationCodeResponse"/> object.</returns>
        IUnsuccessfulAuthorizationCodeResponse CreateError(AuthorizationCodeRequestSpecificErrorType errorCode, IClient client, IUser user, IEnumerable<IScope> scopes, string errorDescription, Uri errorUri, string state, Uri redirectUri, Exception innerException);
    }
}
