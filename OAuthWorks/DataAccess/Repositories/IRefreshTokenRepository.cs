// Copyright 2014 Kallyn Gowdy
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.DataAccess.Repositories
{
    /// <summary>
    /// Defines an interface for a repository that stores <see cref="IRefreshToken"/> objects.
    /// </summary>
    public interface IRefreshTokenRepository : IDisposable
    {
        /// <summary>
        /// Adds the given refresh token to the repository.
        /// </summary>
        /// <param name="refreshToken">The refresh token that should be added to the repository.</param>
        void Add(ICreatedToken<IRefreshToken> refreshToken);

        /// <summary>
        /// Removes(deletes) the given refreshToken from this repository.
        /// </summary>
        /// <param name="refreshToken">The refresh token to remove from the repository.</param>
        void Remove(IRefreshToken token);

        /// <summary>
        /// Gets the refresh refreshToken that can be used by the given client to retrive access tokens for the given user's account.
        /// </summary>
        /// <remarks>
        /// Note that while only one refresh token *should* be active at a time, the usage of this method by <see cref="OAuthWorks.OAuthProvider"/>
        /// requires that all refresh tokens be retrievable, that way unused/old tokens will be able to be easily destroyed.
        /// </remarks>
        /// <param name="user">The user that owns the accout that the refreshToken gives access to.</param>
        /// <param name="client">The client that maintains possesion of the refresh refreshToken.</param>
        /// <returns>Returns the refresh tokens that can be used by the given client for the given user's account if one exists. Otherwise returns null.</returns>
        IEnumerable<IRefreshToken> GetByUserAndClient(IUser user, IClient client);

        /// <summary>
        /// Gets a refresh refreshToken by the value that was given to the client.
        /// </summary>
        /// <param name="refreshToken">The refresh token that was issued to the client.</param>
        /// <returns>Returns the refresh token that belongs to the given value.</returns>
        IRefreshToken GetByValue(string token);
    }
}
