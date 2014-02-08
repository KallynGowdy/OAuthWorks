using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    public class AuthorizationCodeRequest : IAuthorizationCodeRequest
    {
        public AuthorizationCodeResponseType ResponseType
        {
            get;
            set;
        }

        public string ClientId
        {
            get;
            set;
        }

        public string ClientSecret
        {
            get;
            set;
        }

        public Uri RedirectUri
        {
            get;
            set;
        }

        public string Scope
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }
    }
}
