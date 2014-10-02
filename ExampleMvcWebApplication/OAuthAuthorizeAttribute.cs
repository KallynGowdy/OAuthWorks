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
using System.Collections;
using ExampleMvcWebApplication.Controllers;
using System.Security.Principal;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net;

namespace ExampleMvcWebApplication
{
    /// <summary>
    /// Defines an <see cref="AuthorizeAttribute"/> that authorizes OAuth clients.
    /// </summary>
    public class OAuthAuthorizeAttribute : AuthorizeAttribute
    {
        private string[] scopes;

        /// <summary>
        /// Gets the list of scopes that are required to access the action.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEnumerable<IScope>> Scopes
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthAuthorizeAttribute" /> class.
        /// </summary>
        /// <param name="scopes">The scopes that are required in order to be authorized.</param>
        public OAuthAuthorizeAttribute(params string[] scopes)
        {
            if (scopes == null) throw new ArgumentNullException("scopes");

            this.scopes = scopes;
            Scopes = null;
        }

        /// <summary>
        /// Gets a value indicating whether [allow multiple].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow multiple]; otherwise, <c>false</c>.
        /// </value>
        public override bool AllowMultiple
        {
            get
            {
                return true;
            }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            IOAuthProvider provider = ((IApiController)actionContext.ControllerContext.Controller).Provider;
            if (Scopes == null)
            {
                Scopes = scopes.Select(s => provider.GetRequestedScopes(s).ToArray()).ToArray();
            }
            AuthenticationHeaderValue authorizationHeader = actionContext.Request.Headers.Authorization;

            if (authorizationHeader == null)
            {
                CookieStore store = new CookieStore(actionContext.Request.Headers.GetValues("Cookie"));

                authorizationHeader = new AuthenticationHeaderValue("Bearer", store["auth_token"]);
            }

            IAuthorizationResult result = provider.ValidateAuthorization(new AuthorizationRequest
            {
                Authorization = authorizationHeader.Parameter,
                Type = authorizationHeader.Scheme,
                RequiredScopes = Scopes
            });


            if (result.IsValidated)
            {
                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(result.Token.User.Id), null);
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, result.ErrorDescription);
            }
        }

        ///// <summary>
        ///// Determines whether the specified action context is authorized.
        ///// </summary>
        ///// <param name="actionContext">The action context.</param>
        ///// <returns>Returns true if the given context is authorized, otherwise returns false.</returns>
        //protected override bool IsAuthorized(HttpActionContext actionContext)
        //{

        //}
    }
}