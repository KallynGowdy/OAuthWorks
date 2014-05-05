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
    /// All of the tokens that it produces are Bearer tokens.
    /// </remarks>
    public class AccessTokenFactory : IAccessTokenFactory<HashedAccessToken>
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
        /// The default length (in bytes) of identifiers that are generated.
        /// </summary>
        public const int DefaultIdLength = 8;

        /// <summary>
        /// The default pseudorandom value generator.
        /// </summary>
        public static readonly Func<int, string> DefaultValueGenerator = GenerateToken;

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
        public IValueIdFormatter IdFormatter
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        public AccessTokenFactory()
            : this(DefaultTokenLength, DefaultTokenLifetime, DefaultIdLength)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        /// <param name="tokenLength">The Length of the tokens (in bytes) that are generated from this factory.</param>
        /// <param name="tokenLifetime">The lifetime of the generated tokens in seconds.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        public AccessTokenFactory(int tokenLength, int tokenLifetime, int idLength)
            : this(tokenLength, tokenLifetime, idLength, DefaultIdFormatter, DefaultValueGenerator)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenFactory"/> class.
        /// </summary>
        /// <param name="tokenLength">The Length of the tokens (in bytes) that are generated from this factory.</param>
        /// <param name="tokenLifetime">The lifetime of the generated tokens in seconds.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        /// <param name="idFormatter">The formatter that combines the generated ids and tokens.</param>
        /// <param name="valueGenerator">A function that, given an integer returns a string that represents that many pseudorandom bytes.</param>
        public AccessTokenFactory(int tokenLength, int tokenLifetime, int idLength, IValueIdFormatter idFormatter, Func<int ,string> valueGenerator)
        {
            Contract.Requires(tokenLength >= 10);
            Contract.Requires(tokenLifetime >= 1);
            Contract.Requires(idLength >= 4);
            Contract.Requires(idFormatter != null);
            Contract.Requires(valueGenerator != null);
            this.IdLength = idLength;
            this.TokenLength = tokenLength;
            this.TokenLifetime = tokenLifetime;
            this.IdFormatter = idFormatter;
            this.ValueGenerator = valueGenerator;
        }

        public virtual ICreatedToken<HashedAccessToken> Create(IClient client, IUser user, IEnumerable<IScope> scopes)
        {
            string token = ValueGenerator(TokenLength);
            string id = ValueGenerator(IdLength);
            string formatted = IdFormatter.FormatValue(id, token);
            return new CreatedToken<HashedAccessToken>(new HashedAccessToken(formatted, id, user, client, scopes, "Bearer", DateTime.UtcNow.AddSeconds(TokenLifetime)), formatted);
        }

        public virtual HashedAccessToken Create()
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
