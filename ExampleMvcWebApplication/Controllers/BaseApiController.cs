using ExampleMvcWebApplication.Models;
using ExampleMvcWebApplication.Repositories;
using OAuthWorks;
using OAuthWorks.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ExampleMvcWebApplication.Controllers
{
    public abstract class BaseApiController : ApiController, IApiController
    {
        IDisposableObject<DatabaseContext> context;

        IDisposableObject<IOAuthProvider> provider;

        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> used in this controller.
        /// </summary>
        /// <returns>Returns the <see cref="DatabaseContext"/> used by this controller.</returns>
        public DatabaseContext Context
        {
            get
            {
                return context.Value;
            }
        }

        private User user;

        /// <summary>
        /// Gets the <see cref="User"/> that is currently in context.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public new User User
        {
            get
            {
                if (user == null && base.User.Identity != null && base.User.Identity.IsAuthenticated)
                {
                    user = Context.Users.Find(base.User.Identity.Name);
                }
                return user;
            }
        }

        /// <summary>
        /// Gets the <see cref="IOAuthProvider"/> used by this controller.
        /// </summary>
        /// <returns>Returns the <see cref="IOAuthProvider"/> used by this controller.</returns>
        public IOAuthProvider Provider
        {
            get
            {
                return provider.Value;
            }
        }

        /// <summary>
        /// Gets whether this controller has been disposed.
        /// </summary>
        /// <returns></returns>
        protected bool Disposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Validates whether the requesting client has access to all of the scopes in a given 'group'
        /// </summary>
        /// <param name="requiredScopes">The list of scope 'groups' that are required for the request to be valid.</param>
        /// <returns></returns>
        protected IAuthorizationResult ValidateAuthorization(params string[] requiredScopes)
        {
            return Provider.ValidateAuthorization(GetAuthorizationRequest(requiredScopes.Select(s => Provider.GetRequestedScopes(s))));
        }

        /// <summary>
        /// Gets the <see cref="IAuthorizationRequest"/> object that can be used to determine what permissions a client has.
        /// </summary>
        /// <param name="requiredScopes">
        /// The groups of required scopes that are needed for the request to be valid. If the client has access to all of the scopes from one of the 'groups', then the
        /// request is authorized.
        /// </param>
        /// <returns></returns>
        protected IAuthorizationRequest GetAuthorizationRequest(params IEnumerable<IScope>[] requiredScopes)
        {
            return GetAuthorizationRequest(requiredScopes.AsEnumerable());
        }

        /// <summary>
        /// Gets the <see cref="IAuthorizationRequest"/> object that can be used to determine what permissions a client has.
        /// </summary>
        /// <param name="requiredScopes">
        /// The groups of required scopes that are needed for the request to be valid. If the client has access to all of the scopes from one of the 'groups', then the
        /// request is authorized.
        /// </param>
        /// <returns></returns>
        protected IAuthorizationRequest GetAuthorizationRequest(IEnumerable<IEnumerable<IScope>> requiredScopes)
        {
            AuthenticationHeaderValue authHeader = Request.Headers.Authorization;

            if(authHeader == null)
            {
                CookieStore cookies = new CookieStore(Request.Headers.GetValues("Cookie"));
                authHeader = new AuthenticationHeaderValue("Bearer", cookies["auth_token"]);
            }

            IAuthorizationRequest authorizationRequest = new AuthorizationRequest
            {
                Authorization = authHeader.Parameter,
                Type = authHeader.Scheme,
                RequiredScopes = requiredScopes
            };

            return authorizationRequest;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController"/> class.
        /// </summary>
        protected BaseApiController()
        {
            this.context = new DisposableObject<DatabaseContext>(new DatabaseContext(), shouldDispose: true);
            this.provider = new DisposableObject<IOAuthProvider>(new OAuthProvider(p =>
            {
                p.AuthorizationCodeRepository = new AuthorizationCodeRepository(context.Value);
                p.ScopeRepository = new ScopeRepository(context.Value);
                p.ClientRepository = new ClientRepository(context.Value);
                p.AccessTokenRepository = new AccessTokenRepository(context.Value);
                p.RefreshTokenRepository = new RefreshTokenRepository(context.Value);
                p.DistributeRefreshTokens = true;
                p.DeleteRevokedTokens = false;
            }), shouldDispose: true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController"/> class.
        /// </summary>
        /// <param name="context">The <see cref="DatabaseContext"/> object that should be used for database transactions in this controller.</param>
        /// <param name="provider">The <see cref="IOAuthProvider"/> object that should be used for OAuth 2.0 transactions in this controller.</param>
        protected BaseApiController(DatabaseContext context, IOAuthProvider provider)
        {
            if (context == null)
            {
                context = new DatabaseContext();
            }
            if (provider == null)
            {
                provider = new OAuthProvider();
            }
            this.context = new DisposableObject<DatabaseContext>(context, shouldDispose: true);
            this.provider = new DisposableObject<IOAuthProvider>(provider, shouldDispose: true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController"/> class.
        /// </summary>
        /// <param name="context">The <see cref="IDisposableObject{DatabaseContext}"/> object that should be used for database transactions in this controller and determines whether the underlying <see cref="DatabaseContext"/> should be disposed.</param>
        /// <param name="provider">The <see cref="IDisposableObject{IOAuthProvider}"/> object that should be used for OAuth 2.0 transactions in this controller and determines whether the underlying <see cref="IOAuthProvider"/> should be disposed.</param>
        protected BaseApiController(IDisposableObject<DatabaseContext> context, IDisposableObject<IOAuthProvider> provider)
        {
            if ((bool @null = context == null) || context.Value == null)
            {
                throw new ArgumentNullException(@null ? "context" : "context.Value");
            }
            if ((bool @null = context == null) || provider.Value == null)
            {
                throw new ArgumentNullException(@null ? "provider" : "provider.Value");
            }
            this.context = context;
            this.provider = provider;
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if this object has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    if (context != null)
                    {
                        context.Dispose();
                        context = null;
                    }
                    if (provider != null)
                    {
                        provider.Dispose();
                        provider = null;
                    }
                }
            }
            base.Dispose(disposing);
            Disposed = true;
        }
    }
}
