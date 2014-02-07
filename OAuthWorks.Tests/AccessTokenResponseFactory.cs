using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class AccessTokenResponseFactory : IAccessTokenResponseFactory<AccessTokenResponse, AccessTokenResponseException>
    {
        public AccessTokenResponse Create(string accessToken, string refreshToken, string tokenType, string scope, DateTime expirationDateUtc)
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

        public AccessTokenResponseException CreateError(AccessTokenRequestError errorCode, string errorDescription, string errorUri, Exception innerException)
        {
            return new AccessTokenResponseException(errorCode, errorDescription, errorUri, innerException);
        }

        public AccessTokenResponse Create()
        {
            return null;
        }
    }
}
