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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="IPasswordCredentialsAccessTokenRequest"/>.
    /// </summary>
    [DataContract]
    public class PasswordCredentialsAccessTokenRequest : AccessTokenRequest, IPasswordCredentialsAccessTokenRequest
    {
        /// <summary>
        /// Gets the user that account access is being requested for. The given username and password must match the given user for proper validation.
        /// </summary>
        /// <returns>Returns the authenticated user that access is being requested for.</returns>
        public IValidatedUser User
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordCredentialsAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="clientId">The id of the client making the request.</param>
        /// <param name="clientSecret">The secret of the client making the request.</param>
        /// <param name="grantType">The type of grant requested by the client.</param>
        /// <param name="scope">The scope requested by the client.</param>
        public PasswordCredentialsAccessTokenRequest(IValidatedUser user, string clientId, string clientSecret, string grantType, string scope)
            : base(clientId, clientSecret, grantType, scope, null)
        {
            // Do not validate variables because the OAuthProvider will do that and will return a value that the client can understand representing what was wrong.

            //if (string.IsNullOrEmpty(clientId)) throw new ArgumentException("Cannot be null or empty.", "clientId");
            //if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentException("Cannot be null or empty.", "clientSecret");
            //if (string.IsNullOrEmpty(grantType)) throw new ArgumentException("Cannot be null or empty.", "grantType");

            // Validate the user variable because it is supplied by the server. If it is not supplied then something is wrong and we should not make assumptions
            // about whether the user is validated or not.
            if (user == null) throw new ArgumentNullException("user");

            this.User = user;
        }
    }
}