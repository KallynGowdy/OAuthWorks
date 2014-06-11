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
    /// Defines a static class that acts as a wrapper for the different default implementations of <see cref="AccessTokenFactory{TId}"/>.
    /// </summary>
    public static class AccessTokenFactory
    {
        /// <summary>
        /// Defines a static class that contains default values for <see cref="AccessTokenFactory{int}"/>.
        /// </summary>
        public static class Int
        {
            /// <summary>
            /// Gets a new <see cref="AccessTokenFactory{int}"/> object that uses all of the default dependencies.
            /// </summary>
            /// <returns>Returns a new <see cref="AccessTokenFactory{int}"/> object.</returns>
            public static AccessTokenFactory<int> DefaultInstance
            {
                get
                {
                    return new AccessTokenFactory<int>(DefaultIdFormatter, DefaultIdGenerator);
                }
            }

            /// <summary>
            /// The default Id value formatter used when creating new <see cref="AccessTokenFactory{int}"/> objects.
            /// </summary>
            public static readonly IValueIdFormatter<int> DefaultIdFormatter = ValueIdFormatter.Int.DefaultFormatter;

            /// <summary>
            /// The default Id generation function used when creating new <see cref="AccessTokenFactory{int}"/> objects.
            /// </summary>
            public static readonly Func<int> DefaultIdGenerator = () =>
            {
                using(RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    byte[] b = new byte[4];
                    rng.GetBytes(b);
                    return BitConverter.ToInt32(b, 0);
                }
            };
        }

        /// <summary>
        /// Defines a static class that 
        /// </summary>
        public static class String
        {
            /// <summary>
            /// The default length (in bytes) for generated Ids.
            /// </summary>
            public const int DefaultIdLength = 8;

            /// <summary>
            /// Gets a new <see cref="AccessTokenFactory{string}"/> object that represents the default factory instance.
            /// </summary>
            /// <value>
            /// The default instance.
            /// </value>
            public static AccessTokenFactory<string> DefaultFactory
            {
                get
                {
                    return new AccessTokenFactory<string>(DefaultIdFormatter, DefaultIdGenerator);
                }
            }

            /// <summary>
            /// The default pseudorandom Id generator.
            /// </summary>
            public static readonly Func<string> DefaultIdGenerator = () => GenerateToken(DefaultIdLength);

            /// <summary>
            /// The default Id formatter.
            /// </summary>
            public static readonly IValueIdFormatter<string> DefaultIdFormatter = ValueIdFormatter.String.DefaultFormatter;
        }

        /// <summary>
        /// The default pseudorandom value generator.
        /// </summary>
        public static readonly Func<int, string> DefaultValueGenerator = GenerateToken;

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

    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.Factories.IAccessTokenFactory{T}"/>.
    /// </summary>
    /// <remarks>
    /// This factory produces <see cref="OAuthWorks.Implementation.HashedAccessToken"/> objects. 
    /// All of the tokens that it produces are Bearer tokens.
    /// </remarks>
    public class AccessTokenFactory<TId> : IAccessTokenFactory<HashedAccessToken<TId>>
    {
        /// <summary>
        /// The default length (in bytes) of tokens that are generated.
        /// </summary>
        /// <value>40</value>
        public const int DefaultTokenLength = 40;

        /// <summary>
        /// The default lifetime of tokens generated in seconds.
        /// </summary>
        /// <value>3600 or 1 hour.</value>
        public const int DefaultTokenLifetime = 3600;

        /// <summary>
        /// Gets the function thatr generates a unique random ID.
        /// </summary>
        public Func<TId> IdValueGenerator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the function that, given an integer generates a string that represents that many pseudorandom bytes.
        /// </summary>
        /// <returns></returns>
        public Func<int, string> TokenValueGenerator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the length (in bytes) of tokens that are generated with this factory.
        /// </summary>
        public int TokenLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the length (in bytes) of the identifiers that are generated with this factory.
        /// </summary>
        public int IdLength
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
        /// Gets the formatter that combines the Id and refreshToken together.
        /// </summary>
        public IValueIdFormatter<TId> IdFormatter
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        public AccessTokenFactory(IValueIdFormatter<TId> idFormatter, Func<TId> idGenerator)
            : this(DefaultTokenLength, DefaultTokenLifetime, idFormatter, idGenerator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        /// <param name="tokenLength">The Length of the tokens (in bytes) that are generated from this factory.</param>
        /// <param name="tokenLifetime">The lifetime of the generated tokens in seconds.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        public AccessTokenFactory(int tokenLength, int tokenLifetime, IValueIdFormatter<TId> idFormatter, Func<TId> valueGenerator)
            : this(tokenLength, tokenLifetime, idFormatter, valueGenerator, AccessTokenFactory.DefaultValueGenerator)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        /// <param name="tokenLength">The Length of the tokens (in bytes) that are generated from this factory.</param>
        /// <param name="tokenLifetime">The lifetime of the generated tokens in seconds.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        /// <param name="idFormatter">The formatter that combines the generated ids and tokens.</param>
        /// <param name="idValueGenerator">A function that, given an integer returns a value that represents that many pseudorandom bytes.</param>
        public AccessTokenFactory(int tokenLength, int tokenLifetime, IValueIdFormatter<TId> idFormatter, Func<TId> idValueGenerator, Func<int, string> tokenGenerator)
        {
            Contract.Requires(tokenLength >= 10);
            Contract.Requires(tokenLifetime >= 1);
            Contract.Requires(idFormatter != null);
            Contract.Requires(idValueGenerator != null);
            Contract.Requires(tokenGenerator != null);
            this.TokenLength = tokenLength;
            this.TokenLifetime = tokenLifetime;
            this.IdFormatter = idFormatter;
            this.IdValueGenerator = idValueGenerator;
            this.TokenValueGenerator = tokenGenerator;
        }

        /// <summary>
        /// Gets a new <see cref="ICreatedToken{IAccessToken}"/> object given the <see cref="OAuthWorks.IClient"/>, <see cref="OAuthWorks.IUser"/> and the list of scopes that the client has access to.
        /// </summary>
        /// <param name="client">The client that should have access to the new refreshToken.</param>
        /// <param name="user">The user that is giving access to the client.</param>
        /// <param name="scopes">The list of scopes that the client should have access to.</param>
        /// <returns>Returns a new OAuthWorks.CreatedToken(of TAccessToken) object.</returns>
        public virtual ICreatedToken<HashedAccessToken<TId>> Create(IClient client, IUser user, IEnumerable<IScope> scopes)
        {
            string token = TokenValueGenerator(TokenLength);
            TId id = IdValueGenerator();
            string formatted = IdFormatter.FormatValue(id, token);
            return new CreatedToken<HashedAccessToken<TId>>(new HashedAccessToken<TId>(formatted, id, user, client, scopes, "Bearer", DateTime.UtcNow.AddSeconds(TokenLifetime)), formatted);
        }

        public virtual HashedAccessToken<TId> Create()
        {
            return null;
        }        
    }
}
