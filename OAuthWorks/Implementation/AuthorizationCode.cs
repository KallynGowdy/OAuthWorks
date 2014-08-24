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
    /// Defines an abstract class that provides a basic implementation of <see cref="OAuthWorks.IAuthorizationCode"/>.
    /// </summary>
    public abstract class AuthorizationCode<TId> : IAuthorizationCode, IHasId<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCode"/> class.
        /// </summary>
        /// <param name="id">The Id of the authorization code.</param>
        /// <param name="user">The user.</param>
        /// <param name="client">The client.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="expirationDateUtc">The expiration date UTC.</param>
        protected AuthorizationCode(TId id, IUser user, IClient client, IEnumerable<IScope> scopes, Uri redirectUri, DateTime expirationDateUtc)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (user == null) throw new ArgumentNullException("user");
            if (client == null) throw new ArgumentNullException("client");
            if (scopes == null) throw new ArgumentNullException("scopes");
            if (redirectUri == null) throw new ArgumentNullException("redirectUri");
            this.User = user;
            this.Client = client;
            this.Scopes = scopes;
            this.RedirectUri = redirectUri;
            this.ExpirationDateUtc = expirationDateUtc;
            this.Id = id;
        }

        /// <summary>
        /// Gets the Id of this authorization code.
        /// </summary>
        /// <returns></returns>
        public TId Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets whether this authorization code has expired.
        /// </summary>
        public bool Expired
        {
            get
            {
                return (ExpirationDateUtc - DateTime.Now).TotalSeconds < 0;
            }
        }

        /// <summary>
        /// Gets the redirect Uri that was used by the client when retrieving this refreshToken.
        /// </summary>
        public Uri RedirectUri
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the scopes that this code grants access to.
        /// </summary>
        public IEnumerable<IScope> Scopes
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the expiration date of this authorization code in Universal Coordinated Time.
        /// </summary>
        public DateTime ExpirationDateUtc
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the user that this authorization code belongs to.
        /// </summary>
        public IUser User
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the client that this authorization code was granted to.
        /// </summary>
        public IClient Client
        {
            get;
            protected set;
        }

        /// <summary>
        /// Determines if the given refreshToken value matches the one stored internally.
        /// </summary>
        /// <param name="refreshToken">The refreshToken to compare to the internal one.</param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public abstract bool MatchesValue(string token);

        /// <summary>
        /// Gets whether this authorization code has been revoked from the client.
        /// </summary>
        public bool Revoked
        {
            get;
            private set;
        }

        /// <summary>
        /// Revokes the ability to use this authorization code from the client.
        /// </summary>
        public void Revoke()
        {
            Revoked = true;
        }
    }
}
