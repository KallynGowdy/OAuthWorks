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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Hashing
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pbkdf")] // 'Pbkdf' is an abbreveation.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sha")] // 'Sha' is an abbreveation.
    /// <summary>
    /// Defines a basic factory that implements <see cref="OAuthWOrks.Implementation.Hashing.IPbkdf2Factory"/>.
    /// </summary>
    public class Pbkdf2Sha1Factory : IHashFactory
    {
        /// <summary>
        /// Creates a new <see cref="OAuthWorks.Implementation.Hashing.IHasher" /> object using the given password, salt and hash iterations.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt that should be used in the hashing process.</param>
        /// <param name="iterations">The number of iterations that should be used when hashing.</param>
        /// <returns>
        /// Returns a new <see cref="OAuthWorks.Implementation.Hashing.IHasher" /> object that creates a derived password(hash) from the given values.
        /// </returns>
        public IHasher Create(byte[] password, byte[] salt, int iterations)
        {
            return new Pbkdf2Sha1(password, salt, iterations);
        }

        /// <summary>
        /// Creates a new <see cref="OAuthWorks.Implementation.Hashing.IHasher" /> object using the given password, salt length and hash iterations.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="saltLength">The length of the salt to generate.</param>
        /// <param name="iterations">The number of iterations that should be used when hashing.</param>
        /// <returns>
        /// Returns a new <see cref="OAuthWorks.Implementation.Hashing.IHasher" /> object that creates a derived password(hash) from the given values.
        /// </returns>
        public IHasher Create(byte[] password, int saltLength, int iterations)
        {
            return new Pbkdf2Sha1(password, saltLength, iterations);
        }

        public IHasher Create()
        {
            return null;
        }
    }
}
