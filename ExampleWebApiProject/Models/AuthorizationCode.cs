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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ExampleWebApiProject.Models
{
    public class AuthorizationCode : IAuthorizationCode
    {
        public AuthorizationCode()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCode" /> class.
        /// </summary>
        /// <param name="authorizationCode">The authorization code that this code should be generated from.</param>
        public AuthorizationCode(ICreatedToken<IAuthorizationCode> authorizationCode)
        {
            this.Code = new HashedValue(authorizationCode.TokenValue);
            this.Client = (Client)authorizationCode.Token.Client;
            this.ExpirationDateUtc = authorizationCode.Token.ExpirationDateUtc;
            this.Revoked = authorizationCode.Token.Revoked;
            this.RedirectUri = authorizationCode.Token.RedirectUri.ToString();
            this.User = (User)authorizationCode.Token.User;
            if ((IHasId<string> id = authorizationCode.Token as IHasId<string>) != null)
            {
                this.Id = id.Id;
            }
        }

        /// <summary>
        /// Gets whether this authorization code has expired.
        /// </summary>
        public bool Expired
        {
            get
            {
                return DateTime.UtcNow > ExpirationDateUtc;
            }
        }

        /// <summary>
        /// Gets or sets the redirect Uri that was used by the client when retrieving this token.
        /// </summary>
        public string RedirectUri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the redirect Uri that was used by the client when retrieving this token.
        /// </summary>
        Uri IAuthorizationCode.RedirectUri
        {
            get
            {
                return new Uri(RedirectUri);
            }
        }

        /// <summary>
        /// Gets or sets the scopes that this code grants access to.
        /// </summary>
        public virtual ICollection<Scope> Scopes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the scopes that this code grants access to.
        /// </summary>
        IEnumerable<IScope> IAuthorizationCode.Scopes
        {
            get { return this.Scopes; }
        }

        /// <summary>
        /// Gets or sets the expiration date of this authorization code in Universal Coordinated Time.
        /// </summary>
        public DateTime ExpirationDateUtc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user that this authorization code belongs to.
        /// </summary>
        public virtual User User
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the user that this authorization code belongs to.
        /// </summary>
        IUser IAuthorizationCode.User
        {
            get
            {
                return this.User;
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the authorization code.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hashed code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public virtual HashedValue Code
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the given token value matches the one stored internally.
        /// </summary>
        /// <param name="token">The token to compare to the internal one.</param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public bool MatchesValue(string token)
        {
            return Code.MatchesHash(token);
        }

        /// <summary>
        /// Gets the client that this authorization code was granted to.
        /// </summary>
        public virtual Client Client
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the client that this authorization code was granted to.
        /// </summary>
        IClient IAuthorizationCode.Client
        {
            get { return Client; }
        }


        /// <summary>
        /// Gets a value indicating whether this <see cref="AuthorizationCode" /> is revoked.
        /// </summary>
        /// <value>
        /// <c>true</c> if revoked; otherwise, <c>false</c>.
        /// </value>
        public bool Revoked
        {
            get;
            private set;
        }

        /// <summary>
        /// Revokes this instance.
        /// </summary>
        public void Revoke()
        {
            Revoked = true;
        }
    }
}
