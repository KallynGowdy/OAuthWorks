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

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for refresh tokens.
    /// </summary>
    public interface IRefreshToken : IToken
    {
        /// <summary>
        /// Gets the client that this refreshToken belongs to.
        /// </summary>
        IClient Client
        {
            get;
        }

        /// <summary>
        /// Gets the user that the refresh refreshToken belongs to.
        /// </summary>
        IUser User
        {
            get;
        }

        /// <summary>
        /// Gets the list of scopes that this refresh refreshToken provides access to.
        /// </summary>
        IEnumerable<IScope> Scopes
        {
            get;
        }

        /// <summary>
        /// Gets whether this refresh token has been revoked from the client.
        /// </summary>
        bool Revoked
        {
            get;
        }

        /// <summary>
        /// Gets whether this refresh token has expired.
        /// </summary>
        bool Expired
        {
            get;
        }

        /// <summary>
        /// Gets the date that this refresh token expires. Null defines that it does not expire.
        /// </summary>
        DateTime? ExpirationDateUtc
        {
            get;
        }

        /// <summary>
        /// Revokes access from the client to be able to use this refreshToken for retrieving more access tokens.
        /// </summary>
        void Revoke();
    }
}
