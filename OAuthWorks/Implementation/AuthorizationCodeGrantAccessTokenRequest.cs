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
    public class AuthorizationCodeGrantAccessTokenRequest : IAuthorizationCodeGrantAccessTokenRequest
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
        /// Gets the grant type as requested by the client.
        /// </summary>
        [DataMember(Name = "grant_type")]
        public string GrantType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Id of the client.
        /// </summary>
        [DataMember(Name = "client_id")]
        public string ClientId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the redirect uri that was provided in getting the authorization code.
        /// </summary>
        [DataMember(Name = "redirect_uri")]
        public Uri RedirectUri
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the secret (password) that was provided by the client.
        /// </summary>
        [DataMember(Name = "client_secret")]
        public string ClientSecret
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the scope that is requested by the client.
        /// </summary>
        [DataMember(Name = "scope")]
        public string Scope
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeGrantAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="authorizationCode">The authorization code.</param>
        /// <param name="clientId">The identifier of the client that's requesting an access token.</param>
        /// <param name="clientSecret">The secret/password of the client.</param>
        /// <param name="redirectUri">The URI that the user should be redirected to.</param>
        public AuthorizationCodeGrantAccessTokenRequest(string authorizationCode, string clientId, string clientSecret, Uri redirectUri)
        {
            Contract.Requires(!string.IsNullOrEmpty(authorizationCode));
            Contract.Requires(!string.IsNullOrEmpty(clientId));
            Contract.Requires(!string.IsNullOrEmpty(clientSecret));
            this.AuthorizationCode = authorizationCode;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.RedirectUri = redirectUri;
        }
    }
}
