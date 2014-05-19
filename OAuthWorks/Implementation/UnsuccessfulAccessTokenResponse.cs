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
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.AccessTokenResponseExceptionBase"/>.
    /// </summary>
    [DataContract]
    public class UnsuccessfulAccessTokenResponse : IUnsuccessfulAccessTokenResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAccessTokenResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorDescription">The error description.</param>
        /// <param name="errorUri">The error URI.</param>
        public UnsuccessfulAccessTokenResponse(AccessTokenRequestError errorCode, string errorDescription, Uri errorUri)
            : this(errorCode, errorDescription, errorUri, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAccessTokenResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorDescription">The error description.</param>
        /// <param name="errorUri">The error URI.</param>
        /// <param name="innerException">The exception that caused the error to occur.</param>
        public UnsuccessfulAccessTokenResponse(AccessTokenRequestError errorCode, string errorDescription, Uri errorUri, Exception innerException)
        {
            this.errorCode = errorCode;
            this.errorDescription = errorDescription;
            this.errorUri = errorUri;
            this.innerException = innerException;
        }

        [DataMember(Name = "error_code")]
        private readonly AccessTokenRequestError errorCode;

        /// <summary>
        /// Gets the string representation of <see cref="UnsuccessfulAccessTokenResponse.errorCode"/>.
        /// This provides a hack for <see cref="DataContractAttribute"/> serializers by causing them to produce the string representation of the error code.
        /// </summary>
        /// <returns></returns>
        
        private string ErrorCodeDataRepresentation
        {
            get
            {
                return errorCode.ToString();
            }
        }

        [DataMember(Name ="error_description")]
        private readonly string errorDescription;

        [DataMember(Name ="error_uri")]
        private readonly Uri errorUri;

        private readonly Exception innerException;

        /// <summary>
        /// Gets the exception that caused this error to occur.
        /// </summary>
        /// <returns></returns>
        public Exception InnerException
        {
            get
            {
                return innerException;
            }
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public AccessTokenRequestError ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        /// <summary>
        /// Gets the human-readable description of the error.
        /// </summary>
        /// <value>
        /// The error description.
        /// </value>
        public string ErrorDescription
        {
            get
            {
                return this.errorDescription;
            }
        }

        /// <summary>
        /// Gets the URI that points to a human-readable web page that describes the error.
        /// </summary>
        /// <value>
        /// The error URI.
        /// </value>
        public Uri ErrorUri
        {
            get
            {
                return this.errorUri;
            }
        }

        /// <summary>
        /// Gets the error code that describes what was wrong with the request.
        /// </summary>
        /// <returns>Returns a <see cref="AccessTokenRequestError" /> object that describes what was wrong with the request.</returns>
        public AccessTokenRequestError Error
        {
            get
            {
                return errorCode;
            }
        }

        /// <summary>
        /// Gets whether the request was successful.
        /// </summary>
        /// <returns>Returns whether the request by the client was successful.</returns>
        public bool IsSuccessful
        {
            get
            {
                return false;
            }
        }
    }
}
