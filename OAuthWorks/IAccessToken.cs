using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for access tokens.
    /// </summary>
    public interface IAccessToken
    {
        /// <summary>
        /// Gets the client that has access to this token.
        /// </summary>
        IClient Client
        {
            get;
        }

        /// <summary>
        /// Gets the scopes that this token grants access to.
        /// </summary>
        IEnumerable<IScope> Scopes
        {
            get;
        }

        /// <summary>
        /// Determines if the given token value matches the one stored internally.
        /// </summary>
        /// <param name="token">The token to compare to the internal one.</param>
        /// <returns>Returns true if the two tokens match, otherwise false.</returns>
        bool MatchesValue(string token);

        /// <summary>
        /// Gets the expiration date of this token in Universal Coordinated Time.
        /// </summary>
        DateTime ExpirationDateUtc
        {
            get;
        }

        /// <summary>
        /// Gets whether this token has expired.
        /// </summary>
        bool Expired
        {
            get;
        }

        /// <summary>
        /// Gets whether the token has been revoked by the user.
        /// </summary>
        bool Revoked
        {
            get;
        }

        /// <summary>
        /// Causes this token to become invalidated and no longer usable by a client.
        /// </summary>
        void Revoke();
    }
}
