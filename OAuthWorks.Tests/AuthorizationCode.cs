using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    public class AuthorizationCode : IAuthorizationCode
    {
        [Inject]
        public AuthorizationCode(string code, string id, Uri redirectUri, DateTime expirationDateUtc, IUser user, IEnumerable<IScope> scopes)
        {
            this.code = code;
            this.Id = id;
            this.ExpirationDateUtc = expirationDateUtc.ToUniversalTime();
            this.Scopes = scopes;
            this.RedirectUri = redirectUri;
            this.User = user;
        }

        public Uri RedirectUri
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        private string code;

        public bool MatchesValue(string code)
        {
            return this.code.Equals(code);
        }

        public bool Expired
        {
            get
            {
                return DateTime.UtcNow >= ExpirationDateUtc;
            }
        }

        public IEnumerable<IScope> Scopes
        {
            get;
            set;
        }

        public DateTime ExpirationDateUtc
        {
            get;
            set;
        }

        public IUser User
        {
            get;
            set;
        }
    }
}
