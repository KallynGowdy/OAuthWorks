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

namespace OAuthWorks
{
    /// <summary>
    /// Defines an abstract exception that provides unification between the System.Exception class and the OAuthWorks.IAccessTokenResponseException interface.
    /// </summary>
    [Serializable]
    public abstract class AccessTokenResponseExceptionBase : Exception, IAccessTokenResponseError
    {
        protected AccessTokenResponseExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected AccessTokenResponseExceptionBase() : base() { }

        protected AccessTokenResponseExceptionBase(string message) : base(message) { }

        protected AccessTokenResponseExceptionBase(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Gets the error code which defines what happened in the request.
        /// </summary>
        public abstract AccessTokenRequestError ErrorCode
        {
            get;
        }

        /// <summary>
        /// Gets the basic description of the error that contains sugesstions on how the client developer should fix the problem.
        /// </summary>
        public abstract string ErrorDescription
        {
            get;
        }

        /// <summary>
        /// Gets a URI that points to a web page that the developer can visit to find information about the error.
        /// </summary>
        public abstract Uri ErrorUri
        {
            get;
        }
    }
}
