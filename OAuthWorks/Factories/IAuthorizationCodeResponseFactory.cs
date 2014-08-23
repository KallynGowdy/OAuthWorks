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

namespace OAuthWorks.Factories
{
    /// <summary>
    /// Defines an interface for a factory that creates <see cref="OAuthWorks.IAuthorizationCodeResponse"/> objects.
    /// </summary>
    public interface IAuthorizationCodeResponseFactory : IFactory<ISuccessfulAuthorizationCodeResponse>
    {
        /// <summary>
        /// Creates a new <see cref="OAuthWorks.ISuccessfulAuthorizationCodeResponse" /> object given the authorization code and state.
        /// </summary>
        /// <param name="authorizationCode">The authorization code that is granted to the client.</param>
        /// <param name="redirect">The <see cref="Uri" /> provided by the client that the user should be redirected to.</param>
        /// <param name="request">The request that led to this response.</param>
        /// <returns>
        /// Returns a new <see cref="OAuthWorks.ISuccessfulAuthorizationCodeResponse" /> object that contains the given values.
        /// </returns>
        ISuccessfulAuthorizationCodeResponse Create(string authorizationCode, Uri redirect, IProcessedAuthorizationCodeRequest request);

        /// <summary>
        /// Creates a new <see cref="IUnsuccessfulAuthorizationCodeResponse" /> object using the given information.
        /// </summary>
        /// <param name="errorCode">The error code that describes the problem that occurred. (Required)</param>
        /// <param name="request">The <see cref="IAuthorizationCodeRequest" /> that led to the response. (Required)</param>
        /// <param name="errorDescription">A human-readable string that describes what went wrong. (Optional)</param>
        /// <param name="errorUri">A URI that points to a human-readable web page that contains information about the error. (Optional)</param>
        /// <param name="redirectUri">The <see cref="Uri" /> provided by the client that the user should be redirected to.</param>
        /// <param name="innerException">The exception that caused this error to occur.</param>
        /// <returns>
        /// Returns a new <see cref="IUnsuccessfulAuthorizationCodeResponse" /> object.
        /// </returns>
        IUnsuccessfulAuthorizationCodeResponse CreateError(AuthorizationCodeRequestSpecificErrorType errorCode, IProcessedAuthorizationCodeRequest request, string errorDescription, Uri errorUri, Uri redirectUri, Exception innerException);
    }
}
