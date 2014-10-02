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
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a static class that contains default values for <see cref="HashedAuthorizationCode{TId}"/> objects.
    /// </summary>
    public static class HashedAuthorizationCode
    {
        /// <summary>
        /// The default number of hash iterations for authorization codes.
        /// </summary>
        /// <remarks>
        /// Because authorization codes are only valid for a limited amount of time, the number of iterations applied does not need to be very high at all.
        /// To decrease server load when handling authorization codes securely, authorization codes don't use settings as thorough as Refresh Tokens.
        /// </remarks>
        /// <value>2,000</value>
        public const int DefaultHashIterations = 2000;

        /// <summary>
        /// The default length (in bytes) of generated hashes.
        /// </summary>
        /// <value>20 bytes (160 bits)</value>
        public const int DefaultHashLength = 20;
    }

    /// <summary>
    /// Defines a class that provides an implementation of <see cref="OAuthWorks.AuthorizationCode"/>.
    /// </summary>
    public class HashedAuthorizationCode<TId> : AuthorizationCode<TId>
    {
        /// <summary>
        /// Gets the hash of the code.
        /// </summary>
        /// <returns>The code hash.</returns>
        public HashedValue CodeHash
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedAuthorizationCode"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="refreshToken">The refreshToken.</param>
        /// <param name="user">The user.</param>
        /// <param name="client">The client.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="expirationDateUtc">The expiration date UTC.</param>
        public HashedAuthorizationCode(TId id, string token, IUser user, IClient client, IEnumerable<IScope> scopes, Uri redirectUri, DateTime expirationDateUtc)
            : base(id, user, client, scopes, redirectUri, expirationDateUtc)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentException("The given token must not be null or empty", "token");
            this.CodeHash = new HashedValue(token);
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
            return CodeHash.MatchesHash(token);
        }
    }
}
