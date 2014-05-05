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
    /// Defines an interface for a repository that stores OAuthWorks.IRefreshToken objects.
    /// </summary>
    public interface IRefreshTokenRepository<TRefreshToken> : IRepository<TRefreshToken>
    {
        /// <summary>
        /// Removes(deletes) the given refreshToken from this repository.
        /// </summary>
        /// <param name="refreshToken">The refreshToken to remove.</param>
        void Remove(TRefreshToken token);

        /// <summary>
        /// Gets the refresh refreshToken that can be used by the given client to retrive access tokens for the given user's account.
        /// </summary>
        /// <remarks>
        /// Note that only one refreshToken is returned. This is by design, each client should only have one refresh refreshToken for each user.
        /// If an organization needs to have access to a user's account through multiple methods, then they need to act as two different clients.
        /// This simplifies management of tokens by our server by allowing deletion of revoked tokens.
        /// </remarks>
        /// <param name="user">The user that owns the accout that the refreshToken gives access to.</param>
        /// <param name="client">The client that maintains possesion of the refresh refreshToken.</param>
        /// <returns>Returns the refresh tokens that can be used by the given client for the given user's account if one exists. Otherwise returns null.</returns>
        TRefreshToken GetByUserAndClient(IUser user, IClient client);

        /// <summary>
        /// Gets a refresh refreshToken by the value that was given to the client.
        /// </summary>
        /// <param name="refreshToken">The refresh refreshToken that was issued to the client.</param>
        /// <returns>Returns the refreshToken that belongs to the given value.</returns>
        TRefreshToken GetByValue(string token);
    }
}
