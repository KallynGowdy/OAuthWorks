using ExampleWebApiProject.Factories;
using ExampleWebApiProject.Models;
using ExampleWebApiProject.Repositories;
using OAuthWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        public AuthorizationCodeResponse RequestAuthorizationCode(string clientId, string clientSecret, string redirectUri, AuthorizationCodeResponseType responseType, string scope, string state)
        {
            using (DatabaseContext context = new DatabaseContext())
            using (Provider = new OAuthProvider
            {
                AuthorizationCodeFactory = new AuthorizationCodeFactory(),
                AuthorizationCodeRepository = new AuthorizationCodeRepository(context),
                AuthorizationCodeResponseFactory = new AuthorizationCodeResponseFactory(),
                ScopeRepository = new ScopeRepository(context),
                ClientRepository = new ClientRepository(context)
            })
            {

                AuthorizationCodeRequest request = new AuthorizationCodeRequest
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    RedirectUri = new Uri(redirectUri),
                    ResponseType = responseType,
                    Scope = scope,
                    State = state
                };

                User user = new Models.User
                {
                    Id = "Id"
                };

                return (AuthorizationCodeResponse)Provider.RequestAuthorizationCode(request, user);
            }
        }
    }
}
