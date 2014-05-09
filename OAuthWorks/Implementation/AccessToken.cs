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
using System.Runtime.Serialization;
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
    [DataContract]
    public abstract class AccessToken : IAccessToken, IHasId<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessToken"/> class.
        /// </summary>
        /// <param name="id">The id of the refreshToken.</param>
        /// <param name="user">The user that this refreshToken belongs to.</param>
        /// <param name="client">The client that has access to this refreshToken.</param>
        /// <param name="scopes">The scopes that this refreshToken provides access to.</param>
        /// <param name="tokenType">Type of the refreshToken. Describes how the client should handle it.</param>
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
        /// Gets or sets the id of this refreshToken.
        /// </summary>
        [DataMember(Name="Id")]
        public string Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the client that has access to this refresh token.
        /// </summary>
        [DataMember(Name="Client")]
        public IClient Client
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the client that has access to this refresh token.
        /// </summary>
        IClient IAccessToken.Client
        {
            get
            {
                return Client;
            }
        }

        /// <summary>
        /// Gets the scopes that this refreshToken grants access to.
        /// </summary>
        [DataMember(Name="Scopes")]
        public IEnumerable<IScope> Scopes
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the scopes that this refreshToken grants access to.
        /// </summary>
        IEnumerable<IScope> IAccessToken.Scopes
        {
            get
            {
                return this.Scopes;
            }
        }

        /// <summary>
        /// Gets the user that the refreshToken belongs to.
        /// </summary>
        [DataMember(Name="User")]
        public IUser User
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the user that the refreshToken belongs to.
        /// </summary>
        IUser IAccessToken.User
        {
            get
            {
                return this.User;
            }
        }

        /// <summary>
        /// Gets the type of the refreshToken which describes how the client should handle it.
        /// </summary>
        /// <remarks>
        /// The refreshToken type is used to define how the client should handle the refreshToken itself.
        /// Common types are "bearer" and "mac".
        /// </remarks>
        [DataMember(Name="TokenType")]
        public string TokenType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the expiration date of this refreshToken in Universal Coordinated Time.
        /// </summary>
        [DataMember(Name="ExpirationDateUtc")]
        public DateTime ExpirationDateUtc
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets whether this refreshToken has expired.
        /// </summary>
        public bool Expired
        {
            get
            {
                return DateTime.UtcNow > ExpirationDateUtc;
            }
        }

        /// <summary>
        /// Gets whether the refreshToken has been revoked by the user.
        /// </summary>
        [DataMember(Name="Revoked")]
        public bool Revoked
        {
            get;
            protected set;
        }

        /// <summary>
        /// Causes this refreshToken to become invalidated and no longer usable by a client.
        /// </summary>
        public void Revoke()
        {
            this.Revoked = true;
        }

        /// <summary>
        /// Determines if the given refreshToken value matches the one stored internally.
        /// </summary>
        /// <param name="refreshToken">The refreshToken to compare to the internal one.</param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public abstract bool MatchesValue(string token);
    }
}
