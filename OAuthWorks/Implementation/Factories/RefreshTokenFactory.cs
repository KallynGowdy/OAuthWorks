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
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.IRefreshTokenFactory"/>.
    /// </summary>
    public class RefreshTokenFactory : IRefreshTokenFactory<HashedRefreshToken>
    {
        /// <summary>
        /// The default length (in bytes) of tokens that are generated.
        /// </summary>
        /// <value>50</value>
        public const int DefaultTokenLength = 50;

        /// <summary>
        /// The default length (in bytes) of identifiers that are generated.
        /// </summary>
        public const int DefaultIdLength = 8;

        /// <summary>
        /// The default pseudorandom value generator.
        /// </summary>
        public static readonly Func<int, string> DefaultValueGenerator = AccessTokenFactory.GenerateToken;

        private static readonly Lazy<IValueIdFormatter> lazyDefaultIdFormatter = new Lazy<IValueIdFormatter>(() => new ValueIdFormatter());

        /// <summary>
        /// Gets the function that, given an integer generates a string that represents that many pseudorandom bytes.
        /// </summary>
        public Func<int, string> ValueGenerator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default formatter for Ids and tokens.
        /// </summary>
        public static IValueIdFormatter DefaultIdFormatter
        {
            get
            {
                return lazyDefaultIdFormatter.Value;
            }
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
        /// Gets the formatter that combines the Id and refreshToken together.
        /// </summary>
        public IValueIdFormatter IdFormatter
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        public RefreshTokenFactory()
            : this(DefaultTokenLength, DefaultIdLength)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        /// <param name="tokenLength">The Length of the tokens (in bytes) that are generated from this factory.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        public RefreshTokenFactory(int tokenLength, int idLength)
            : this(tokenLength, idLength, DefaultIdFormatter, DefaultValueGenerator)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        /// <param name="tokenLength">The Length of the tokens (in bytes) that are generated from this factory.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        /// <param name="idFormatter">The formatter that combines the generated ids and tokens.</param>
        /// <param name="valueGenerator">A function that, given an integer returns a string that represents that many pseudorandom bytes.</param>
        public RefreshTokenFactory(int tokenLength, int idLength, IValueIdFormatter idFormatter, Func<int ,string> valueGenerator)
        {
            Contract.Requires(tokenLength >= 10);
            Contract.Requires(idLength >= 4);
            Contract.Requires(idFormatter != null);
            Contract.Requires(valueGenerator != null);
            this.IdLength = idLength;
            this.TokenLength = tokenLength;
            this.IdFormatter = idFormatter;
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
        public ICreatedToken<HashedRefreshToken> Create(IClient client, IUser user, IEnumerable<IScope> scopes)
        {
            string token = ValueGenerator(TokenLength);
            string id = ValueGenerator(IdLength);
            string formatted = IdFormatter.FormatValue(id, token);
            return new CreatedToken<HashedRefreshToken>(new HashedRefreshToken(id, formatted, user, client, scopes), formatted);
        }

        public HashedRefreshToken Create()
        {
            return null;
        }
    }
}
