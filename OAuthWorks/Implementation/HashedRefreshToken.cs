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
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides an implementation of <see cref="OAuthWorks.Implementation.RefreshToken"/> by using hashing
    /// as validation for refreshToken values.
    /// </summary>
    [DataContract]
    public sealed class HashedRefreshToken : RefreshToken, IHasId<string>
    {
        /// <summary>
        /// The default number of iterations used for hashing refreshToken values.
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
        /// The default lifetime (in seconds) of these refresh tokens.
        /// </summary>
        /// <value>31,536,000 or 1 year</value>
        public const int DefaultLifetime = 31536000;

        /// <summary>
        /// Gets or sets the refreshToken hash.
        /// </summary>
        /// <value>
        /// The refreshToken hash.
        /// </value>
        [DataMember(Name = "TokenHash")]
        public string TokenHash
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the refreshToken salt.
        /// </summary>
        /// <value>
        /// The refreshToken salt.
        /// </value>
        [DataMember(Name = "TokenSalt")]
        public string TokenSalt
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of iterations used when hashing the refreshToken.
        /// </summary>
        /// <value>
        /// The hash iterations.
        /// </value>
        [DataMember(Name = "HashIterations")]
        public int HashIterations
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Id of the refreshToken.
        /// </summary>
        [DataMember(Name = "Id")]
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the <see cref="IHashFactory"/> factory.
        /// </summary>
        /// <value>
        /// The <see cref="IHashFactory"/> factory.
        /// </value>
        public IHashFactory HashFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedRefreshToken"/> class.
        /// </summary>
        /// <param name="id">The Id of the refreshToken.</param>
        /// <param name="refreshToken">The refreshToken.</param>
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
        /// <param name="refreshToken">The refreshToken.</param>
        /// <param name="hashIterations">The hash iterations.</param>
        /// <param name="hashLength">Length of the hash.</param>
        /// <param name="user">The user.</param>
        /// <param name="client">The client.</param>
        /// <param name="scopes">The scopes.</param>
        public HashedRefreshToken(string id, string token, int hashIterations, int hashLength, IUser user, IClient client, IEnumerable<IScope> scopes)
            : base(DateTime.UtcNow.AddSeconds(DefaultLifetime))
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentException("The given token must not be null or empty.", "token");
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("The given ID must not be null or empty.", "id");
            if (hashIterations < 1000) throw new ArgumentOutOfRangeException("hashIterations", "The given hash iterations must be greater than or equal to 1000.");
            if (hashLength < 20) throw new ArgumentOutOfRangeException("hashLength", "The given hash length must be greater than or equal to 20.");
            if (user == null) throw new ArgumentNullException("user");
            if (client == null) throw new ArgumentNullException("client");
            if (scopes == null) throw new ArgumentNullException("scopes");

            this.Client = client;
            this.User = user;
            this.Scopes = scopes;
            this.Id = id;
            this.HashIterations = hashIterations;
            this.HashFactory = new Pbkdf2Sha1Factory();
            using (IHasher pbkdf2 = HashFactory.Create(Encoding.UTF8.GetBytes(token), hashLength, this.HashIterations))
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
        /// Determines if the given refreshToken value matches the one stored internally.
        /// </summary>
        /// <param name="refreshToken">The refreshToken to compare to the internal one.</param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public override bool MatchesValue(string token)
        {
            byte[] s = decodeString(this.TokenSalt);
            byte[] h = decodeString(this.TokenHash);

            using (IHasher pbkdf2 = HashFactory.Create(Encoding.UTF8.GetBytes(token), s, HashIterations))
            {
                return pbkdf2.GetBytes(h.Length).SequenceEqual(h);
            }
        }
    }
}
