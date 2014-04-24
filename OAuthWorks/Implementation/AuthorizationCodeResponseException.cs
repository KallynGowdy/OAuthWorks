// Copyright 2014 Kallyn gowdy
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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides a basic implemenatation of <see cref="IAuthorizationCodeResponseError"/>.
    /// </summary>
    [DataContract]
    public class AuthorizationCodeResponseException : AuthorizationCodeResponseExceptionBase
    {
        /// <summary>
        /// Gets the type of the error that provides information on the basic problem that occured.
        /// </summary>
        [DataMember(Name = "error_code")]
        public override AuthorizationRequestCodeErrorType ErrorCode
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the human-readable ASCII text that provides additional information that is used to assis the client developer
        /// in understanding the error that occurred.
        /// </summary>
        [DataMember(Name = "error_description")]
        public override string ErrorDescription
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a URI that identifies a human-readable web page with information about the error, used to provide the client developer with additional information about
        /// the error.
        /// </summary>
        [DataMember(Name = "error_uri")]
        public override Uri ErrorUri
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the state that was provided by the client in the incomming Authorization Request. REQUIRED ONLY IF the state was provided in the request.
        /// </summary>
        [DataMember(Name = "state")]
        public override string State
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeResponseError"/> class.
        /// </summary>
        /// <param name="errorCode">The error code which describes the basic problem.</param>
        /// <param name="state">The state that was provided by the client during the request.</param>
        /// <param name="errorDescription">The human readable error description.</param>
        /// <param name="errorUri">A uri that points to a web page which contains information about the error.</param>
        public AuthorizationCodeResponseException(AuthorizationRequestCodeErrorType errorCode, string state, string errorDescription, Uri errorUri)
            : base(errorDescription)
        {
            Contract.Requires(!string.IsNullOrEmpty(state));
            this.ErrorCode = errorCode;
            this.State = state;
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeResponseError"/> class.
        /// </summary>
        /// <param name="errorCode">The error code which describes the basic problem.</param>
        /// <param name="state">The state that was provided by the client during the request.</param>
        /// <param name="errorDescription">The human readable error description.</param>
        /// <param name="errorUri">A uri that points to a web page which contains information about the error.</param>
        public AuthorizationCodeResponseException(AuthorizationRequestCodeErrorType errorCode, string state, string errorDescription, Uri errorUri, Exception causingException)
            : base(errorDescription, causingException)
        {
            Contract.Requires(!string.IsNullOrEmpty(state));
            this.ErrorCode = errorCode;
            this.State = state;
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
        }
    }
}
