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
        /// <param name="generatedAuthorizationCode">The object that the generated authorization code should be put in.</param>
        /// <param name="scopes">The enumerable list of scopes that were granted by the user to the client.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAuthorizationCode"/> object.</returns>
        TAuthorizationCode Create(out string generatedAuthorizationCode, IEnumerable<IScope> scopes);
    }
}
