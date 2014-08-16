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

namespace ExampleMvcWebApplication.Models
{
    public class User : IUser
    {
        /// <summary>
        /// Gets the Id of the user.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the granted authorization codes.
        /// </summary>
        /// <value>
        /// The granted authorization codes.
        /// </value>
        public virtual ICollection<AuthorizationCode> GrantedAuthorizationCodes
        {
            get;
            set;
        }

        
        public virtual HashedValue Password
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the given scope has been granted to the given client by this user.
        /// </summary>
        /// <param name="client">The client that is used to determine whether it has been granted the scope by this user.</param>
        /// <param name="scope">The scope that may or may not have been granted to the given client.</param>
        /// <returns>
        /// Returns true if the given client has been granted the given scope, otherwise false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// client
        /// or
        /// scope
        /// </exception>
        public bool HasGrantedScope(IClient client, IScope scope)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }
            return GrantedAuthorizationCodes.Any(c => c.Client.Equals(client) && c.Scopes.Contains(scope));
        }

        public bool IsValidPassword(string password)
        {
            return password != null && this.Password.MatchesHash(password);
        }
    }
}
