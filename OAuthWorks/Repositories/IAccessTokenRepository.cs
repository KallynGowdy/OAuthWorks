using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Repositories
{
    /// <summary>
    /// Defines an interface for a repository that contains <see cref="OAuthWorks.IAccessToken"/> objects.
    /// </summary>
    public interface IAccessTokenRepository<T> : IRepository<string, T> where T : IAccessToken
    {
        /// <summary>
        /// Gets a list of Access Tokens that belong to the given user.
        /// </summary>
        /// <param name="user">The User that owns the access tokens.</param>
        /// <returns>Returns a new enumerable list of <see cref="OAuthWorks.IAccessToken"/> objects that belong to the given user.</returns>
        IEnumerable<T> GetByUser(IUser user);
    }
}
