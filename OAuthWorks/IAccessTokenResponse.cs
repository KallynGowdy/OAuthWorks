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
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for an object that contains values that should be returned in response to a client's access token request.
    /// </summary>
    public interface IAccessTokenResponse
    {
        /// <summary>
        /// Gets the access token that provides access to certian scopes.
        /// </summary>
        string AccessToken
        {
            get;
        }

        /// <summary>
        /// Gets the type of the token as required by RCF 6749 Section 7.1 (http://tools.ietf.org/html/rfc6749#section-7.1)
        /// </summary>
        string TokenType
        {
            get;
        }

        /// <summary>
        /// Gets the scope of the access that is granted by the access token.
        /// </summary>
        string Scope
        {
            get;
        }

        /// <summary>
        /// Gets the refresh token that is used to retrieve new access tokens without user interaction.
        /// </summary>
        string RefreshToken
        {
            get;
        }

        /// <summary>
        /// Gets the lifetime of the access token in seconds.
        /// </summary>
        int ExpiresIn
        {
            get;
        }

        /// <summary>
        /// Gets the date of expiration in universal coordinated time.
        /// </summary>
        DateTime ExpirationDateUtc
        {
            get;
        }
    }
}
