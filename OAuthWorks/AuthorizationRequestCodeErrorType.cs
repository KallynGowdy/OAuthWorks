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
    /// Defines an enum that contains a list of error codes as defined in RFC 6749, Section 4.1.2.1, http://tools.ietf.org/html/rfc6749#section-4.1.2.1.
    /// </summary>
    public enum AuthorizationCodeRequestErrorType
    {
        /// <summary>
        /// Defines that the request is missing a required parameter, includes an invalid parameter value, includes a parameter more than once,
        /// or is otherwise malformed.
        /// </summary>
        InvalidRequest,

        /// <summary>
        /// Defines that the client is not authorized to request an authorization
        /// code using this method.
        /// </summary>
        UnauthorizedClient,

        /// <summary>
        /// Defines that the resource owner or authorization server denied the request.
        /// </summary>
        AccessDenied,

        /// <summary>
        /// Defines that the authorization server does not support obtaining an authorization code using this method.
        /// </summary>
        UnsupportedResponseType,

        /// <summary>
        /// Defines that the requested scope is invalid, unknown, or malformed.
        /// </summary>
        InvalidScope,

        /// <summary>
        /// The authorization server encountered an unexpected condition that prevented it from fufilling the request.
        /// </summary>
        ServerError,

        /// <summary>
        /// Defines that the authorization server is currently unable to handle the request due to a temporary overloading or maintenance of the server.
        /// </summary>
        TemporarilyUnavailable
    }
}
