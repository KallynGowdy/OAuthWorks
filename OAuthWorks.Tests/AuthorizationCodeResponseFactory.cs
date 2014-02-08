using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class AuthorizationCodeResponseFactory : IAuthorizationCodeResponseFactory<AuthorizationCodeResponse, AuthorizationCodeResponseException>
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

        class AuthorizationCodeException : AuthorizationCodeResponseException
        {

            public AuthorizationCodeException(AuthorizationRequestCodeErrorType errorCode, string desc, Uri uri, string state, Exception innerException = null)
                : base(desc, innerException)
            {
                this.ErrorCode = errorCode;
                this.ErrorDescription = desc;
                this.ErrorUri = uri;
                this.State = state;
            }

            public override AuthorizationRequestCodeErrorType ErrorCode
            {
                get;
                protected set;
            }

            public override string ErrorDescription
            {
                get;
                protected set;
            }

            public override Uri ErrorUri
            {
                get;
                protected set;
            }

            public override string State
            {
                get;
                protected set;
            }
        }

        public AuthorizationCodeResponseException CreateError(AuthorizationRequestCodeErrorType errorCode, string errorDescription, Uri errorUri, string state, Exception innerException)
        {
            return new AuthorizationCodeException(errorCode, errorDescription, errorUri, state, innerException);
        }

        public AuthorizationCodeResponse Create()
        {
            return null;
        }
    }
}
