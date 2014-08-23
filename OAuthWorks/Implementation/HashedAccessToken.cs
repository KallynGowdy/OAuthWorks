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
    public static class HashedAccessToken
    {
        private static readonly Lazy<IHashFactory> lazyDefaultHashFactory = new Lazy<IHashFactory>(() => new Pbkdf2Sha1Factory());

        /// <summary>
        /// Gets the default <see cref="IHashFactory"/> used to create new <see cref="IHasher"/> objects.
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
    /// Defines a class that provides an implementation of <see cref="OAuthWorks.Implementation.AccessToken"/> that hashes it's value to prevent stealing.
    /// </summary>
    /// <remarks>
    /// Access Tokens should be stored as a hash. Even though they only live for a limited amount of time, the extra layer of security will make it all but impossible to crack.
    /// This implementation uses PBKDF2. The number of iterations are configurable and the output is 20 bytes (180 bits) long. A salt is generated with the hash as well.
    /// The default number of iterations is 1000. While this is outdated compared to the minimum recommended iterations, the fact that tokens are short lived mitigates this fear.
    /// </remarks>
    [DataContract]
    public class HashedAccessToken<TId> : AccessToken<TId>, IEquatable<HashedAccessToken<TId>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashedAccessToken"/> class.
        /// </summary>
        /// <param name="refreshToken">The refreshToken stored in this object.</param>
        /// <param name="id">The id of the refreshToken.</param>
        /// <param name="user">The user that this refreshToken belongs to.</param>
        /// <param name="client">The client that has access to this refreshToken.</param>
        /// <param name="scopes">The scopes that this refreshToken provides access to.</param>
        /// <param name="tokenType">Type of the refreshToken. Describes how the client should handle it.</param>
        /// <param name="expirationDateUtc">The date of expiration in Universal Coordinated Time.</param>
        public HashedAccessToken(string token, TId id, IUser user, IClient client, IEnumerable<IScope> scopes, string tokenType, DateTime expirationDateUtc)
            : this(HashedAccessToken.DefaultHashFactory, token, id, user, client, scopes, tokenType, expirationDateUtc)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedAccessToken"/> class.
        /// </summary>
        /// <param name="refreshToken">The refreshToken stored in this object.</param>
        /// <param name="id">The id of the refreshToken.</param>
        /// <param name="user">The user that this refreshToken belongs to.</param>
        /// <param name="client">The client that has access to this refreshToken.</param>
        /// <param name="scopes">The scopes that this refreshToken provides access to.</param>
        /// <param name="tokenType">Type of the refreshToken. Describes how the client should handle it.</param>
        /// <param name="expirationDateUtc">The date of expiration in Universal Coordinated Time.</param>
        public HashedAccessToken(IHashFactory hashFactory, string token, TId id, IUser user, IClient client, IEnumerable<IScope> scopes, string tokenType, DateTime expirationDateUtc)
            : base(id, user, client, scopes, tokenType, expirationDateUtc)
        {
            Contract.Requires(hashFactory != null);
            this.TokenHash = new HashedValue(hashFactory, token);
        }


        [DataMember(Name = "TokenHash")]
        public HashedValue TokenHash
        {
            get;
            private set;
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


        public override int GetHashCode()
        {
            return TokenHash.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as HashedAccessToken<TId>);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(HashedAccessToken<TId> other)
        {
            return other != null &&
                this.TokenHash.Equals(other.TokenHash);
        }
    }
}
