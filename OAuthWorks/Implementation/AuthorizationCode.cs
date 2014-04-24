using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines an abstract class that provides a basic implementation of <see cref="OAuthWorks.IAuthorizationCode"/>.
    /// </summary>
    public abstract class AuthorizationCode : IAuthorizationCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCode"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="client">The client.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="expirationDateUtc">The expiration date UTC.</param>
        protected AuthorizationCode(IUser user, IClient client, IEnumerable<IScope> scopes, Uri redirectUri, DateTime expirationDateUtc)
        {
            Contract.Requires(user != null);
            Contract.Requires(client != null);
            Contract.Requires(scopes != null);
            Contract.Requires(redirectUri != null);
            this.User = user;
            this.Client = client;
            this.Scopes = scopes;
            this.RedirectUri = redirectUri;
            this.ExpirationDateUtc = expirationDateUtc;
        }

        /// <summary>
        /// Gets whether this authorization code has expired.
        /// </summary>
        public bool Expired
        {
            get
            {
                return (ExpirationDateUtc - DateTime.Now).TotalSeconds < 0;
            }
        }

        /// <summary>
        /// Gets the redirect Uri that was used by the client when retrieving this token.
        /// </summary>
        public virtual Uri RedirectUri
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the scopes that this code grants access to.
        /// </summary>
        public virtual IEnumerable<IScope> Scopes
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the expiration date of this authorization code in Universal Coordinated Time.
        /// </summary>
        public virtual DateTime ExpirationDateUtc
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the user that this authorization code belongs to.
        /// </summary>
        public virtual IUser User
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the client that this authorization code was granted to.
        /// </summary>
        public virtual IClient Client
        {
            get;
            protected set;
        }

        /// <summary>
        /// Determines if the given token value matches the one stored internally.
        /// </summary>
        /// <param name="token">The token to compare to the internal one.</param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public abstract bool MatchesValue(string token);
    }
}
