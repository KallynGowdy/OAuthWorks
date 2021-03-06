﻿// Copyright 2014 Kallyn Gowdy
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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines an abstract class that provides a basic implementation of <see cref="OAuthWorks.IRefreshToken"/>.
    /// </summary>
    [DataContract]
    public abstract class RefreshToken<TId> : IRefreshToken, IHasId<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshToken"/> class.
        /// </summary>
        /// <param name="user">The user that this token belongs to.</param>
        /// <param name="client">The client that has access to this token.</param>
        /// <param name="scopes">The scopes that this token provides access to.</param>
        /// <param name="expirationDateUtc">The date in UTC that this token should expire. Null means no expiration.</param>
        protected RefreshToken(TId id, IUser user, IClient client, IEnumerable<IScope> scopes, DateTime? expirationDateUtc)
        {
            if (id == null)     throw new ArgumentNullException("id");
            if (user == null)   throw new ArgumentNullException("user");
            if (client == null) throw new ArgumentNullException("client");
            if (scopes == null) throw new ArgumentNullException("scopes");

            this.Id = id;
            this.User = user;
            this.Client = client;
            this.Scopes = scopes;
            this.ExpirationDateUtc = expirationDateUtc;
        }

        /// <summary>
        /// Gets the client that this refreshToken belongs to.
        /// </summary>
        [DataMember(Name = "Client")]
        public IClient Client
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets whether this refreshToken has been revoked by the user.
        /// </summary>
        [DataMember(Name = "Revoked")]
        public bool Revoked
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name = "Id")]
        public TId Id
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
        /// Gets the user that the refresh refreshToken belongs to.
        /// </summary>
        [DataMember(Name = "User")]
        public IUser User
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the list of scopes that this refresh refreshToken provides access to.
        /// </summary>
        [DataMember(Name = "Scopes")]
        public IEnumerable<IScope> Scopes
        {
            get;
            set;
        }

        /// <summary>
        /// Revokes access from the client to be able to use this refreshToken for retrieving more access tokens.
        /// </summary>
        public virtual void Revoke()
        {
            this.Revoked = true;
        }

        /// <summary>
        /// Gets whether this refresh token has expired.
        /// </summary>
        public bool Expired
        {
            get
            {
                return !(ExpirationDateUtc == null || ExpirationDateUtc > DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Gets the date in universal coordinated time that this refresh token expires. Null defines that it does not expire.
        /// </summary>
        [DataMember(Name = "ExpirationDateUtc")]
        public DateTime? ExpirationDateUtc
        {
            get;
            protected set;
        }
    }
}
