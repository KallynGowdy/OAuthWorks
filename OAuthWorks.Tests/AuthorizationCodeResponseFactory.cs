using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class AuthorizationCodeResponseFactory : IAuthorizationCodeResponseFactory<AuthorizationCodeResponse>
    {
        public AuthorizationCodeResponse Create(string authorizationCode, string state)
        {
            return new AuthorizationCodeResponse
            {
                Code = authorizationCode,
                State = state,
                IsError = false
            };
        }

        public AuthorizationCodeResponse Create()
        {
            return null;
        }
    }
}
