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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a base class that provides a basic implemenatation of <see cref="OAuthWorks.AccessToken"/>.
    /// </summary>
    /// <remarks>
    /// For an implemenatation, see <see cref="OAuthWorks.Implementation.HashedAccessToken"/>.
    /// </remarks>
    public abstract class AccessToken : IAccessToken, IHasId<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessToken"/> class.
        /// </summary>
        /// <param name="id">The id of the token.</param>
        /// <param name="user">The user that this token belongs to.</param>
        /// <param name="client">The client that has access to this token.</param>
        /// <param name="scopes">The scopes that this token provides access to.</param>
        /// <param name="tokenType">Type of the token. Describes how the client should handle it.</param>
        /// <param name="expirationDateUtc">The date of expiration in Universal Coordinated Time.</param>
        protected AccessToken(string id, IUser user, IClient client, IEnumerable<IScope> scopes, string tokenType, DateTime expirationDateUtc)
        {
            Contract.Requires(!string.IsNullOrEmpty(id));
            Contract.Requires(user != null);
            Contract.Requires(client != null);
            Contract.Requires(scopes != null);
            Contract.Requires(!string.IsNullOrEmpty(tokenType));
            Contract.Requires(expirationDateUtc > DateTime.UtcNow);
            this.Id = id;
            this.Client = client;
            this.User = user;
            this.Scopes = scopes;
            this.TokenType = tokenType;
            this.ExpirationDateUtc = expirationDateUtc;
        }

        /// <summary>
        /// Gets or sets the id of this token.
        /// </summary>
        public string Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the client that has access to this token.
        /// </summary>
        public IClient Client
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the scopes that this token grants access to.
        /// </summary>
        public IEnumerable<IScope> Scopes
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the user that the token belongs to.
        /// </summary>
        public IUser User
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the type of the token which describes how the client should handle it.
        /// </summary>
        /// <remarks>
        /// The token type is used to define how the client should handle the token itself.
        /// Common types are "bearer" and "mac".
        /// </remarks>
        public string TokenType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the expiration date of this token in Universal Coordinated Time.
        /// </summary>
        public DateTime ExpirationDateUtc
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets whether this token has expired.
        /// </summary>
        public bool Expired
        {
            get
            {
                return (ExpirationDateUtc - DateTime.UtcNow).TotalSeconds <= 0;
            }
        }

        /// <summary>
        /// Gets whether the token has been revoked by the user.
        /// </summary>
        public bool Revoked
        {
            get;
            protected set;
        }

        /// <summary>
        /// Causes this token to become invalidated and no longer usable by a client.
        /// </summary>
        public void Revoke()
        {
            this.Revoked = true;
        }

        /// <summary>
        /// Determines if the given token value matches the one stored internally.
        /// </summary>
        /// <param name="token">The token to compare to the internal one.</param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public abstract bool MatchesValue(string token);
    }
}
