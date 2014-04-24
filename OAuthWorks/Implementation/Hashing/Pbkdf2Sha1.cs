using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Hashing
{
    /// <summary>
    /// Defines a class that provides an implementation of PBKDF2 for SHA-1.
    /// </summary>
    public class Pbkdf2Sha1 : IPbkdf2
    {
        Rfc2898DeriveBytes pbkdf2;

        public Pbkdf2Sha1(byte[] password, byte[] salt, int iterations)
        {
            Contract.Requires(password != null);
            Contract.Requires(salt != null);
            Contract.Requires(iterations > 0);
            pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        }

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

        public byte[] Salt
        {
            get { return pbkdf2.Salt; }
        }

        public int Iterations
        {
            get { return pbkdf2.IterationCount; }
        }

        public byte[] GetBytes(int length)
        {
            return pbkdf2.GetBytes(length);
        }

        public void Dispose()
        {
            pbkdf2.Dispose();
        }
    }
}
