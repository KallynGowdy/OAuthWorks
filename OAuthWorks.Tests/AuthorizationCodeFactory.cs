using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuthWorks.Factories;
using System.Security.Cryptography;

namespace OAuthWorks.Tests
{
    class AuthorizationCodeFactory : IAuthorizationCodeFactory<AuthorizationCode>
    {

        public AuthorizationCode Create(out string generatedAuthorizationCode, IEnumerable<IScope> scopes)
        {
            StringBuilder token = new StringBuilder(61);
            string id;
            string value;

            //Generate the token
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                //Generate the 20 chracter identifier
                byte[] idBytes = new byte[10];
                rng.GetBytes(idBytes);
                id = BitConverter.ToString(idBytes).Replace("-", string.Empty);
                token.Append(id);

                token.Append('-');

                //Generate the 40 character token
                byte[] valueBytes = new byte[20];
                rng.GetBytes(valueBytes);
                value = BitConverter.ToString(valueBytes).Replace("-", string.Empty);
                token.Append(value);
            }

            generatedAuthorizationCode = token.ToString();
            return new AuthorizationCode(token.ToString(), id, DateTime.UtcNow.AddHours(2), scopes);
        }

        public AuthorizationCode Create()
        {
            return null;
        }
    }
}
