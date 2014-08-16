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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExampleMvcWebApplication.Controllers
{
    public class OAuthController : Controller
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
        /// Initializes a new instance of the <see cref="OAuthApiController"/> class.
        /// </summary>
        public OAuthController()
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
        /// Initializes a new instance of the <see cref="OAuthApiController"/> class.
        /// </summary>
        /// <param name="context">The <see cref="DatabaseContext"/> object that should be used for database transactions in this controller.</param>
        /// <param name="provider">The <see cref="IOAuthProvider"/> object that should be used for OAuth 2.0 transactions in this controller.</param>
        public OAuthController(DatabaseContext context, IOAuthProvider provider)
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
        /// Initializes a new instance of the <see cref="OAuthApiController"/> class.
        /// </summary>
        /// <param name="context">The <see cref="IDisposableObject{DatabaseContext}"/> object that should be used for database transactions in this controller and determines whether the underlying <see cref="DatabaseContext"/> should be disposed.</param>
        /// <param name="provider">The <see cref="IDisposableObject{IOAuthProvider}"/> object that should be used for OAuth 2.0 transactions in this controller and determines whether the underlying <see cref="IOAuthProvider"/> should be disposed.</param>
        public OAuthController(IDisposableObject<DatabaseContext> context, IDisposableObject<IOAuthProvider> provider)
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

        // GET: OAuth
        public ActionResult Authorize(IScopeAuthorizationRequest request)
        {
            return View(request);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Authorize(IScopeAuthorizationRequest request, bool authorized)
        {
            if (authorized)
            {
                return RedirectToRoute("/api/v1/requestAuthorizationCode", request);
            }
            return null;
        }
    }
}