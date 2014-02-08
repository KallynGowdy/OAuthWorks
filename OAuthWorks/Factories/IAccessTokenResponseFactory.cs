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
    /// Defines an interface for a <see cref="OAuthWorks.IFactory"/> object that produces <see cref="OAuthWorks.IAccessTokenResponse"/> objects.
    /// </summary>
    public interface IAccessTokenResponseFactory<out TAccessTokenResponse, out TResponseException> :
        IFactory<TAccessTokenResponse>
        where TAccessTokenResponse : IAccessTokenResponse
        where TResponseException : Exception, IAccessTokenResponseError
    {
        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessTokenResponse"/> object given the distributed access token, refresh token, access token type, granted scope, and expiration date.
        /// </summary>
        /// <param name="accessToken">The access token that grants access to the resources governed by the scope.</param>
        /// <param name="refreshToken">The refresh token that allows retrieval of additional access tokens.</param>
        /// <param name="tokenType">The type of token that is returned.</param>
        /// <param name="scope"></param>
        /// <param name="expirationDateUtc"></param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object.</returns>
        TAccessTokenResponse Create(string accessToken, string refreshToken, string tokenType, string scope, DateTime expirationDateUtc);

        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessTokenResponse"/> object used for error responsed to the client given the error type, error description and error description uri.
        /// </summary>
        /// <param name="errorCode">The error code that describes the basic problem.</param>
        /// <param name="errorDescription">A more in-depth description of the error. May be null.</param>
        /// <param name="errorUri">A uri where a developer can go to find information about the error.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object.</returns>
        TResponseException CreateError(AccessTokenRequestError errorCode, string errorDescription, Uri errorUri, Exception innerException);
    }
}
