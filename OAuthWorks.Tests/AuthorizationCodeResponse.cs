using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class AuthorizationCodeResponse : IAuthorizationCodeResponse
    {
        public string Code
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public bool IsError
        {
            get;
            set;
        }

        public IAuthorizationCodeResponseError ErrorCode
        {
            get;
            set;
        }
    }
}
