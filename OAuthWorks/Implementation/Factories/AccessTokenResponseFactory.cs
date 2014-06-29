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
using NUnit.Framework;
using OAuthWorks.Factories;

namespace OAuthWorks.Implementation.Factories
{
    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="IAccessTokenResponseFactory"/>.
    /// </summary>
    public class AccessTokenResponseFactory : IAccessTokenResponseFactory
    {
        /// <summary>
        /// Defines a class that contains tests for <see cref="AccessTokenResponseFactory"/>.
        /// </summary>
        [TestFixture]
        public class Tests
        {
            [TestCase("dalfhslfa", "asdflahs;", "bearer", "scope")]
            public void TestCreate(string accessToken, string refreshToken, string tokenType, string scope)
            {
                AccessTokenResponseFactory factory = new AccessTokenResponseFactory();
                ISuccessfulAccessTokenResponse response = factory.Create(accessToken, refreshToken, tokenType, scope, DateTime.UtcNow.AddHours(2));
                Assert.That(response, Is.Not.Null);
                Assert.That(response.AccessToken, Is.Not.Null.Or.Empty);
                Assert.That(response.IsSuccessful, Is.True);
                Assert.That(response.TokenType, Is.Not.Null.Or.Empty);
            }
        }

        /// <summary>
        /// Gets a new <see cref="OAuthWorks.ISuccessfulAccessTokenResponse"/> object given the distributed access token, refresh token, access token type, granted scope, and expiration date.
        /// </summary>
        /// <param name="accessToken">The access token that grants access to the resources governed by the scope.</param>
        /// <param name="refreshToken">The refresh token that allows retrieval of additional access tokens.</param>
        /// <param name="tokenType">The type of token that is returned.</param>
        /// <param name="scope">The scope granted to the client.</param>
        /// <param name="expirationDateUtc">The date that the token expires in Universal Coordinated Time (UTC).</param>
        /// <returns>Returns a new <see cref="OAuthWorks.ISuccessfulAccessTokenResponse"/> object.</returns>
        public ISuccessfulAccessTokenResponse Create(string accessToken, string refreshToken, string tokenType, string scope, DateTime expirationDateUtc)
        {
            return new SuccessfulAccessTokenResponse(accessToken, expirationDateUtc, scope, tokenType, refreshToken);
        }

        /// <summary>
        /// Gets a new <see cref="OAuthWorks.ISuccessfulAccessTokenResponse"/> object used for error responsed to the client given the error type, error description and error description uri.
        /// </summary>
        /// <param name="errorCode">The error code that describes the basic problem.</param>
        /// <param name="errorDescription">A more in-depth description of the error. May be null.</param>
        /// <param name="errorUri">A uri where a developer can go to find information about the error.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.ISuccessfulAccessTokenResponse"/> object.</returns>
        public IUnsuccessfulAccessTokenResponse CreateError(AccessTokenRequestError errorCode, string errorDescription, Uri errorUri, Exception innerException)
        {
            return new UnsuccessfulAccessTokenResponse(errorCode, errorDescription, errorUri, innerException);
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
        public ISuccessfulAccessTokenResponse Create()
        {
            return null;
        }
    }
}
