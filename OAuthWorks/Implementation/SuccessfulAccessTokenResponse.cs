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
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides a basic implementation of an access refreshToken response. (Section 5.1 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-5.1).
    /// </summary>
    [DataContract]
    public class SuccessfulAccessTokenResponse : ISuccessfulAccessTokenResponse
    {
        /// <summary>
        /// Gets whether the request was successful.
        /// </summary>
        /// <returns>Returns whether the request by the client was successful.</returns>
        public bool IsSuccessful
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the access token that provides access to certian scopes.
        /// </summary>
        [DataMember(Name = "access_token")]
        public string AccessToken
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of the token as required by RCF 6749 Section 7.1 (http://tools.ietf.org/html/rfc6749#section-7.1)
        /// </summary>
        [DataMember(Name = "token_type")]
        public string TokenType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the scope of the access that is granted by the access token.
        /// </summary>
        [DataMember(Name = "scope")]
        public string Scope
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the refresh token that is used to retrieve new access tokens without user interaction.
        /// </summary>
        [DataMember(Name = "refresh_token")]
        public string RefreshToken
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the lifetime of the access token in seconds.
        /// </summary>
        [DataMember(Name = "expires_in")]
        public int ExpiresIn
        {
            get
            {
                return (int)(ExpirationDateUtc - DateTime.UtcNow).TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the date of expiration in universal coordinated time.
        /// </summary>
        [DataMember(Name = "expiration_date_utc")]
        public DateTime ExpirationDateUtc
        {
            get;
            private set;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessfulAccessTokenResponse"/> class.
        /// </summary>
        /// <param name="refreshToken">The token, REQUIRED.</param>
        /// <param name="expirationDateUtc">The expiration date of the token as expressed in Universal Coordinated Time.</param>
        /// <param name="scope">The scope of the token, OPTIONAL.</param>
        /// <param name="tokenType">Type of the token as described by Section 7.1 [RFC 6749] (http://tools.ietf.org/html/rfc6749#section-7.1).</param>
        /// <param name="refreshToken">The refresh token that can be used by the client to obtain new access tokens.</param>
        public SuccessfulAccessTokenResponse(string token, DateTime expirationDateUtc, string scope, string tokenType, string refreshToken)
        {
            Contract.Requires(!string.IsNullOrEmpty(token));
            Contract.Requires(!string.IsNullOrEmpty(tokenType));
            this.AccessToken = token;
            this.ExpirationDateUtc = expirationDateUtc;
            this.Scope = scope;
            this.TokenType = tokenType;
            this.RefreshToken = refreshToken;
        }
    }
}
