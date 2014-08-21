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
    public class UnsuccessfulAuthorizationCodeResponse : AuthorizationCodeResponse, IUnsuccessfulAuthorizationCodeResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse" /> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="request">The request that led to this response.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestSpecificErrorType errorCode, Uri redirectUri, IProcessedAuthorizationCodeRequest request)
            : this(errorCode, redirectUri, null, null, request)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse" /> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="errorDescription">The human-readable description of the error.</param>
        /// <param name="request">The request that led to this response.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestSpecificErrorType errorCode, Uri redirectUri, string errorDescription, IProcessedAuthorizationCodeRequest request)
            : this(errorCode, redirectUri, errorDescription, null, request)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse" /> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="errorDescription">The human-readable description of the error.</param>
        /// <param name="errorUri">A URI that points to a human-readable web page that describes the error.</param>
        /// <param name="request">The request that led to this response.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestSpecificErrorType errorCode, Uri redirectUri, string errorDescription, Uri errorUri, IProcessedAuthorizationCodeRequest request)
            : base(request)
        { 
            this.SpecificErrorCode = errorCode;
            this.ErrorCode = SpecificErrorCode.GetSubgroup<AuthorizationCodeRequestErrorType>();
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
            if (redirectUri != null)
            {
                this.Redirect = new Uri(redirectUri, new
                {
                    error = ErrorCode,
                    state = State,
                    error_description = ErrorDescription,
                    error_uri = ErrorUri
                }.ToQueryString());
            }
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
        public override bool IsSuccessful
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the sepecific error that occured, providing more information about what happened.
        /// </summary>
        /// <returns>Returns a <see cref="AuthorizationCodeRequestSpecificErrorType" /> object that represents the problem that occured.</returns>
        [DataMember(Name = "specific_error_code")]
        public AuthorizationCodeRequestSpecificErrorType SpecificErrorCode
        {
            get;
            private set;
        }
    }
}
