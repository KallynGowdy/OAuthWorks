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

using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for an object that defines that an error/exception occurred in a client's access refreshToken request.
    /// </summary>
    public interface IAccessTokenResponseError
    {
        /// <summary>
        /// Gets the error code which defines what happened in the request.
        /// </summary>
        AccessTokenRequestError ErrorCode
        {
            get;
        }

        /// <summary>
        /// Gets the basic description of the error that contains sugesstions on how the client developer should fix the problem.
        /// </summary>
        string ErrorDescription
        {
            get;
        }

        /// <summary>
        /// Gets a URI that points to a web page that the developer can visit to find information about the error.
        /// </summary>
        Uri ErrorUri
        {
            get;
        }
    }
}
