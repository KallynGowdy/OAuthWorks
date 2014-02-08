using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for an object that contains a value that is used as a representation of granted authorization.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Determines if the given token value matches the one stored internally.
        /// </summary>
        /// <param name="token">The token to compare to the internal one.</param>
        /// <returns>Returns true if the two tokens match, otherwise false.</returns>
        bool MatchesValue(string token);
    }
}
