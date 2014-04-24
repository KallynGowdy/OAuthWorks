﻿// Copyright 2014 Kallyn Gowdy
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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Factories
{
    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.Factories.IAccessTokenFactory{T}"/>.
    /// </summary>
    /// <remarks>
    /// This factory produces <see cref="OAuthWorks.Implementation.HashedAccessToken"/> objects.
    /// </remarks>
    public class AccessTokenFactory : IAccessTokenFactory<HashedAccessToken>
    {
        /// <summary>
        /// The default length of tokens (in bytes) that are generated.
        /// </summary>
        /// <value>28</value>
        public const int DefaultTokenLength = 28;

        /// <summary>
        /// The default lifetime of tokens generated in seconds.
        /// </summary>
        /// <value>3600 or 1 hour.</value>
        public const int DefaultTokenLifetime = 3600;

        /// <summary>
        /// Gets the length (in bytes) of tokens that are generated with this factory.
        /// </summary>
        public int TokenLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of seconds that tokens generated with this factory are live for.
        /// </summary>
        public int TokenLifetime
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        public AccessTokenFactory() : this(DefaultTokenLength, DefaultTokenLifetime)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        /// <param name="tokenLength">The Length of the tokens (in bytes) that are generated from this factory.</param>
        /// <param name="tokenLifetime">The lifetime of the generated tokens in seconds.</param>
        public AccessTokenFactory(int tokenLength, int tokenLifetime)
        {
            Contract.Requires(tokenLength >= 10);
            Contract.Requires(tokenLifetime >= 1);
            this.TokenLength = tokenLength;
            this.TokenLifetime = tokenLifetime;
        }

        public ICreatedToken<HashedAccessToken> Create(IClient client, IUser user, IEnumerable<IScope> scopes)
        {
            string token = GenerateToken(TokenLength);
            string id = token.Substring(0, 8);
            return new CreatedToken<HashedAccessToken>(new HashedAccessToken(token, id, user, client, scopes, "bearer", DateTime.UtcNow.AddSeconds(TokenLifetime)), token);
        }

        public HashedAccessToken Create()
        {
            return null;
        }

        /// <summary>
        /// Generates a random set of bytes and returns them as Base64 encoded.
        /// </summary>
        /// <param name="length">The number of bytes to generate.</param>
        /// <returns>Returns a base64 encoded version of the generated bytes.</returns>
        public static string GenerateToken(int length)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] b = new byte[length];
                rng.GetBytes(b);
                return Convert.ToBase64String(b);
            }
        }
    }
}
