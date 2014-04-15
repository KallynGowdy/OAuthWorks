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
        public AuthorizationCode(string code, string id, Uri redirectUri, DateTime expirationDateUtc, IUser user, IClient client, IEnumerable<IScope> scopes)
        {
            this.code = code;
            this.Id = id;
            this.ExpirationDateUtc = expirationDateUtc.ToUniversalTime();
            this.Scopes = scopes;
            this.RedirectUri = redirectUri;
            this.User = user;
            this.Client = client;
        }

        /// <summary>
        /// Gets the redirect Uri that was used by the client when retrieving this token.
        /// </summary>
        public Uri RedirectUri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
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

        /// <summary>
        /// Gets whether this authorization code has expired.
        /// </summary>
        public bool Expired
        {
            get
            {
                return DateTime.UtcNow >= ExpirationDateUtc;
            }
        }

        /// <summary>
        /// Gets the scopes that this code grants access to.
        /// </summary>
        public IEnumerable<IScope> Scopes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the expiration date of this authorization code in Universal Coordinated Time.
        /// </summary>
        public DateTime ExpirationDateUtc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the user that this authorization code belongs to.
        /// </summary>
        public IUser User
        {
            get;
            set;
        }


        /// <summary>
        /// Gets the client that this authorization code was granted to.
        /// </summary>
        public IClient Client
        {
            get;
            set;
        }
    }
}
