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
    public interface IAuthorizationCodeResponseFactory<out TAuthorizationCodeResponse> : IFactory<TAuthorizationCodeResponse> where TAuthorizationCodeResponse : IAuthorizationCodeResponse
    {
        /// <summary>
        /// Creates a new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> object given the authorization code and state.
        /// </summary>
        /// <param name="authorizationCode">The authorization code that is granted to the client.</param>
        /// <param name="state">The state that was provided by the client to prevent Cross Site Request Forgery.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> object that contains the given values.</returns>
        TAuthorizationCodeResponse Create(string authorizationCode, string state);

    }
}
