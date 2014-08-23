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

using OAuthWorks;
using OAuthWorks.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ExampleMvcWebApplication.Models
{
    [DataContract(Name = "AccessToken")]
    public class AccessToken : IAccessToken
    {

        public AccessToken()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessToken"/> class using the given <see cref="OAuthWorks.Implementation.HashedAccessToken"/> as a reference.
        /// </summary>
        /// <param name="token">The token that should be copied into a new token.</param>
        public AccessToken(ICreatedToken<IAccessToken> token)
        {
            TokenValue = new HashedValue(token.TokenValue);
            Client = (Client)token.Token.Client;
            User = (User)token.Token.User;
            Scopes = token.Token.Scopes.Cast<Scope>().ToList();
            ExpirationDateUtc = token.Token.ExpirationDateUtc;
            Revoked = token.Token.Revoked;
            if((IHasId<string> id = token.Token as IHasId<string>) != null)
            {
                this.Id = id.Id;
            }
            else
            {
                this.Id = Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the token.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hashed token value.
        /// </summary>
        public virtual HashedValue TokenValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the client that has access to this refreshToken.
        /// </summary>
        public virtual Client Client
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the client that has access to this refreshToken.
        /// </summary>
        IClient IAccessToken.Client
        {
            get { return Client; }
        }

        /// <summary>
        /// Gets the scopes that this refresh token grants access to.
        /// </summary>
        public virtual ICollection<Scope> Scopes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the scopes that this refresh token grants access to.
        /// </summary>
        IEnumerable<IScope> IAccessToken.Scopes
        {
            get
            {
                return Scopes;
            }
        }

        /// <summary>
        /// Gets the user that the refresh token belongs to.
        /// </summary>
        public virtual User User
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the user that the refresh token belongs to.
        /// </summary>
        IUser IAccessToken.User
        {
            get { return User; }
        }

        /// <summary>
        /// Gets the type of the refreshToken which describes how the client should handle it.
        /// </summary>
        /// <remarks>
        /// The refreshToken type is used to define how the client should handle the refresh token itself.
        /// Common types are "bearer" and "mac".
        /// </remarks>
        public string TokenType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the expiration date of this refresh token in Universal Coordinated Time.
        /// </summary>
        public DateTime ExpirationDateUtc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether this refreshToken has expired.
        /// </summary>
        public bool Expired
        {
            get { return DateTime.UtcNow > ExpirationDateUtc; }
        }

        /// <summary>
        /// Gets whether the refreshToken has been revoked by the user.
        /// </summary>
        public bool Revoked
        {
            get;
            set;
        }

        /// <summary>
        /// Causes this refreshToken to become invalidated and no longer usable by a client.
        /// </summary>
        public void Revoke()
        {
            Revoked = true;
        }

        /// <summary>
        /// Determines if the given refreshToken value matches the one stored internally.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public bool MatchesValue(string token)
        {
            return TokenValue.MatchesHash(token);
        }
    }
}