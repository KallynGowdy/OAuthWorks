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

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class which provides a basic implemenatation of <see cref="OAuthWorks.IAccessTokenResponseError"/>.
    /// </summary>
    public class AccessTokenResponseError : IAccessTokenResponseError
    {
        /// <summary>
        /// Gets the error code which defines what happened in the request.
        /// </summary>
        public AccessTokenRequestError ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the basic description of the error that contains sugesstions on how the client developer should fix the problem.
        /// </summary>
        public string ErrorDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a URI that points to a web page that the developer can visit to find information about the error.
        /// </summary>
        public Uri ErrorUri
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenResponseError"/> class.
        /// </summary>
        /// <param name="errorCode">The code which describes what happened.</param>
        /// <param name="errorDescription">The human readable description of the error.</param>
        /// <param name="errorUri">The URI of a web page which describes the error.</param>
        public AccessTokenResponseError(AccessTokenRequestError errorCode, string errorDescription, Uri errorUri)
        {
            this.ErrorCode = errorCode;
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
        }
    }
}
