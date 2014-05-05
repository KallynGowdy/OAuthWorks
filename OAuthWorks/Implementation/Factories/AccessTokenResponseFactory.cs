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
using OAuthWorks.Factories;

namespace OAuthWorks.Implementation.Factories
{
    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="IAccessTokenResponseFactory"/>.
    /// </summary>
    public class AccessTokenResponseFactory : IAccessTokenResponseFactory<AccessTokenResponse, AccessTokenResponseException>
    {
        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessTokenResponse"/> object given the distributed access refreshToken, refresh refreshToken, access refreshToken type, granted scope, and expiration date.
        /// </summary>
        /// <param name="accessToken">The access refreshToken that grants access to the resources governed by the scope.</param>
        /// <param name="refreshToken">The refresh refreshToken that allows retrieval of additional access tokens.</param>
        /// <param name="tokenType">The type of refreshToken that is returned.</param>
        /// <param name="scope"></param>
        /// <param name="expirationDateUtc"></param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object.</returns>
        public AccessTokenResponse Create(string accessToken, string refreshToken, string tokenType, string scope, DateTime expirationDateUtc)
        {
            return new AccessTokenResponse(accessToken, expirationDateUtc, scope, tokenType, refreshToken);
        }

        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessTokenResponse"/> object used for error responsed to the client given the error type, error description and error description uri.
        /// </summary>
        /// <param name="errorCode">The error code that describes the basic problem.</param>
        /// <param name="errorDescription">A more in-depth description of the error. May be null.</param>
        /// <param name="errorUri">A uri where a developer can go to find information about the error.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object.</returns>
        public AccessTokenResponseException CreateError(AccessTokenRequestError errorCode, string errorDescription, Uri errorUri, Exception innerException)
        {
            return new AccessTokenResponseException(errorCode, errorDescription, errorUri, innerException);
        }

        /// <summary>
        /// Gets the default object of the type T.
        /// </summary>
        /// <remarks>
        /// This method should not be confused with 
        /// <code>
        /// default(T);
        /// </code>
        /// It returns the default object as defined by the factory, which is up to the creator of the factory.
        /// Acceptable responses are null, an non-null empty object or an object that is fully initalized based on whatever the 
        /// implementation defines. Therefore beware when calling this method without proper documentation on the specific behaviour.
        /// </remarks>
        /// <returns>Returns the default value for objects of the type T.</returns>
        public AccessTokenResponse Create()
        {
            return null;
        }
    }
}
