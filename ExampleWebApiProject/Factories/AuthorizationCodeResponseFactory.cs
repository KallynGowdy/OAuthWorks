using OAuthWorks;
using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleWebApiProject.Factories
{
    public class AuthorizationCodeResponseFactory : IAuthorizationCodeResponseFactory<AuthorizationCodeResponse, AuthorizationCodeResponseException>
    {
        public AuthorizationCodeResponse Create(string authorizationCode, string state)
        {
            return new AuthorizationCodeResponse
            {
                Code = authorizationCode,
                State = state
            };
        }

        public AuthorizationCodeResponseException CreateError(AuthorizationRequestCodeErrorType errorCode, string errorDescription, Uri errorUri, string state, Exception innerException)
        {
            throw new NotImplementedException();
        }

        public AuthorizationCodeResponse Create()
        {
            throw new NotImplementedException();
        }
    }
}