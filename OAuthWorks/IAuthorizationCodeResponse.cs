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
    /// Defines an interface that serves as a base for responses to both successful and unsuccessful requests for an authorization code.
    /// </summary>
    public interface IAuthorizationCodeResponse
    {
        /// <summary>
        /// Gets whether the request for an authorization code was successful.
        /// </summary>
        /// <returns>Returns whether the request was successful.</returns>
        bool IsSuccessful
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Uri"/> that specifies where the user should be redirected to. 
        /// This value should contain all of the values needed for a successful OAuth 2.0 authorization code redirect. (<a href="http://tools.ietf.org/html/rfc6749#section-4.1.2">Section 4.1.2 [RFC 6749]</a>)
        /// </summary>
        /// <returns>Returns the <see cref="Uri"/> that the user should be redirected to. Reuturns null if the user should not be redirected for security reasons.</returns>
        Uri Redirect
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IProcessedAuthorizationCodeRequest"/> that led to this response.
        /// </summary>
        /// <remarks>
        /// When implementing, make sure to mark this property as not serializable to prevent any information from being leaked.
        /// </remarks>
        /// <returns>Returns a <see cref="IProcessedAuthorizationCodeRequest"/> object.</returns>
        IProcessedAuthorizationCodeRequest Request
        {
            get;
        }

        /// <summary>
        /// Gets the state that was provided by the client in the incomming Authorization Request. REQUIRED ONLY IF the state was provided in the request.
        /// </summary>
        string State
        {
            get;
        }
    }
}
