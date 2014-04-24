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
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for objects that store information about an Access Token Request using the Authorization Code Grant OAuth 2.0 flow (See Section 4.1 [RFC 46749] http://tools.ietf.org/html/rfc6749#section-4.1).
    /// </summary>
    public interface IAuthorizationCodeGrantAccessTokenRequest : IAccessTokenRequest
    {
        /// <summary>
        /// Gets the authorization that was given to the client.
        /// </summary>
        string AuthorizationCode
        {
            get;
        }
    }
}
