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
    /// Defines a class that provides a basic implementation of <see cref="IAuthorizationCodeGrantAccessTokenRequest"/>.
    /// </summary>
    [DataContract]
    public class AuthorizationCodeGrantAccessTokenRequest : AccessTokenRequest, IAuthorizationCodeGrantAccessTokenRequest
    {
        /// <summary>
        /// Gets the authorization that was given to the client.
        /// </summary>
        [DataMember(Name = "code")]
        public string AuthorizationCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeGrantAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="authorizationCode">The authorization code.</param>
        /// <param name="clientId">The id of the client making the request.</param>
        /// <param name="clientSecret">The secret of the client making the request.</param>
        /// <param name="scope">The scope requested by the client.</param>
        /// <param name="redirectUri">The redirect URI provided by the client when requesting an authorization code.</param>
        public AuthorizationCodeGrantAccessTokenRequest(string authorizationCode, string clientId, string clientSecret, Uri redirectUri)
            : base(clientId, clientSecret, "access_token", null, redirectUri)
        {
            Contract.Requires(!string.IsNullOrEmpty(authorizationCode));
            this.AuthorizationCode = authorizationCode;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.RedirectUri = redirectUri;
        }
    }
}
