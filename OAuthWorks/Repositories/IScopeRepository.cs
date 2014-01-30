using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Repositories
{
    /// <summary>
    /// Defines an interface for a repository that contains <see cref="OAuthWorks.IScope"/> objects.
    /// </summary>
    public interface IScopeRepository<in T> : IRepository<string, T> where T : IScope
    {
        /// <summary>
        /// Gets a list of all of the scopes that are contained in this repository.
        /// </summary>
        /// <returns>An enumerable list of the scopes that are contained in this repository.</returns>
        IEnumerable<T> GetAllScopes();

        /// <summary>
        /// Gets a list of all of the scopes that are provided by the given access token.
        /// </summary>
        /// <param name="token">The token to get all of the scopes for.</param>
        /// <returns>Returns an enumerable list of the scopes that are provided by the given token.</returns>
        IEnumerable<T> GetByToken(IAccessToken token);
    }
}
