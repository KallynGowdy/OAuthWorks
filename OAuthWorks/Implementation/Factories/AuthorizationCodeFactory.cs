using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Factories
{
    /// <summary>
    /// Defines a class which provides a basic implementation of <see cref="OAuthWorks.IAuthorizationCodeFactory"/>.
    /// </summary>
    public class AuthorizationCodeFactory : IAuthorizationCodeFactory<HashedAuthorizationCode>
    {
        /// <summary>
        /// The default length of the generated codes in bytes.
        /// </summary>
        public const int DefaultCodeLength = 28;

        /// <summary>
        /// The default lifetime for generated authorization codes.
        /// </summary>
        /// <value>3,600 seconds or 1 hour</value>
        public const int DefaultCodeLifetime = 3600;

        /// <summary>
        /// Gets the length of the codes(in bytes) generated from this factory.
        /// </summary>
        public int CodeLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the lifetime of the generated codes in seconds.
        /// </summary>
        public int CodeLifetime
        {
            get;
            private set;
        }

        public AuthorizationCodeFactory()
            : this(DefaultCodeLength, DefaultCodeLifetime)
        { }

        public AuthorizationCodeFactory(int codeLength, int codeLifetime)
        {
            Contract.Requires(codeLength >= 20);
            Contract.Requires(codeLifetime > 0);
            this.CodeLength = codeLength;
            this.CodeLifetime = codeLifetime;
        }

        /// <summary>
        /// Creates a new <see cref="OAuthWorks.IAuthorizationCode"/> object given the granted scopes.
        /// </summary>
        /// <param name="scopes">The enumerable list of scopes that were granted by the user to the client.</param>
        /// <param name="user">The user that the created authorization code is bound to.</param>
        /// <param name="client">The client that the code is granted for.</param>
        /// <param name="redirectUri">The URI that was provided by the client that specifies the location that the user should be redirected to after completing authorization.</param>
        /// <returns>Returns a new OAuthWorks.CreatedToken(of TAuthorizationCode) object.</returns>
        public ICreatedToken<HashedAuthorizationCode> Create(Uri redirectUri, IUser user, IClient client, IEnumerable<IScope> scopes)
        {
            string token = AccessTokenFactory.GenerateToken(CodeLength);
            string id = token.Substring(0, 8);
            DateTime expirationDate = DateTime.UtcNow.AddSeconds(CodeLifetime);
            return new CreatedToken<HashedAuthorizationCode>(new HashedAuthorizationCode(id, token, user, client, scopes, redirectUri, expirationDate), token);
        }

        public HashedAuthorizationCode Create()
        {
            return null;
        }
    }
}
