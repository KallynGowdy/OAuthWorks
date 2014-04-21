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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Code
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
            return this.Code.Equals(token, StringComparison.Ordinal);
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
    }
}
