using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks.Factories
{
    /// <summary>
    /// Defines an interface for a factory that creates authorization code objects.
    /// </summary>
    /// <typeparam name="T">The type of authorization codes to create.</typeparam>
    public interface IAuthorizationCodeFactory<out TAuthorizationCode> : IFactory<TAuthorizationCode> where TAuthorizationCode : IAuthorizationCode
    {
        /// <summary>
        /// Creates a new <see cref="OAuthWorks.IAuthorizationCode"/> object given the granted scopes.
        /// </summary>
        /// <param name="scopes">The enumerable list of scopes that were granted by the user to the client.</param>
        /// <param name="user">The user that the created authorization code is bound to.</param>
        /// <param name="client">The client that the code is granted for.</param>
        /// <param name="redirectUri">The URI that was provided by the client that specifies the location that the user should be redirected to after completing authorization.</param>
        /// <returns>Returns a new OAuthWorks.CreatedToken(of TAuthorizationCode) object.</returns>
        ICreatedToken<TAuthorizationCode> Create(Uri redirectUri, IUser user, IClient client, IEnumerable<IScope> scopes);
    }
}
