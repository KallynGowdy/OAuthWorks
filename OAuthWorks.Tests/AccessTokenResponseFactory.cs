using Ninject;
using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    public class AccessTokenResponseFactory : IAccessTokenResponseFactory<IAccessTokenResponse, OAuthWorks.AccessTokenResponseException>
    {
        [Inject]
        public AccessTokenResponseFactory()
        {

        }

        public IAccessTokenResponse Create(string accessToken, string refreshToken, string tokenType, string scope, DateTime expirationDateUtc)
        {
            return new AccessTokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = tokenType,
                Scope = scope,
                ExpirationDateUtc = expirationDateUtc
            };
        }

        public OAuthWorks.AccessTokenResponseException CreateError(AccessTokenRequestError errorCode, string errorDescription, Uri errorUri, Exception innerException)
        {
            return new AccessTokenResponseException(errorCode, errorDescription, errorUri, innerException);
        }

        public IAccessTokenResponse Create()
        {
            return null;
        }
    }
}
