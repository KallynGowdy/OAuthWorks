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
using System.Runtime.Serialization;
using System.Text;

namespace OAuthWorks.Implementation.Hashing
{
    /// <summary>
    /// Defines a class that stores a hash of a string value.
    /// </summary>
    [DataContract]
    public class HashedValue : IEquatable<HashedValue>
    {
        /// <summary>
        /// Gets or sets the <see cref="IHashFactory"/> used to create new <see cref="IHasher"/> objects used to hash values.
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public IHashFactory HashFactory
        {
            get;
            set;
        }

        private static readonly Lazy<IHashFactory> lazyHashFactory = new Lazy<IHashFactory>(() => new Pbkdf2Sha1Factory());

        /// <summary>
        /// Gets the default factory used to retrieve new <see cref="IHasher"/> objects.
        /// </summary>
        /// <value>
        /// The default hash factory.
        /// </value>
        public static IHashFactory DefaultHashFactory
        {
            get
            {
                return lazyHashFactory.Value;
            }
        }

        /// <summary>
        /// The default number of iterations to use when hashing.
        /// </summary>
        public const int DefaultHashIterations = 40000;

        /// <summary>
        /// The default size of the outputed hash, in bytes.
        /// </summary>
        public const int DefaultHashSize = 20;

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        [DataMember]
        public string Hash
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the salt used with the hash.
        /// </summary>
        [DataMember]
        public string Salt
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of iterations used to hash the value.
        /// </summary>
        [DataMember]
        public int HashIterations
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a pre-hashed value.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterationsUsed">The iterations used.</param>
        public HashedValue(string hash, string salt, int iterationsUsed)
        {
            if(hash == null)
            {
                throw new ArgumentNullException("hash");
            }
            if(salt == null)
            {
                throw new ArgumentNullException("salt");
            }
            if(iterationsUsed <= 0)
            {
                throw new ArgumentOutOfRangeException("iterationsUsed", "must be greater than 0");
            }
            this.Hash = hash;
            this.Salt = salt;
            this.HashIterations = iterationsUsed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedValue"/> class.
        /// </summary>
        public HashedValue()
        {
            this.HashFactory = DefaultHashFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedValue"/> class.
        /// </summary>
        /// <param name="hashFactory">The factory to use for retrieving new <see cref="IHasher"/> objects.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">hashFactory</exception>
        public HashedValue(IHashFactory hashFactory, string value) : this()
        {
            if(hashFactory == null)
            {
                throw new ArgumentNullException("hashFactory");
            }
            this.HashFactory = hashFactory;

            Init(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public HashedValue(string value) : this()
        {
            Init(value);
        }

        private void Init(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("The given value must not be null or empty", "value");
            }
            var hash = getHash(value, DefaultHashSize, DefaultHashIterations);
            this.Hash = hash.Item1;
            this.Salt = hash.Item2;
            this.HashIterations = DefaultHashIterations;
        }

        private Tuple<string, string> getHash(string value, int hashSize, int iterations)
        {
            using (IHasher hasher = HashFactory.Create(Encoding.UTF8.GetBytes(value), hashSize, iterations))
            {
                byte[] b = hasher.GetBytes(hashSize);
                return new Tuple<string, string>(Convert.ToBase64String(b), Convert.ToBase64String(hasher.Salt));
            }
        }

        private string getHash(string value, byte[] salt, int iterations)
        {
            using (IHasher hasher = HashFactory.Create(Encoding.UTF8.GetBytes(value), salt, iterations))
            {
                byte[] b = hasher.GetBytes(Convert.FromBase64String(Hash).Length);
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

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as HashedValue);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return unchecked((Hash ?? "").GetHashCode() * 23);
        }

        /// <summary>
        /// Determines if the specified value equals this one..
        /// </summary>
        /// <param name="other">The value to determine equality against.</param>
        /// <returns><c>true</c> if the specified <see cref="HashedValue"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(HashedValue other)
        {
            return other != null &&
                (
                    ( //neither null
                        other.Hash != null &&
                        this.Hash != null &&
                        other.Hash.Equals(this.Hash, StringComparison.Ordinal)
                    )
                    ||
                    ( //both null
                        other.Hash == null &&
                        this.Hash == null
                    )
                );
        }
    }
}
