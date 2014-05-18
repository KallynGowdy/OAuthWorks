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

using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Factories
{
    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.Factories.IAuthorizationCodeResponseFactory"/>.
    /// </summary>
    public class AuthorizationCodeResponseFactory : IAuthorizationCodeResponseFactory
    {
        /// <summary>
        /// Creates a new <see cref="OAuthWorks.ISuccessfulAuthorizationCodeResponse" /> object given the authorization code and state.
        /// </summary>
        /// <param name="authorizationCode">The authorization code that is granted to the client.</param>
        /// <param name="state">The state that was provided by the client to prevent Cross Site Request Forgery.</param>
        /// <returns>
        /// Returns a new <see cref="OAuthWorks.ISuccessfulAuthorizationCodeResponse" /> object that contains the given values.
        /// </returns>
        public ISuccessfulAuthorizationCodeResponse Create(string authorizationCode, string state)
        {
            return new SuccessfulAuthorizationCodeResponse(authorizationCode, state);
        }

        /// <summary>
        /// Creates a new <see cref="IUnsuccessfulAuthorizationCodeResponse" /> object using the given information.
        /// </summary>
        /// <param name="errorCode">The error code that describes the basic problem that occurred. (Required)</param>
        /// <param name="errorDescription">A human-readable string that describes what went wrong. (Optional)</param>
        /// <param name="errorUri">A URI that points to a human-readable web page that contains information about the error. (Optional)</param>
        /// <param name="state">The state that was provided by the client in the request. (Required if client provided state)</param>
        /// <param name="innerException">The exception that caused this error to occur.</param>
        /// <returns>
        /// Returns a new <see cref="IUnsuccessfulAuthorizationCodeResponse" /> object.
        /// </returns>
        public IUnsuccessfulAuthorizationCodeResponse CreateError(AuthorizationCodeRequestErrorType errorCode, string errorDescription, Uri errorUri, string state, Exception innerException)
        {
            return new UnsuccessfulAuthorizationCodeResponse(errorCode, state, errorDescription, errorUri);
        }

        public ISuccessfulAuthorizationCodeResponse Create()
        {
            return null;
        }
    }
}
