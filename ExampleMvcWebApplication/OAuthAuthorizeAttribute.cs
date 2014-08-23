// Copyright 2014 Kallyn Gowdy
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using ExampleMvcWebApplication.Models;
using ExampleMvcWebApplication.Repositories;
using OAuthWorks;
using OAuthWorks.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ExampleMvcWebApplication
{
    /// <summary>
    /// Defines an <see cref="AuthorizeAttribute"/> that authorizes OAuth clients.
    /// </summary>
    public class OAuthAuthorizeAttribute : AuthorizeAttribute, IDisposable
    {
        /// <summary>
        /// Whether or not the <see cref="IOAuthProvider"/> and <see cref="DatabaseContext"/> should be created from the configured 
        /// <see cref="GlobalConfiguration.Configuration.DependencyResolver"/> or not.
        /// </summary>
        public const bool UseDependencyResolver = false;

        /// <summary>
        /// The <see cref="IOAuthProvider"/> that this attribute uses.
        /// </summary>
        private IDisposableObject<IOAuthProvider> provider;

        private IDisposableObject<DatabaseContext> context;

        private bool disposed = false;

        /// <summary>
        /// Gets the list of scopes that are required to access the action.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IScope> Scopes
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthAuthorizeAttribute" /> class.
        /// </summary>
        /// <param name="scopes">The scopes that are required in order to be authorized.</param>
        public OAuthAuthorizeAttribute(string scopes)
        {
            scopes.ThrowIfNull();

            context = new DatabaseContext().AsDisposable();
            provider = UseDependencyResolver ?
                ((IOAuthProvider)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOAuthProvider))).AsDisposable() :
                new OAuthProvider
                (
                    accessTokenRepository: new AccessTokenRepository(context.Value),
                    authorizationCodeRepository: new AuthorizationCodeRepository(context.Value),
                    refreshTokenRepository: new RefreshTokenRepository(context.Value),
                    clientRepository: new ClientRepository(context.Value),
                    scopeRepository: new ScopeRepository(context.Value)
                ).AsDisposable();

            this.Scopes = provider.Value.GetRequestedScopes(scopes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="scopes">The scopes that are required in order to be authorized.</param>
        /// <param name="context">The <see cref="DatabaseContext"/> that should be used.</param>
        /// <param name="oauthProvider">The <see cref="IOAuthProvider"/> that should be used.</param>
        public OAuthAuthorizeAttribute(string scopes, DatabaseContext context, IOAuthProvider oauthProvider)
            : this(scopes, context.AsDisposable(), oauthProvider.AsDisposable())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="scopes">The scopes that are required in order to be authorized.</param>
        /// <param name="context">The <see cref="DatabaseContext"/> that should be used.</param>
        /// <param name="oauthProvider">The <see cref="IOAuthProvider"/> that should be used.</param>
        public OAuthAuthorizeAttribute(string scopes, IDisposableObject<DatabaseContext> context, IDisposableObject<IOAuthProvider> oauthProvider)
        {
            scopes.ThrowIfNull();
            context.ThrowIfNull();
            oauthProvider.ThrowIfNull();

            this.context = context;
            this.provider = oauthProvider;

            this.Scopes = provider.Value.GetRequestedScopes(scopes);
        }

        /// <summary>
        /// Determines whether the specified action context is authorized.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>Returns true if the given context is authorized, otherwise returns false.</returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ThrowIfDisposed();
            AuthenticationHeaderValue authorizationHeader = actionContext.Request.Headers.Authorization;
            if (authorizationHeader == null)
            {
                return provider.Value.ValidateAuthorization(new AuthorizationRequest
                {
                    Authorization = actionContext.Request.Headers.GetValues("Cookie").SelectMany(c => c.Split(';')).First(c => c.Trim().StartsWith("auth_token")).Substring(12),
                    Type = "Bearer",
                    RequiredScopes = Scopes
                }).IsSuccessful;
            }
            else
            {
                return provider.Value.ValidateAuthorization(new AuthorizationRequest
                {
                    Authorization = authorizationHeader.Parameter,
                    Type = authorizationHeader.Scheme,
                    RequiredScopes = Scopes
                }).IsSuccessful;
            }
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if this object has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="OAuthAuthorizeAttribute"/> class.
        /// </summary>
        ~OAuthAuthorizeAttribute()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (provider != null)
                    {
                        provider.Dispose();
                        provider = null;
                    }
                    if (context != null)
                    {
                        context.Dispose();
                        context = null;
                    }
                    if (Scopes != null)
                    {
                        Scopes = null;
                    }
                }
            }
            disposed = true;
        }
    }
}