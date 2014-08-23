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
    /// Defines a static class that contains default values for <see cref="HashedRefreshToken{TId}"/> objects.
    /// </summary>
    public static class HashedRefreshToken
    {
        /// <summary>
        /// The default lifetime (in seconds) of these refresh tokens.
        /// </summary>
        /// <value>31,536,000 or 1 year</value>
        public const int DefaultLifetime = 31536000;

        private static readonly Lazy<IHashFactory> lazyDefaultHashFactory = new Lazy<IHashFactory>(() => new Pbkdf2Sha1Factory());

        /// <summary>
        /// Gets the default <see cref="IHashFactory"/> object used for hashing the refresh tokens.
        /// </summary>
        /// <returns></returns>
        public static IHashFactory DefaultHashFactory
        {
            get
            {
                return lazyDefaultHashFactory.Value;
            }
        }
    }

    /// <summary>
    /// Defines a class that provides an implementation of <see cref="OAuthWorks.Implementation.RefreshToken"/> by using hashing
    /// as validation for refreshToken values.
    /// </summary>
    [DataContract]
    public sealed class HashedRefreshToken<TId> : RefreshToken<TId>
    {
        /// <summary>
        /// Gets the hash of the token.
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public HashedValue TokenHash
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
        public HashedRefreshToken(string token, TId id, IUser user, IClient client, IEnumerable<IScope> scopes)
            : this(HashedRefreshToken.DefaultHashFactory, token, id, user, client, scopes)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedRefreshToken"/> class.
        /// </summary>
        /// <param name="token">The generated token.</param>
        /// <param name="hashIterations">The hash iterations.</param>
        /// <param name="hashLength">Length of the hash.</param>
        /// <param name="user">The user.</param>
        /// <param name="client">The client.</param>
        /// <param name="scopes">The scopes.</param>
        public HashedRefreshToken(IHashFactory hashFactory, string token, TId id, IUser user, IClient client, IEnumerable<IScope> scopes)
            : base(id, user, client, scopes, DateTime.UtcNow.AddSeconds(HashedRefreshToken.DefaultLifetime))
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentException("The given token must not be null or empty.", "token");

            TokenHash = new HashedValue(hashFactory, token);            
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
            return TokenHash.MatchesHash(token);
        }
    }
}
