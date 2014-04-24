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

using OAuthWorks.Implementation.Hashing;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides an implementation of <see cref="OAuthWorks.Implementation.AccessToken"/> that hashes it's value to prevent stealing.
    /// </summary>
    /// <remarks>
    /// Access Tokens should be stored as a hash. Even though they only live for a limited amount of time, the extra layer of security will make it all but impossible to crack.
    /// This implementation uses PBKDF2. The number of iterations are configurable and the output is 20 bytes (180 bits) long. A salt is generated with the hash as well.
    /// The default number of iterations is 1000. While this is outdated compared to the minimum recommended iterations, the fact that tokens are short lived mitigates this fear.
    /// </remarks>
    public sealed class HashedAccessToken : AccessToken, IHasId<string>
    {
        /// <summary>
        /// The number of hash iterations used if not specified.
        /// </summary>
        public const int DefaultHashIterations = 1000;

        /// <summary>
        /// The number of bytes that should be generated for the hash and salt.
        /// </summary>
        public const int DefaultHashLength = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedAccessToken"/> class.
        /// </summary>
        /// <param name="token">The token stored in this object.</param>
        /// <param name="id">The id of the token.</param>
        /// <param name="user">The user that this token belongs to.</param>
        /// <param name="client">The client that has access to this token.</param>
        /// <param name="scopes">The scopes that this token provides access to.</param>
        /// <param name="tokenType">Type of the token. Describes how the client should handle it.</param>
        /// <param name="expirationDateUtc">The date of expiration in Universal Coordinated Time.</param>
        public HashedAccessToken(string token, string id, IUser user, IClient client, IEnumerable<IScope> scopes, string tokenType, DateTime expirationDateUtc)
            : base(id, user, client, scopes, tokenType, expirationDateUtc)
        {
            this.HashIterations = DefaultHashIterations;
            Tuple<string, string> hashSalt = generateHash(token, DefaultHashLength, this.HashIterations);
            this.TokenHash = hashSalt.Item1;
            this.TokenSalt = hashSalt.Item2;
            Pbkdf2Factory = new Pbkdf2Sha1Factory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedAccessToken"/> class.
        /// </summary>
        /// <param name="token">The token stored in this object.</param>
        /// <param name="id">The id of the token.</param>
        /// <param name="user">The user that this token belongs to.</param>
        /// <param name="client">The client that has access to this token.</param>
        /// <param name="scopes">The scopes that this token provides access to.</param>
        /// <param name="tokenType">Type of the token. Describes how the client should handle it.</param>
        /// <param name="expirationDateUtc">The date of expiration in Universal Coordinated Time.</param>
        public HashedAccessToken(IPbkdf2Factory pbkdf2Factory, string token, string id, IUser user, IClient client, IEnumerable<IScope> scopes, string tokenType, DateTime expirationDateUtc)
            : base(id, user, client, scopes, tokenType, expirationDateUtc)
        {
            Contract.Requires(pbkdf2Factory != null);
            this.HashIterations = DefaultHashIterations;
            Tuple<string, string> hashSalt = generateHash(token, DefaultHashLength, this.HashIterations);
            this.TokenHash = hashSalt.Item1;
            this.TokenSalt = hashSalt.Item2;
            Pbkdf2Factory = pbkdf2Factory;
        }

        /// <summary>
        /// Gets the PBKDF2 factory.
        /// </summary>
        /// <value>
        /// The PBKDF2 factory.
        /// </value>
        public IPbkdf2Factory Pbkdf2Factory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the hash that represents the token.
        /// </summary>
        public string TokenHash
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the salt that was used in creating the hash.
        /// </summary>
        public string TokenSalt
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of iterations used to hash the token.
        /// </summary>
        public int HashIterations
        {
            get;
            private set;
        }

        /// <summary>
        /// Determines if the given token value matches the one stored internally.
        /// </summary>
        /// <param name="token">The token to compare to the internal one.</param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public override bool MatchesValue(string token)
        {
            byte[] salt = decodeString(TokenSalt);
            byte[] target = decodeString(TokenHash);
            byte[] hash;

            using (IPbkdf2 pbkdf2 = Pbkdf2Factory.Create(Encoding.UTF8.GetBytes(token), salt, HashIterations))
            {
                hash = pbkdf2.GetBytes(target.Length);
            }

            return target.SequenceEqual(hash);
        }

        private Tuple<string, string> generateHash(string password, int length, int iterations)
        {
            using (IPbkdf2 pbkdf2 = Pbkdf2Factory.Create(Encoding.UTF8.GetBytes(password), length, iterations))
            {
                return new Tuple<string, string>(encodeBytes(pbkdf2.GetBytes(length)), encodeBytes(pbkdf2.Salt));
            }
        }

        private static string encodeBytes(byte[] b)
        {
            return Convert.ToBase64String(b);
        }

        private static byte[] decodeString(string s)
        {
            return Convert.FromBase64String(s);
        }
    }
}
