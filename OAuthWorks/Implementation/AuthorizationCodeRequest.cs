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
    /// Defines an implementation of <see cref="OAuthWorks.IAuthorizationCodeRequest"/> that is serializable and deserializable acording to the OAuth 2.0 spec.
    /// </summary>
    [DataContract]
    public class AuthorizationCodeRequest : IAuthorizationCodeRequest
    {
        /// <summary>
        /// Gets the type of response that is expected from the provider.
        /// </summary>
        [DataMember(Name = "response_type")]
        public AuthorizationCodeResponseType ResponseType
        {
            get;
            private set;
        }
        

        /// <summary>
        /// Gets or sets the client id that designates who this request is coming from.
        /// </summary>
        [DataMember(Name = "client_id")]
        public string ClientId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets secret that the client provided in the request.
        /// </summary>
        [DataMember(Name = "client_secret")]
        public string ClientSecret
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the client Id that designates who this request is coming from.
        /// </summary>
        string IAuthorizationCodeRequest.ClientId
        {
            get
            {
                return this.ClientId.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the redirect uri that designates where to send the user after processing the request.
        /// </summary>
        [DataMember(Name = "redirect_uri")]
        public Uri RedirectUri
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the scope of the permissions requested by the client.
        /// </summary>
        [DataMember(Name = "scope")]
        public string Scope
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the state that was sent by the client.
        /// </summary>
        [DataMember(Name = "state")]
        public string State
        {
            get;
            private set;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeRequest"/> class.
        /// </summary>
        /// <param name="clientId">The identifier of the client.</param>
        /// <param name="clientSecret">The secret/password of the client.</param>
        /// <param name="scope">The scope that is requested by the client.</param>
        /// <param name="state">The state that is provided by the client.</param>
        /// <param name="redirectUri">The URI that the user should be redirected to.</param>
        /// <param name="responseType">Type of the response requested by the client.</param>
        public AuthorizationCodeRequest(string clientId, string clientSecret, string scope, string state, Uri redirectUri, AuthorizationCodeResponseType responseType)
        {
            Contract.Requires(!string.IsNullOrEmpty(clientId));
            Contract.Requires(!string.IsNullOrEmpty(clientSecret));
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.Scope = scope;
            this.State = state;
            this.RedirectUri = redirectUri;
            this.ResponseType = responseType;
        }
    }
}
