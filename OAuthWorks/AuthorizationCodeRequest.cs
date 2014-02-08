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

namespace OAuthWorks
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
            set;
        }
        

        /// <summary>
        /// Gets or sets the client id that designates who this request is coming from.
        /// </summary>
        [DataMember(Name = "client_id")]
        public int ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets secret that the client provided in the request.
        /// </summary>
        [DataMember(Name = "client_secret")]
        public string ClientSecret
        {
            get;
            set;
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
            set;
        }

        /// <summary>
        /// Gets or sets the scope of the permissions requested by the client.
        /// </summary>
        [DataMember(Name = "scope")]
        public string Scope
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state that was sent by the client.
        /// </summary>
        [DataMember(Name = "state")]
        public string State
        {
            get;
            set;
        }
    }
}
