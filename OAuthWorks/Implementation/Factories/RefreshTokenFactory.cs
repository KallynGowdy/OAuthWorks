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
    public static class RefreshTokenFactory
    {

        /// <summary>
        /// The default length (in bytes) of tokens that are generated.
        /// </summary>
        /// <value>20</value>
        public const int DefaultTokenLength = 20;

        /// <summary>
        /// The default pseudorandom value generator.
        /// </summary>
        public static readonly Func<int, string> DefaultValueGenerator = AccessTokenFactory.GenerateToken;

        public static class String
        {
            /// <summary>
            /// The default length (in bytes) of identifiers that are generated.
            /// </summary>
            public const int DefaultIdLength = 8;

            /// <summary>
            /// The default pseudorandom id generator.
            /// </summary>
            public static readonly Func<string> DefaultIdValueGenerator = () => AccessTokenFactory.GenerateToken(DefaultIdLength);

            private static readonly Lazy<IValueIdFormatter<string>> lazyDefaultIdFormatter = new Lazy<IValueIdFormatter<string>>(() => ValueIdFormatter.String.DefaultFormatter);

            /// <summary>
            /// Gets the default formatter for Ids and tokens.
            /// </summary>
            public static IValueIdFormatter<string> DefaultIdFormatter
            {
                get
                {
                    return lazyDefaultIdFormatter.Value;
                }
            }

            /// <summary>
            /// Gets the default refresh token factory that produces .
            /// </summary>
            /// <value>
            /// The default factory.
            /// </value>
            public static RefreshTokenFactory<string> DefaultFactory
            {
                get
                {
                    return new RefreshTokenFactory<string>(DefaultIdFormatter, DefaultIdValueGenerator);
                }
            }
        }

    }

    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.IRefreshTokenFactory"/>.
    /// </summary>
    public class RefreshTokenFactory<TId> : IRefreshTokenFactory<HashedRefreshToken<TId>>
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
        /// Gets the identifier generator.
        /// </summary>
        /// <value>
        /// The identifier generator.
        /// </value>
        public Func<TId> IdGenerator
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
        /// Gets the formatter that combines the Id and refreshToken together.
        /// </summary>
        public IValueIdFormatter<TId> IdFormatter
        {
            get;
            private set;
        }

        public RefreshTokenFactory(IValueIdFormatter<TId> idFormatter, Func<TId> idGenerator)
            : this(RefreshTokenFactory.DefaultTokenLength, idFormatter, idGenerator, RefreshTokenFactory.DefaultValueGenerator)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        /// <param name="tokenLength">The Length of the tokens (in bytes) that are generated from this factory.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        /// <param name="idFormatter">The formatter that combines the generated ids and tokens.</param>
        /// <param name="idGenerator">A function that generates a new psudorandom unique identifier for the new token.</param>
        /// <param name="valueGenerator">A function that, given an integer returns a string that represents that many pseudorandom bytes.</param>
        public RefreshTokenFactory(int tokenLength, IValueIdFormatter<TId> idFormatter, Func<TId> idGenerator, Func<int, string> valueGenerator)
        {
            if (tokenLength <= 0) throw new ArgumentOutOfRangeException("tokenLength must be greater than 0.", "tokenLength");

            Contract.Requires(idFormatter != null);
            Contract.Requires(idGenerator != null);
            this.TokenLength = tokenLength;
            this.IdFormatter = idFormatter;
            this.IdGenerator = idGenerator;
            this.ValueGenerator = valueGenerator;
        }

        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IRefreshToken"/> that can be used by the given <see cref="OAuthWorks.IClient"/> for the given <see cref="OAuthWorks.IUser"/> for the given
        /// enumerable <see cref="OAuthWorks.IScope"/> objects.
        /// </summary>
        /// <param name="generatedToken">The refreshToken that was generated as it should be returned to the client.</param>
        /// <param name="client">The client that will be using the issued refresh refreshToken.</param>
        /// <param name="user">The user that is granting access to the given client for the given scopes.</param>
        /// <param name="scopes">The enumerable list of <see cref="OAuthWorks.IScope"/> objects that define what access the client has to the
        /// user's account and data.</param>
        /// <returns>Returns a new Refresh Token that can be used to request new Access Tokens.</returns>
        public ICreatedToken<HashedRefreshToken<TId>> Create(IClient client, IUser user, IEnumerable<IScope> scopes)
        {
            string token = ValueGenerator(TokenLength);
            TId id = IdGenerator();
            string formatted = IdFormatter.FormatValue(id, token);
            return new CreatedToken<HashedRefreshToken<TId>>(new HashedRefreshToken<TId>(formatted, id, user, client, scopes), formatted);
        }

        public HashedRefreshToken<TId> Create()
        {
            return null;
        }
    }
}
