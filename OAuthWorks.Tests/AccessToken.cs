using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    public class AccessToken : IAccessToken
    {

        public string Id
        {
            get;
            set;
        }

        public IClient Client
        {
            get;
            set;
        }

        public IEnumerable<IScope> Scopes
        {
            get;
            set;
        }

        public IUser User
        {
            get;
            set;
        }

        public string TokenType
        {
            get;
            set;
        }

        public string Token
        {
            get;
            set;
        }

        public bool MatchesValue(string token)
        {
            return this.Token.Equals(token);
        }

        public DateTime ExpirationDateUtc
        {
            get;
            set;
        }

        public bool Expired
        {
            get;
            set;
        }

        public bool Revoked
        {
            get;
            set;
        }

        public void Revoke()
        {
            Revoked = false;
        }
    }
}
