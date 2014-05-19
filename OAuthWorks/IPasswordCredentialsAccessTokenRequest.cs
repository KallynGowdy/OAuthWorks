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
    /// Defines an interface for objects which contain values concerning a request for an Access Token using the Resource Owner Password Credentials Grant.
    /// </summary>
    public interface IPasswordCredentialsAccessTokenRequest : IAccessTokenRequest
    {
        /// <summary>
        /// Gets the identifier of the user who's credentials are going to be used.
        /// </summary>
        string UserName
        {
            get;
        }

        /// <summary>
        /// Gets or sets the password provided in the request.
        /// </summary>
        string Password
        {
            get;
        }
    }
}
