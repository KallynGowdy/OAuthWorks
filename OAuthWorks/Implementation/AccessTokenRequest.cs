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
    /// Defines a base class that provides an implementation of <see cref="OAuthWorks.IAccessTokenRequest"/>.
    /// </summary>
    [DataContract]
    public abstract class AccessTokenRequest : IAccessTokenRequest
    {
        /// <summary>
        /// Gets the type of the grant that was given to the client.
        /// </summary>
        [DataMember(Name = "grant_type")]
        public string GrantType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the Id of the client.
        /// </summary>
        [DataMember(Name = "client_id")]
        public string ClientId
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the redirect uri that was provided in getting the authorization code.
        /// </summary>
        [DataMember(Name = "redirect_uri")]
        public Uri RedirectUri
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the secret (password) that was provided by the client.
        /// </summary>
        [DataMember(Name = "client_secret")]
        public string ClientSecret
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the scope that is requested by the client.
        /// </summary>
        [DataMember(Name = "scope")]
        public string Scope
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenRequest"/> class.
        /// </summary>
        /// <param name="clientId">The id of the client making the request.</param>
        /// <param name="clientSecret">The secret of the client making the request.</param>
        /// <param name="grantType">The type of grant requested by the client.</param>
        /// <param name="scope">The scope requested by the client.</param>
        /// <param name="redirectUri">The redirect URI provided by the client when requesting an authorization code.</param>
        protected AccessTokenRequest(string clientId, string clientSecret, string grantType, string scope, Uri redirectUri)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.GrantType = grantType;
            this.Scope = scope;
            this.RedirectUri = redirectUri;
        }
    }
}
