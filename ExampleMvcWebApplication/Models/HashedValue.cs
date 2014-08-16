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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace ExampleMvcWebApplication.Models
{
    /// <summary>
    /// Defines a class that stores values that are hashed.
    /// </summary>
    public class HashedValue
    {
        /// <summary>
        /// The default hash iterations.
        /// </summary>
        public const int DefaultHashIterations = 40000;

        /// <summary>
        /// The default hash size, in bytes.
        /// </summary>
        public const int DefaultHashSize = 20;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hashed result.
        /// </summary>
        public string Hash
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the salt used with the hash.
        /// </summary>
        public string Salt
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of iterations used to hash the value.
        /// </summary>
        public int HashIterations
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedValue"/> class.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterationsUsed">The iterations used.</param>
        public HashedValue(string hash, string salt, int iterationsUsed)
        {
            this.Hash = hash;
            this.Salt = salt;
            this.HashIterations = iterationsUsed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedValue"/> class.
        /// </summary>
        public HashedValue()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedValue"/> class.
        /// </summary>
        /// <param name="value">The value to store in the hash.</param>
        public HashedValue(string value)
        {
            var hash = getHash(value, DefaultHashSize, DefaultHashIterations);
            this.Hash = hash.Item1;
            this.Salt = hash.Item2;
            this.HashIterations = DefaultHashIterations;
        }

        private Tuple<string, string> getHash(string value, int hashSize, int iterations)
        {
            using (System.Security.Cryptography.Rfc2898DeriveBytes pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(value, hashSize + 2, iterations))
            {
                byte[] b = pbkdf2.GetBytes(hashSize);
                return new Tuple<string, string>(Convert.ToBase64String(b), Convert.ToBase64String(pbkdf2.Salt));
            }
        }

        private string getHash(string value, byte[] salt, int iterations)
        {
            using (System.Security.Cryptography.Rfc2898DeriveBytes pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(value, salt, iterations))
            {
                byte[] b = pbkdf2.GetBytes(Convert.FromBase64String(Hash).Length);
                return Convert.ToBase64String(b);
            }
        }

        /// <summary>
        /// Determines if the given value matches the hash stored in this value.
        /// </summary>
        /// <param name="value">The value to match to this hash.</param>
        /// <returns>Returns true if the given value matches the originally stored value, otherwise false.</returns>
        public bool MatchesHash(string value)
        {
            return getHash(value, Convert.FromBase64String(this.Salt), this.HashIterations).Equals(this.Hash, StringComparison.Ordinal);
        }
    }
}