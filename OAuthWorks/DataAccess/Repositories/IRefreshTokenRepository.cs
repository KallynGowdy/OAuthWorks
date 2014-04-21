using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.DataAccess.Repositories
{
    /// <summary>
    /// Defines an interface for a repository that stores OAuthWorks.IRefreshToken objects.
    /// </summary>
    public interface IRefreshTokenRepository<TRefreshToken> : IRepository<TRefreshToken>
    {
        /// <summary>
        /// Gets a refresh token that can be used by the given client to retrive access tokens for the given user's account.
        /// </summary>
        /// <param name="user">The user that owns the accout that the token gives access to.</param>
        /// <param name="client">The client that maintains possesion of the refresh token.</param>
        /// <returns>Returns the refresh token that can be used by the given client for the given user's account if one exists. Otherwise returns null.</returns>
        TRefreshToken GetByUserAndClient(IUser user, IClient client);
    }
}
