using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class Client : IClient
    {
        public Client(string secret)
        {
            this.secret = secret;
        }

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public IEnumerable<Uri> RedirectUris
        {
            get;
            set;
        }

        private string secret;

        public bool IsValidRedirectUri(Uri redirectUri)
        {
            return RedirectUris.Any(r => r.Equals(redirectUri));
        }

        public bool MatchesSecret(string secret)
        {
            return this.secret.Equals(secret);
        }

        public bool Equals(IClient other)
        {
            return other != null && other is Client && this.Id.Equals(((Client)other).Id);
        }
    }
}
