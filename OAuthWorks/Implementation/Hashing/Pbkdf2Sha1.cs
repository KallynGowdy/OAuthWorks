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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Hashing
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pbkdf")] // 'Pbkdf' is an abbreveation
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sha")] // 'Sha' is an abbreveation.
    /// <summary>
    /// Defines a class that provides an implementation of PBKDF2 for SHA-1.
    /// </summary>
    public class Pbkdf2Sha1 : IHasher
    {
        Rfc2898DeriveBytes pbkdf2;

        /// <summary>
        /// Whether this object has been disposed or not.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pbkdf2Sha1"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The iterations.</param>
        public Pbkdf2Sha1(byte[] password, byte[] salt, int iterations)
        {
            Contract.Requires(password != null);
            Contract.Requires(salt != null);
            Contract.Requires(iterations > 0);
            pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pbkdf2Sha1"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="saltLength">Length of the salt.</param>
        /// <param name="iterations">The iterations.</param>
        public Pbkdf2Sha1(byte[] password, int saltLength, int iterations)
        {
            Contract.Requires(password != null);
            Contract.Requires(saltLength > 0);
            Contract.Requires(iterations > 0);
            byte[] salt = new byte[saltLength];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        }

        /// <summary>
        /// Gets the salt that is used for hashing.
        /// </summary>
        public byte[] Salt
        {
            get
            {
                ThrowIfDisposed();
                return pbkdf2.Salt;
            }
        }

        /// <summary>
        /// Gets the number of iterations used in creating the derived hash.
        /// </summary>
        public int Iterations
        {
            get
            {
                ThrowIfDisposed();
                return pbkdf2.IterationCount;
            }
        }

        /// <summary>
        /// Returns the pseudo-random key for this object.
        /// </summary>
        /// <param name="length">The number of pseudo-random bytes to return.</param>
        /// <returns></returns>
        public byte[] GetBytes(int length)
        {
            ThrowIfDisposed();
            return pbkdf2.GetBytes(length);
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if this object has been disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Pbkdf2Sha1"/> class.
        /// </summary>
        ~Pbkdf2Sha1()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (pbkdf2 != null)
                    {
                        pbkdf2.Dispose();
                        pbkdf2 = null;
                    }
                }
            }
            disposed = true;
        }
    }
}
