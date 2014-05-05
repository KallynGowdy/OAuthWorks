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
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.ITokenRefreshRequest"/>.
    /// </summary>
    [DataContract]
    public class TokenRefreshRequest : AccessTokenRequest, ITokenRefreshRequest
    {
        [DataMember(Name = "refresh_token")]
        public string RefreshToken
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenRefreshRequest"/> class.
        /// </summary>
        /// <param name="refreshToken">The refresh refreshToken.</param>
        /// <param name="clientId">The id of the client making the request.</param>
        /// <param name="clientSecret">The secret of the client making the request.</param>
        /// <param name="grantType">The type of grant requested by the client.</param>
        /// <param name="scope">The scope requested by the client.</param>
        public TokenRefreshRequest(string refreshToken, string clientId, string clientSecret, string grantType, string scope)
            : base(clientId, clientSecret, grantType, scope, null)
        {
            Contract.Requires(!string.IsNullOrEmpty(refreshToken));
            RefreshToken = refreshToken;
        }
    }
}
