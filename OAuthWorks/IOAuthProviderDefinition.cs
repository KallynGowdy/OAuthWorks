using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface that contains several values that define OAuth provider properties such as endpoints and supported scopes.
    /// </summary>
    public interface IOAuthProviderDefinition
    {
        /// <summary>
        /// Gets the scopes that can be requested by a third party client.
        /// Note that excluding a scope from this list does not prevent it from being requested and given,
        /// it just prevents it from being advertized.
        /// </summary>
        IEnumerable<IScope> Scopes
        {
            get;
        }

    }
}
