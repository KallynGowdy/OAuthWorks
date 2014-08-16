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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using System.Web.Http.Results;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ExampleMvcWebApplication.Controllers
{
    /// <summary>
    /// Defines a controller that provides OAuth 2.0 endpoints through the use of an API.
    /// </summary>
    public class OAuthApiController : ApiController
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
        public OAuthApiController()
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
        public OAuthApiController(DatabaseContext context, IOAuthProvider provider)
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
        public OAuthApiController(IDisposableObject<DatabaseContext> context, IDisposableObject<IOAuthProvider> provider)
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
        /// Requests an Authorization Code that can be used to retrieve an access token from the server for the client with the given client id and client secret.
        /// </summary>
        /// <param name="clientId">The ID of the client that is requesting the authorization code.</param>
        /// <param name="redirectUri">The URI that the user should be redirected to upon successful distribution of the authorization code.</param>
        /// <param name="responseType">The type of authorization code response that should be returned, this is always 'code'.</param>
        /// <param name="scope">The scope of access requested by the client.</param>
        /// <param name="state">An opaque value used by the client to maintain state between the request and the callback. Used to prevent Cross Site Request Forgery.</param>
        /// <returns>Returns a <see cref="IAuthorizationCodeResponse"/> object that contains the granted authorization code.</returns>
        [Route("api/v1/authorizationCode")]
        [HttpGet]
        public IHttpActionResult RequestAuthorizationCode(string clientId, string redirectUri, AuthorizationCodeResponseType responseType, string scope, string state)
        {
            AuthorizationCodeRequest request = new AuthorizationCodeRequest
            (
                clientId: clientId,
                redirectUri: redirectUri != null ? new Uri(redirectUri) : null,
                responseType: responseType,
                scope: scope,
                state: state
            );

            if (User.Identity.IsAuthenticated) // Check for logged in user
            {
                User user = Context.Users.Find(User.Identity.Name);

                IAuthorizationCodeResponse response = Provider.RequestAuthorizationCode(request, user); // Issue authorization code
                Context.SaveChanges();
                if (response.ShouldRedirect())
                {
                    return Redirect(response.Redirect);
                }
                else if ((var scopeRequest = response.GetScopeAuthorizationRequest()) != null)
                {
                    return RedirectToRoute("/oauth/authorize", new { request = scopeRequest });
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return RedirectToRoute("/users/login", new { @return = Request.RequestUri.AbsoluteUri }); // Redirect to login
            }
        }

        /// <summary>
        /// Requests an access token from the server using the given <see cref="AuthorizationCodeGrantAccessTokenRequest"/>.
        /// </summary>
        /// <param name="request">The request containing the previously obtained authorization code.</param>
        /// <returns>Returns a <see cref="IAccessTokenResponse"/> object that contains the access token and optional refresh token.</returns>
        [Route("api/v1/accessToken")]
        [HttpPost]
        public HttpResponseMessage RequestAccessToken(AuthorizationCodeGrantAccessTokenRequest request)
        {
            IAccessTokenResponse response = Provider.RequestAccessToken(request);
            Context.SaveChanges();
            var r = Request.CreateResponse(response.StatusCode(), response);
            return AddHeadersToTokenResponse(r);
        }

        /// <summary>
        /// Adds the required headers to the given response specifiying that the user agent shouldn't cache the response at all.
        /// </summary>
        /// <param name="r">The message that the headers should be added to.</param>
        /// <returns>Returns the message with the headers added.</returns>
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
        /// <param name="request">The request that contains the user credentials for the authorization request.</param>
        /// <returns></returns>
        [Route("api/v1/accessToken")]
        [HttpPost]
        public async Task<HttpResponseMessage> RequestAccessToken(CredentialsTokenRequest request)
        {
            using (DatabaseContext context = new DatabaseContext()) // Create the unit-of-work
            using (IOAuthProvider Provider = new OAuthProvider // Create the provider for the request
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

        /// <summary>
        /// Refreshes an access token using the given <see cref="TokenRefreshRequest"/>.
        /// </summary>
        /// <param name="request">The refresh access token request containing the granted refresh token.</param>
        /// <returns>Returns a new <see cref="IAccessTokenResponse"/> containing the new access token and refresh token.</returns>
        [Route("api/v1/refreshToken")]
        [HttpPost]
        public async Task<HttpResponseMessage> RefreshAccessToken(TokenRefreshRequest request)
        {
            IAccessTokenResponse response = Provider.RefreshAccessToken(request); // Process refresh token request
            await Context.SaveChangesAsync(); // Save transaction
            HttpResponseMessage r = Request.CreateResponse(response.StatusCode(), response);
            return AddHeadersToTokenResponse(r); // Add required headers and return
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
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
                    context.Dispose();
                    context = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
