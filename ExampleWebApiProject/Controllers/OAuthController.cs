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
<<<<<<< Updated upstream
        public AuthorizationCodeResponse RequestAuthorizationCode(string clientId, string clientSecret, string redirectUri, AuthorizationCodeResponseType responseType, string scope, string state)
=======
        public IAuthorizationCodeResponse RequestAuthorizationCode(string clientId, string clientSecret, string redirectUri, AuthorizationCodeResponseType responseType, string scope, string state)
>>>>>>> Stashed changes
        {
            using (DatabaseContext context = new DatabaseContext())
            using (Provider = new OAuthProvider
            {
                AuthorizationCodeFactory = new AuthorizationCodeFactory(),
                AuthorizationCodeRepository = new AuthorizationCodeRepository(context),
<<<<<<< Updated upstream
                AuthorizationCodeResponseFactory = new AuthorizationCodeResponseFactory(),
=======
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
                    return (AuthorizationCodeResponse)Provider.RequestAuthorizationCode(request, user);
=======
                    IAuthorizationCodeResponse response = Provider.RequestAuthorizationCode(request, user);
                    context.SaveChanges();
                    return response;
>>>>>>> Stashed changes
                }
                else
                {
                    return null;
                }
            }
        }
<<<<<<< Updated upstream
=======

        [Route("api/v1/accessToken")]
        [HttpPost]
        public IAccessTokenResponse RequestAccessToken(AuthorizationCodeGrantAccessTokenRequest request)
        {
            using (DatabaseContext context = new DatabaseContext())
            using (Provider = new OAuthProvider
            {
                AccessTokenRepository = new AccessTokenRepository(context),
                AuthorizationCodeRepository = new AuthorizationCodeRepository(context),
                ClientRepository = new ClientRepository(context),
                ScopeRepository = new ScopeRepository(context)

            })
            {
                IAccessTokenResponse response = Provider.RequestAccessToken(request);
                context.SaveChanges();
                return response;
            }
        }
>>>>>>> Stashed changes
    }
}
