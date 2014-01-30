using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Factories
{
    /// <summary>
    /// Defines an interface for a factory that creates access token objects.
    /// </summary>
    public interface IAccessTokenFactory<in TAccessToken> : IFactory<TAccessToken> where TAccessToken : IAccessToken
    {
        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessToken"/> given the token value and expiration date in Universal Coordinated Time.
        /// </summary>
        /// <param name="token">The value that defines the 'password' of the token which determines if a client should have access to a resource.</param>
        /// <param name="expirationDateUtc">The expiration date of the token in Universal Coordinated Time.</param>
        /// <returns></returns>
        TAccessToken Get(string token, DateTime expirationDateUtc);

        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessToken"/> given the Client and the list of scopes that the client has access to.
        /// </summary>
        /// <param name="clientId">The client that should have access to the new token.</param>
        /// <param name="scopes">The list of scopes that the client should have access to.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessToken"/> object.</returns>
        TAccessToken Get(IClient client, params IScope[] scopes);

        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessToken"/> object given the <see cref="OAuthWorks.IClient"/> and the list of scopes that the client has access to.
        /// </summary>
        /// <param name="client">The client that should have access to the new token.</param>
        /// <param name="scopes">The list of scopes that the client should have access to.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessToken"/> object.</returns>
        TAccessToken Get(IClient client, IEnumerable<IScope> scopes);
    }
}
