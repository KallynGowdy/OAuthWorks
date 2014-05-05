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
    /// Defines a class that provides a basic implementation of <see cref="IPasswordCredentialsAccessTokenRequest"/>.
    /// </summary>
    [DataContract]
    public class PasswordCredentialsAccessTokenRequest : AccessTokenRequest, IPasswordCredentialsAccessTokenRequest
    {
        /// <summary>
        /// Gets the identifier of the user who's credentials are going to be used.
        /// </summary>
        [DataMember(Name = "username")]
        public string Username
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the password provided in the request.
        /// </summary>
        [DataMember(Name = "password")]
        public string Password
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordCredentialsAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="clientId">The id of the client making the request.</param>
        /// <param name="clientSecret">The secret of the client making the request.</param>
        /// <param name="grantType">The type of grant requested by the client.</param>
        /// <param name="scope">The scope requested by the client.</param>
        /// <param name="redirectUri">The redirect URI provided by the client when requesting an authorization code.</param>
        public PasswordCredentialsAccessTokenRequest(string username, string password, string clientId, string clientSecret, string grantType, Uri redirectUri, string scope)
            : base(clientId, clientSecret, grantType, scope, redirectUri)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(!string.IsNullOrEmpty(clientId));
            Contract.Requires(!string.IsNullOrEmpty(clientSecret));
            Contract.Requires(!string.IsNullOrEmpty(grantType));
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.Username = username;
            this.Password = password;
            this.GrantType = grantType;
            this.RedirectUri = redirectUri;
            this.Scope = scope;
        }
    }
}
