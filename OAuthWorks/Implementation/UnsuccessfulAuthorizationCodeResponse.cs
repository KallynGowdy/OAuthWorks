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
    /// Defines a class which provides a basic implementation of <see cref="IUnsuccessfulAuthorizationCodeResponse"/>.
    /// </summary>
    public class UnsuccessfulAuthorizationCodeResponse : IUnsuccessfulAuthorizationCodeResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestErrorType errorCode, string state)
            : this(errorCode, state, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="errorDescription">The human-readable description of the error.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestErrorType errorCode, string state, string errorDescription)
            : this(errorCode, state, errorDescription, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="errorDescription">The human-readable description of the error.</param>
        /// <param name="errorUri">A URI that points to a human-readable web page that describes the error.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestErrorType errorCode, string state, string errorDescription, Uri errorUri)
        {
            this.ErrorCode = errorCode;
            this.State = state;
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
        }

        /// <summary>
        /// Gets the type of the error that provides information on the basic problem that occured.
        /// </summary>
        public AuthorizationCodeRequestErrorType ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the human-readable ASCII text that provides additional information that is used to assis the client developer
        /// in understanding the error that occurred.
        /// </summary>
        public string ErrorDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a URI that identifies a human-readable web page with information about the error, used to provide the client developer with additional information about
        /// the error.
        /// </summary>
        public Uri ErrorUri
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether the request for an authorization code was successful.
        /// </summary>
        /// <value>false</value>
        /// <returns>Returns whether the request was successful.</returns>
        public bool IsSuccessful
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the state that was provided by the client in the incomming Authorization Request. REQUIRED ONLY IF the state was provided in the request.
        /// </summary>
        public string State
        {
            get;
            private set;
        }
    }
}
