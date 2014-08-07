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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides an implementation of <see cref="IScopeAuthorizationRequest"/>.
    /// </summary>
    [DataContract]
    public class ScopeAuthorizationRequest : IScopeAuthorizationRequest
    {
        /// <summary>
        /// Gets the <see cref="IClient" /> that is requesting access to the given scopes.
        /// </summary>
        /// <returns>Returns the <see cref="IClient" /> that is requesting authorization for the scopes.</returns>
        [DataMember(Name = "client", IsRequired = true)]
        public IClient Client
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of <see cref="IScope" /> objects that the client is requesting authorization to.
        /// </summary>
        /// <returns>Returns a <see cref="IEnumerable{IScope}" /> object that contain the list of <see cref="IScope" /> objects that the client is requesting authorization to.</returns>
        [DataMember(Name = "scopes", IsRequired = true)]
        public IEnumerable<IScope> Scopes
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the State that the Client sent in it's request to act as a CSRF token.
        /// </summary>
        /// <returns>Returns a <see cref="string" /> that the Client sent in it's request.</returns>
        [DataMember(Name = "state")]
        public string State
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="IUser" /> that the access is being requested from.
        /// </summary>
        /// <returns>Returns a <see cref="IUser" /> object that the authorization is being requested from.</returns>
        [DataMember(Name = "user", IsRequired = true)]
        public IUser User
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeAuthorizationRequest"/> class.
        /// </summary>
        /// <param name="client">The client that made the authorization request.</param>
        /// <param name="user">The user that the client made the authorization request for.</param>
        /// <param name="scopes">The scopes that the client is requesting authorization for.</param>
        /// <param name="state">The state that the client sent in it's request.</param>
        public ScopeAuthorizationRequest(IClient client, IUser user, IEnumerable<IScope> scopes, string state)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (user == null) throw new ArgumentNullException("user");
            if (scopes == null) throw new ArgumentNullException("scopes");

            this.Client = client;
            this.User = user;
            this.Scopes = scopes;
            this.State = state;
        }
    }
}
