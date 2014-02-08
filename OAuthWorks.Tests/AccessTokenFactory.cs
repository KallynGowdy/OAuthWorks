using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class AccessTokenFactory : IAccessTokenFactory<AccessToken>
    {

        public ICreatedToken<AccessToken> Create( IClient client, IUser user, IEnumerable<IScope> scopes)
        {
            StringBuilder generatedAccessToken = new StringBuilder(61);

            string token;
            string id;

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] idBytes = new byte[10];
                rng.GetBytes(idBytes);
                id = BitConverter.ToString(idBytes).Replace('-', ' ');
                generatedAccessToken.Append(id);
                generatedAccessToken.Append(' ');

                byte[] tokenBytes = new byte[20];
                rng.GetBytes(tokenBytes);
                token = BitConverter.ToString(tokenBytes).Replace('-', ' ');
                generatedAccessToken.Append(token);
            }


            return new CreatedToken<AccessToken>(new AccessToken
            {
                Id = id,
                Token = generatedAccessToken.ToString(),
                Client = client,
                User = user,
                Scopes = scopes,
                TokenType = "bearer",
                ExpirationDateUtc = DateTime.UtcNow.AddHours(2)
            },
            token.ToString());
        }

        public AccessToken Create()
        {
            return null;
        }
    }
}
