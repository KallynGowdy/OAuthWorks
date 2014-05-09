using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace ExampleWebApiProject.Models
{
    /// <summary>
    /// Defines a class that stores values that are hashed.
    /// </summary>
    public class HashedValue
    {
        public const int DefaultHashIterations = 40000;

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

        public HashedValue(string hash, string salt, int iterationsUsed)
        {
            this.Hash = hash;
            this.Salt = salt;
            this.HashIterations = iterationsUsed;
        }

        public HashedValue()
        {

        }

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