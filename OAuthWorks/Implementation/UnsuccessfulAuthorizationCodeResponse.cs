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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class which provides a basic implementation of <see cref="IUnsuccessfulAuthorizationCodeResponse"/>.
    /// </summary>
    [DataContract]
    public class UnsuccessfulAuthorizationCodeResponse : IUnsuccessfulAuthorizationCodeResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestErrorType errorCode, string state, Uri redirectUri)
            : this(errorCode, state, redirectUri, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="errorDescription">The human-readable description of the error.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestErrorType errorCode, string state, Uri redirectUri, string errorDescription)
            : this(errorCode, state, redirectUri, errorDescription, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="errorDescription">The human-readable description of the error.</param>
        /// <param name="errorUri">A URI that points to a human-readable web page that describes the error.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestErrorType errorCode, string state, Uri redirectUri, string errorDescription, Uri errorUri)
        {
            this.ErrorCode = errorCode;
            this.State = state;
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
            if (redirectUri != null)
                this.Redirect = new Uri(redirectUri, new { error = errorCode, state = state, error_description = errorDescription, error_uri = errorUri }.ToQueryString());
            else
            {
                this.Redirect = null;
            }
        }

        /// <summary>
        /// Gets the type of the error that provides information on the basic problem that occured.
        /// </summary>
        [DataMember(Name = "error_code")]
        public AuthorizationCodeRequestErrorType ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the human-readable ASCII text that provides additional information that is used to assis the client developer
        /// in understanding the error that occurred.
        /// </summary>
        [DataMember(Name = "error_description")]
        public string ErrorDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a URI that identifies a human-readable web page with information about the error, used to provide the client developer with additional information about
        /// the error.
        /// </summary>
        [DataMember(Name = "error_uri")]
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
        /// Gets the 
        /// <see cref="Uri" /> that specifies where the user should be redirected to. 
        /// This value should contain all of the values needed for a successful OAuth 2.0 authorization code redirect. (Section 4.1.2 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.1.2)
        /// </summary>
        /// <returns></returns>
        public Uri Redirect
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the state that was provided by the client in the incomming Authorization Request. REQUIRED ONLY IF the state was provided in the request.
        /// </summary>
        [DataMember(Name = "state")]
        public string State
        {
            get;
            private set;
        }
    }
}
