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

using ExampleWebApiProject.Models;
using ExampleWebApiProject.Repositories;
using OAuthWorks;
using OAuthWorks.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using System.Web.Http.Results;

namespace ExampleWebApiProject.Controllers
{
    public class OAuthController : ApiController
    {
        public OAuthProvider Provider
        {
            get;
            private set;
        }

        public OAuthController()
        {
        }

        [Route("api/v1/authorizationCode")]
        [HttpGet]
        public IHttpActionResult RequestAuthorizationCode(string clientId, string clientSecret, string redirectUri, AuthorizationCodeResponseType responseType, string scope, string state)
        {
            using (DatabaseContext context = new DatabaseContext())
            using (Provider = new OAuthProvider(p =>
            {
                p.AuthorizationCodeRepository = new AuthorizationCodeRepository(context);
                p.ScopeRepository = new ScopeRepository(context);
                p.ClientRepository = new ClientRepository(context);
            }))
            {

                AuthorizationCodeRequest request = new AuthorizationCodeRequest
                (
                    clientId: clientId,
                    clientSecret: clientSecret,
                    redirectUri: new Uri(redirectUri),
                    responseType: responseType,
                    scope: scope,
                    state: state
                );

                if (User.Identity.IsAuthenticated)
                {
                    User user = context.Users.Find(User.Identity.Name);

                    IAuthorizationCodeResponse response = Provider.RequestAuthorizationCode(request, user);
                    context.SaveChanges();
                    if (response.ShouldRedirect())
                    {
                        return Redirect(response.Redirect);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return RedirectToRoute("/users/login", new { @return = Request.RequestUri.AbsoluteUri });
                }
            }
        }

        [Route("api/v1/accessToken")]
        [HttpPost]
        public HttpResponseMessage RequestAccessToken(AuthorizationCodeGrantAccessTokenRequest request)
        {
            using (DatabaseContext context = new DatabaseContext())
            using (Provider = new OAuthProvider
            {
                AccessTokenRepository = new AccessTokenRepository(context),
                AuthorizationCodeRepository = new AuthorizationCodeRepository(context),
                RefreshTokenRepository = new RefreshTokenRepository(context),
                ClientRepository = new ClientRepository(context),
                ScopeRepository = new ScopeRepository(context),
                DistributeRefreshTokens = true,
                DeleteRevokedTokens = true
            })
            {
                IAccessTokenResponse response = Provider.RequestAccessToken(request);
                context.SaveChanges();
                var r = Request.CreateResponse(response.StatusCode(), response);
                r.Headers.Add("Cache-Control", "no-store");
                r.Headers.Add("Pragma", "no-cache");
                return r;
            }
        }

        [Route("api/v1/refreshToken")]
        [HttpPost]
        public HttpResponseMessage RefreshAccessToken(TokenRefreshRequest request)
        {
            using (DatabaseContext context = new DatabaseContext())
            using (Provider = new OAuthProvider
            {
                AccessTokenRepository = new AccessTokenRepository(context),
                RefreshTokenRepository = new RefreshTokenRepository(context),
                ClientRepository = new ClientRepository(context),
                ScopeRepository = new ScopeRepository(context),
                DistributeRefreshTokens = true,
                DeleteRevokedTokens = true
            })
            {
                IAccessTokenResponse response = Provider.RefreshAccessToken(request);
                context.SaveChanges();
                return Request.CreateResponse(response.StatusCode(), response);
            }
        }
    }
}
