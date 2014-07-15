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
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ExampleWebApiProject.Controllers
{
    public class OAuthApiController : ApiController
    {
        public OAuthProvider Provider
        {
            get;
            private set;
        }

        public OAuthApiController()
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
                return AddHeadersToTokenResponse(r);
            }
        }

        private static HttpResponseMessage AddHeadersToTokenResponse(HttpResponseMessage r)
        {
            r.Headers.Add("Cache-Control", "no-store");
            r.Headers.Add("Pragma", "no-cache");
            return r;
        }

        /// <summary>
        /// Defines a DTO (Data Transfer Object) that is used for Resource Owner Password Credentials access token requests.
        /// </summary>
        [DataContract]
        public class CredentialsTokenRequest
        {
            /// <summary>
            /// The username of the resource owner.
            /// </summary>
            /// <returns></returns>
            [DataMember(Name = "username")]
            public string Username
            {
                get;
                set;
            }

            /// <summary>
            /// The password of the resource owner.
            /// </summary>
            /// <returns></returns>
            [DataMember(Name = "password")]
            public string Password
            {
                get;
                set;
            }

            /// <summary>
            /// The ID of the client requesting the access token.
            /// </summary>
            /// <returns></returns>
            [DataMember(Name = "client_id")]
            public string ClientId
            {
                get;
                set;
            }

            /// <summary>
            /// The secret/password of the client requesting the access token.
            /// </summary>
            /// <returns></returns>
            [DataMember(Name = "client_secret")]
            public string ClientSecret
            {
                get;
                set;
            }

            /// <summary>
            /// The scope(s) that the client is requesting authorization for.
            /// </summary>
            /// <returns></returns>
            [DataMember(Name = "scope")]
            public string Scope
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            [DataMember(Name = "redirect_uri")]
            public Uri RedirectUri
            {
                get;
                set;
            }

            [DataMember(Name = "grant_type")]
            public string GrantType
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Requests an access token from the authorization server using the Resource Owner Password Credentials OAuth 2.0 flow.
        /// </summary>
        /// <param name="request">The request that contains the </param>
        /// <returns></returns>
        [Route("api/v1/accessToken")]
        [HttpPost]
        public async Task<HttpResponseMessage> RequestAccessToken(CredentialsTokenRequest request)
        {
            using (DatabaseContext context = new DatabaseContext()) // Create the unit-of-work
            using (Provider = new OAuthProvider // Create the provider for the request
            {
                AccessTokenRepository = new AccessTokenRepository(context), // Pass in our repositories and arguments
                AuthorizationCodeRepository = new AuthorizationCodeRepository(context),
                RefreshTokenRepository = new RefreshTokenRepository(context),
                ClientRepository = new ClientRepository(context),
                ScopeRepository = new ScopeRepository(context),
                DistributeRefreshTokens = true,
                DeleteRevokedTokens = true
            })
            {
                User user = context.Users.FirstOrDefault(u => u.Id.Equals(request.Username)); // Get the user based on the username given in the request
                if (user == null || !user.IsValidPassword(request.Password)) // Validate the given credentials
                {
                    user = null; // Set the user to null if the credentials are bad so the provider can handle creating the correct response
                }
                IAccessTokenResponse response = Provider.RequestAccessToken
                (
                    new PasswordCredentialsAccessTokenRequest // Send the request to the provider
                    (
                        user,
                        request.ClientId, 
                        request.ClientSecret, 
                        request.GrantType, 
                        request.RedirectUri, 
                        request.Scope
                    )
                );
                await context.SaveChangesAsync(); // Save any changes that the provider made to the database
                HttpResponseMessage r = Request.CreateResponse(response.StatusCode(), response);
                return AddHeadersToTokenResponse(r); // Add the headers and return the response
            }
        }

        [Route("api/v1/refreshToken")]
        [HttpPost]
        public async Task<HttpResponseMessage> RefreshAccessToken(TokenRefreshRequest request)
        {
            using (DatabaseContext context = new DatabaseContext()) // Create unit-of-work
            using (Provider = new OAuthProvider // Create provider for request
            {
                AccessTokenRepository = new AccessTokenRepository(context),
                RefreshTokenRepository = new RefreshTokenRepository(context),
                ClientRepository = new ClientRepository(context),
                ScopeRepository = new ScopeRepository(context),
                DistributeRefreshTokens = true,
                DeleteRevokedTokens = true
            })
            {
                IAccessTokenResponse response = Provider.RefreshAccessToken(request); // Process refresh token request
                await context.SaveChangesAsync(); // Save transaction
                HttpResponseMessage r = Request.CreateResponse(response.StatusCode(), response);
                return AddHeadersToTokenResponse(r); // Add required headers and return
            }
        }
    }
}
