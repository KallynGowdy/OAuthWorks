using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuthWorks.Factories;
using System.Security.Cryptography;

namespace OAuthWorks.Tests
{
    class AuthorizationCodeFactory : IAuthorizationCodeFactory<AuthorizationCode, AuthorizationCodeResponseException>
    {
        public ICreatedToken<AuthorizationCode> Create(Uri redirectUri, IEnumerable<IScope> scopes)
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

            return new CreatedToken<AuthorizationCode>(new AuthorizationCode(token.ToString(), id, redirectUri, DateTime.UtcNow.AddHours(2), scopes), token.ToString());
        }

        public AuthorizationCode Create()
        {
            return null;
        }



    }
}
