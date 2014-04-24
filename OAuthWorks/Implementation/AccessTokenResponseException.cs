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
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.AccessTokenResponseExceptionBase"/>.
    /// </summary>
    public class AccessTokenResponseException : AccessTokenResponseExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenResponseException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorDescription">The error description.</param>
        /// <param name="errorUri">The error URI.</param>
        public AccessTokenResponseException(AccessTokenRequestError errorCode, string errorDescription, Uri errorUri)
            : this(errorCode, errorDescription, errorUri, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenResponseException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorDescription">The error description.</param>
        /// <param name="errorUri">The error URI.</param>
        /// <param name="innerException">The exception that caused the error to occur.</param>
        public AccessTokenResponseException(AccessTokenRequestError errorCode, string errorDescription, Uri errorUri, Exception innerException) : base(errorDescription, innerException)
        {
            this.errorCode = errorCode;
            this.errorDescription = errorDescription;
            this.errorUri = errorUri;
        }

        private AccessTokenRequestError errorCode;
        private string errorDescription;
        private Uri errorUri;

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public override AccessTokenRequestError ErrorCode
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
        public override string ErrorDescription
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
        public override Uri ErrorUri
        {
            get
            {
                return this.errorUri;
            }
        }
    }
}
