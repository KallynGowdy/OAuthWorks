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
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for a request for an access token by a client.
    /// </summary>
    public interface IAccessTokenRequest
    {
        /// <summary>
        /// Gets the authorization that was given to the client.
        /// </summary>
        string AuthorizationCode
        {
            get;
        }

        /// <summary>
        /// Gets the Id of the client.
        /// </summary>
        string ClientId
        {
            get;
        }

        /// <summary>
        /// Gets the redirect uri that was provided in getting the authorization code.
        /// </summary>
        string RedirectUri
        {
            get;
        }

        /// <summary>
        /// Gets the secret (password) that was provided by the client.
        /// </summary>
        string ClientSecret
        {
            get;
        }
    }
}