using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for a representation of an OAuth Client from the perspective of an OAuth provider.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Gets the name of the client.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the list of allowed redirect URIs for this client.
        /// </summary>
        IEnumerable<string> RedirectUris
        {
            get;
        }

        /// <summary>
        /// Determines if the given secret matches the one stored internally.
        /// </summary>
        /// <param name="secret">The secret to match to the internal one.</param>
        /// <returns>Returns true if the secrets match, otherwise false.</returns>
        bool MatchesSecret(string secret);

        /// <summary>
        /// Determines if this client equals the given other client.
        /// </summary>
        /// <param name="other">The other client to determine equality to.</param>
        /// <returns>Returns true if the two clients equal each other, otherwise returns false.</returns>
        bool Equals(IClient other);
    }
}
