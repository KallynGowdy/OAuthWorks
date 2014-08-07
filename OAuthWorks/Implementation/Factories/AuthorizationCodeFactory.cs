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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Factories
{
    /// <summary>
    /// Defines a static class that acts as a wrapper for the different default implementations of <see cref="AuthorizationCodeFactory{TId}"/>.
    /// </summary>
    public static class AuthorizationCodeFactory
    {
        /// <summary>
        /// The default length of the generated codes in bytes.
        /// </summary>
        /// <value>40</value>
        public const int DefaultCodeLength = 40;

        /// <summary>
        /// The default lifetime for generated authorization codes.
        /// </summary>
        /// <value>3,600 seconds or 1 hour</value>
        public const int DefaultCodeLifetime = 3600;

        /// <summary>
        /// The default pseudorandom value generator.
        /// </summary>
        public static readonly Func<int, string> DefaultValueGenerator = AccessTokenFactory.GenerateToken;

        public static class String
        {
            /// <summary>
            /// The default length of the generated identifiers in bytes.
            /// </summary>
            /// <value>8</value>
            public const int DefaultIdLength = 8;

            private static readonly Lazy<IValueIdFormatter<string>> lazyFormatter = new Lazy<IValueIdFormatter<string>>(() => ValueIdFormatter.String.DefaultFormatter);

            /// <summary>
            /// Gets the default formatter for Ids and tokens.
            /// </summary>
            public static IValueIdFormatter<string> DefaultIdFormatter
            {
                get
                {
                    return lazyFormatter.Value;
                }
            }

            public static readonly Func<string> DefaultIdGenerator = () => AccessTokenFactory.GenerateToken(DefaultIdLength);

            public static AuthorizationCodeFactory<string> DefaultFactory
            {
                get
                {
                    return new AuthorizationCodeFactory<string>(DefaultIdFormatter, DefaultIdGenerator);
                }
            }
        }
    }

    /// <summary>
    /// Defines a class which provides a basic implementation of <see cref="OAuthWorks.IAuthorizationCodeFactory{HashedAuthorizationCode{TId}"/>.
    /// </summary>
    public class AuthorizationCodeFactory<TId> : IAuthorizationCodeFactory<HashedAuthorizationCode<TId>>
    {

        /// <summary>
        /// Gets the function that, given an integer generates a string that represents that many pseudorandom bytes.
        /// </summary>
        public Func<int, string> ValueGenerator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the function that returns a unique psudorandom Id 
        /// </summary>
        /// <returns></returns>
        public Func<TId> IdGenerator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the length of the codes(in bytes) generated from this factory.
        /// </summary>
        public int CodeLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the lifetime of the generated codes in seconds.
        /// </summary>
        public int CodeLifetime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the length (in bytes) of identifiers generated for codes in this factory.
        /// </summary>
        public int IdLength
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
        /// Initializes a new instance of the <see cref="AuthorizationCodeFactory"/> class.
        /// </summary>
        public AuthorizationCodeFactory(IValueIdFormatter<TId> idFormatter, Func<TId> idGenerator)
            : this(AuthorizationCodeFactory.DefaultCodeLength, AuthorizationCodeFactory.DefaultCodeLifetime, idFormatter, idGenerator)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeFactory"/> class.
        /// </summary>
        /// <param name="codeLength">Length of the authorization codes to generate (in bytes).</param>
        /// <param name="codeLifetime">The number of seconds that the generated codes will be valid for.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        public AuthorizationCodeFactory(int codeLength, int codeLifetime, IValueIdFormatter<TId> idFormatter, Func<TId> idGenerator)
            : this(codeLength, codeLifetime, idFormatter, idGenerator, AuthorizationCodeFactory.DefaultValueGenerator)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeFactory"/> class.
        /// </summary>
        /// <param name="codeLength">Length of the authorization codes to generate (in bytes).</param>
        /// <param name="codeLifetime">The number of seconds that the generated codes will be valid for.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        /// <param name="idFormatter">The formatter that combines the generated ids and tokens.</param>
        /// <param name="valueGenerator">A function that, given an integer returns a string that represents that many pseudorandom bytes.</param>
        public AuthorizationCodeFactory(int codeLength, int codeLifetime, IValueIdFormatter<TId> idFormatter, Func<TId> idGenerator, Func<int, string> valueGenerator)
        {
            if (codeLength < 20)        throw new ArgumentOutOfRangeException("The given codeLength must be greater than or equal to 20.");
            if (codeLifetime <= 0)      throw new ArgumentOutOfRangeException("The given codeLifetime must be greater than 0.", "codeLifetime");
            if (idFormatter == null)    throw new ArgumentNullException("idFormatter");
            if (idGenerator == null)    throw new ArgumentNullException("idGenerator");
            if (valueGenerator == null) throw new ArgumentNullException("valueGenerator");

            this.CodeLength = codeLength;
            this.CodeLifetime = codeLifetime;
            this.IdFormatter = idFormatter;
            this.ValueGenerator = valueGenerator;
            this.IdGenerator = idGenerator;
        }

        /// <summary>
        /// Creates a new <see cref="OAuthWorks.IAuthorizationCode"/> object given the granted scopes.
        /// </summary>
        /// <param name="scopes">The enumerable list of scopes that were granted by the user to the client.</param>
        /// <param name="user">The user that the created authorization code is bound to.</param>
        /// <param name="client">The client that the code is granted for.</param>
        /// <param name="redirectUri">The URI that was provided by the client that specifies the location that the user should be redirected to after completing authorization.</param>
        /// <returns>Returns a new OAuthWorks.CreatedToken(of TAuthorizationCode) object.</returns>
        public ICreatedToken<HashedAuthorizationCode<TId>> Create(Uri redirectUri, IUser user, IClient client, IEnumerable<IScope> scopes)
        {
            string token = ValueGenerator(CodeLength);
            TId id = IdGenerator();
            string formatted = IdFormatter.FormatValue(id, token);
            DateTime expirationDate = DateTime.UtcNow.AddSeconds(CodeLifetime);
            return new CreatedToken<HashedAuthorizationCode<TId>>(new HashedAuthorizationCode<TId>(id, formatted, user, client, scopes, redirectUri, expirationDate), formatted);
        }

        public HashedAuthorizationCode<TId> Create()
        {
            return null;
        }
    }
}
