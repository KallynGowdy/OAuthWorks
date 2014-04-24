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

namespace OAuthWorks
{
    /// <summary>
    /// Defines an abstract class for an IAuthorizationCodeResponseError.
    /// </summary>
    [Serializable]
    public abstract class AuthorizationCodeResponseExceptionBase : Exception, IAuthorizationCodeResponseError
    {
        /// <summary>
        /// Creates a new AuthorizationCodeResponseException object using the given message and the given inner exception.
        /// </summary>
        /// <param name="message">A string describing what caused the exception to occur.</param>
        /// <param name="innerException">An exception that occurred that caused this exception to occur. Can be null.</param>
        protected AuthorizationCodeResponseExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AuthorizationCodeResponseExceptionBase(string message) : base(message) { }

        public abstract AuthorizationRequestCodeErrorType ErrorCode
        {
            get;
            protected set;
        }

        public abstract string ErrorDescription
        {
            get;
            protected set;
        }

        public abstract Uri ErrorUri
        {
            get;
            protected set;
        }

        public abstract string State
        {
            get;
            protected set;
        }
    }
}
