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

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.IAuthorizationCodeResponse"/>.
    /// </summary>
    [DataContract]
    public class SuccessfulAuthorizationCodeResponse : ISuccessfulAuthorizationCodeResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessfulAuthorizationCodeResponse"/> class.
        /// </summary>
        /// <param name="code">The code that should be returned to the client.</param>
        /// <param name="state">The state that should be returned to the client.</param>
        /// <exception cref="System.ArgumentException">The given code must not be null or empty.;code</exception>
        public SuccessfulAuthorizationCodeResponse(string code, string state, Uri redirectUri)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("The given code must not be null or empty.", "code");
            }
            if(redirectUri == null)
            {
                throw new ArgumentNullException("redirectUri");
            }
            this.Code = code;
            this.State = state;
            this.Redirect = new Uri(redirectUri, string.Format("?code={0}&state={1}", Uri.EscapeDataString(code), Uri.EscapeDataString(state)));
        }

        /// <summary>
        /// Gets the code that was generated by the authorization server. REQUIRED.
        /// </summary>
        [DataMember(Name = "code")]
        public string Code
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether the request for an authorization code was successful.
        /// </summary>
        /// <returns>Returns whether the request was successful.</returns>
        public bool IsSuccessful
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the 
        /// <see cref="Uri" /> that specifies where the user should be redirected to. 
        /// This value should contain all of the values needed for a successful OAuth 2.0 authorization code redirect. (Section 4.1.2 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.1.2)
        /// </summary>
        /// <returns></returns>
        public Uri Redirect
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the state that was sent by the client in the Authorization Request. REQUIRED ONLY IF the state was sent in the request.
        /// </summary>
        [DataMember(Name = "state")]
        public string State
        {
            get;
            private set;
        }
    }
}
