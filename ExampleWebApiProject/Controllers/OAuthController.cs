using ExampleWebApiProject.Factories;
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

                    return (AuthorizationCodeResponse)Provider.RequestAuthorizationCode(request, user);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
