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

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for access tokens.
    /// </summary>
    public interface IAccessToken : IToken
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
        /// Gets the user that the token belongs to.
        /// </summary>
        IUser User
        {
            get;
        }

        /// <summary>
        /// Gets the type of the token which describes how the client should handle it.
        /// </summary>
        /// <remarks>
        /// The token type is used to define how the client should handle the token itself.
        /// Common types are "bearer" and "mac".
        /// </remarks>
        string TokenType
        {
            get;
        }

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
