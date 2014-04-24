using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Hashing
{
    public class Pbkdf2Sha1Factory : IPbkdf2Factory
    {
        public IPbkdf2 Create(byte[] password, byte[] salt, int iterations)
        {
            return new Pbkdf2Sha1(password, salt, iterations);
        }

        public IPbkdf2 Create(byte[] password, int saltLength, int iterations)
        {
            return new Pbkdf2Sha1(password, saltLength, iterations);
        }

        public IPbkdf2 Create()
        {
            return null;
        }
    }
}
