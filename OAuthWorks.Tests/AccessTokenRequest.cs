using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    public class AccessTokenRequest : IAccessTokenRequest
    {
        public string AuthorizationCode
        {
            get;
            set;
        }

        public string ClientId
        {
            get;
            set;
        }

        public Uri RedirectUri
        {
            get;
            set;
        }

        public string ClientSecret
        {
            get;
            set;
        }
    }
}
