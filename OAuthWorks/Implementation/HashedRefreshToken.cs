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
    /// Defines a class that provides an implementation of <see cref="OAuthWorks.Implementation.RefreshToken"/> by using hashing
    /// as validation for token values.
    /// </summary>
    public class HashedRefreshToken : RefreshToken, IHasId<string>
    {
        /// <summary>
        /// The default number of iterations used for hashing token values.
        /// </summary>
        /// <remarks>
        /// Because Refresh Tokens can live possibly forever, it is important to treat them just like passwords.
        /// Therefore the number of hash iterations should be high.
        /// </remarks>
        /// <value>100,000</value>
        public const int DefaultHashIterations = 100000;

        /// <summary>
        /// The default number of bytes generated for the output hash.
        /// </summary>
        /// <remarks>
        /// Because Rfc2898DeriveBytes uses SHA-1, the output for that hash algorithm is 160 bits long. Anything
        /// over that does not add security, it just causes computation to take longer.
        /// </remarks>
        /// <value>20 or 160 bits</value>
        public const int DefaultHashSize = 20;

        /// <summary>
        /// Gets or sets the token hash.
        /// </summary>
        /// <value>
        /// The token hash.
        /// </value>
        public string TokenHash
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the token salt.
        /// </summary>
        /// <value>
        /// The token salt.
        /// </value>
        public string TokenSalt
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the number of iterations used when hashing the token.
        /// </summary>
        /// <value>
        /// The hash iterations.
        /// </value>
        public int HashIterations
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the Id of the token.
        /// </summary>
        public virtual string Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the PBKDF2 factory.
        /// </summary>
        /// <value>
        /// The PBKDF2 factory.
        /// </value>
        public IPbkdf2Factory Pbkdf2Factory
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedRefreshToken"/> class.
        /// </summary>
        /// <param name="id">The Id of the token.</param>
        /// <param name="token">The token.</param>
        /// <param name="user">The user.</param>
        /// <param name="client">The client.</param>
        /// <param name="scopes">The scopes.</param>
        public HashedRefreshToken(string id, string token, IUser user, IClient client, IEnumerable<IScope> scopes)
            : this(id, token, DefaultHashIterations, DefaultHashSize, user, client, scopes)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedRefreshToken"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="hashIterations">The hash iterations.</param>
        /// <param name="hashLength">Length of the hash.</param>
        /// <param name="user">The user.</param>
        /// <param name="client">The client.</param>
        /// <param name="scopes">The scopes.</param>
        public HashedRefreshToken(string id, string token, int hashIterations, int hashLength, IUser user, IClient client, IEnumerable<IScope> scopes)
            : base(user, client, scopes)
        {
            Contract.Requires(!string.IsNullOrEmpty(token));
            Contract.Requires(!string.IsNullOrEmpty(id));
            Contract.Requires(hashIterations >= 1000);
            Contract.Requires(hashLength >= 20);
            this.Id = id;
            this.HashIterations = hashIterations;
            this.Pbkdf2Factory = new Pbkdf2Sha1Factory();
            using (IPbkdf2 pbkdf2 = Pbkdf2Factory.Create(Encoding.UTF8.GetBytes(token), hashLength, this.HashIterations))
            {
                this.TokenSalt = encodeBytes(pbkdf2.Salt);
                this.TokenHash = encodeBytes(pbkdf2.GetBytes(hashLength));
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

        /// <summary>
        /// Determines if the given token value matches the one stored internally.
        /// </summary>
        /// <param name="token">The token to compare to the internal one.</param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public override bool MatchesValue(string token)
        {
            byte[] s = decodeString(this.TokenSalt);
            byte[] h = decodeString(this.TokenHash);

            using (IPbkdf2 pbkdf2 = Pbkdf2Factory.Create(Encoding.UTF8.GetBytes(token), s, HashIterations))
            {
                return pbkdf2.GetBytes(h.Length).SequenceEqual(h);
            }
        }
    }
}
