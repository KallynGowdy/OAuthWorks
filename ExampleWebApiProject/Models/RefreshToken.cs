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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ExampleWebApiProject.Models
{
    public class RefreshToken : IRefreshToken, IHasId<string>
    {
        public RefreshToken() { }

        public RefreshToken(HashedRefreshToken<string> token)
        {
            this.Id = token.Id;
            this.Client = (Client)token.Client;
            this.User = (User)token.User;
            this.TokenValue = new HashedValue(token.TokenHash.Hash, token.TokenHash.Salt, token.TokenHash.HashIterations);
            this.Scopes = token.Scopes.Cast<Scope>().ToList();
            this.ExpirationDateUtc = token.ExpirationDateUtc;
            this.Revoked = token.Revoked;
        }

        public RefreshToken(ICreatedToken<IRefreshToken> refreshToken)
        {
            this.Client = (Client)refreshToken.Token.Client;
            this.User = (User)refreshToken.Token.User;
            this.TokenValue = new HashedValue(refreshToken.TokenValue);
            this.Scopes = refreshToken.Token.Scopes.Cast<Scope>().ToList();
            this.ExpirationDateUtc = refreshToken.Token.ExpirationDateUtc;
            this.Revoked = refreshToken.Token.Revoked;
            if((IHasId<string> id = refreshToken as IHasId<string>) != null)
            {
                this.Id = id.Id;
            }
        }


        /// <summary>
        /// Gets or sets the id of this token.
        /// </summary>
        [Key]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client that this refreshToken belongs to.
        /// </summary>
        public virtual Client Client
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user that the refresh refreshToken belongs to.
        /// </summary>
        public virtual User User
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of scopes that this refresh refresh token provides access to.
        /// </summary>
        public virtual ICollection<Scope> Scopes
        {
            get;
            set;
        }

        public virtual HashedValue TokenValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the client that this refresh token belongs to.
        /// </summary>
        IClient IRefreshToken.Client
        {
            get
            {
                return Client;
            }
        }

        /// <summary>
        /// Gets the user that the refresh token belongs to.
        /// </summary>
        IUser IRefreshToken.User
        {
            get
            {
                return User;
            }
        }

        /// <summary>
        /// Gets the list of scopes that this refresh token provides access to.
        /// </summary>
        IEnumerable<IScope> IRefreshToken.Scopes
        {
            get
            {
                return Scopes;
            }
        }

        /// <summary>
        /// Gets or sets whether this refresh token has been revoked from the client.
        /// </summary>
        public bool Revoked
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether this refresh token has expired.
        /// </summary>
        public bool Expired
        {
            get { return !(ExpirationDateUtc == null || ExpirationDateUtc > DateTime.Now); }
        }

        /// <summary>
        /// Gets or sets the date that this refresh token expires. Null defines that it does not expire.
        /// </summary>
        public DateTime? ExpirationDateUtc
        {
            get;
            set;
        }

        /// <summary>
        /// Revokes access from the client to be able to use this refreshToken for retrieving more access tokens.
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