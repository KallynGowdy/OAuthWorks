using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    public class OAuthProviderTests
    {

        public void TestOAuthProvider()
        {
            var scopes = new ScopeRepository();

            ClientRepository newClientRepository = new ClientRepository();

            AuthorizationCodeResponseFactory newAuthorizationCodeResponseFactory = new AuthorizationCodeResponseFactory();
            AuthorizationCodeRepository newAuthorizationCodeRepository = new AuthorizationCodeRepository();
            AuthorizationCodeFactory newAuthorizationCodeFactory = new AuthorizationCodeFactory();
            AccessTokenResponseFactory newAccessTokenResponseFactory = new AccessTokenResponseFactory();
            AccessTokenRepository newAccessTokenRepository = new AccessTokenRepository();
            AccessTokenFactory newAccessTokenFactory = new AccessTokenFactory();

            OAuthProvider provider = new OAuthProvider
            {
                AccessTokenFactory = newAccessTokenFactory,
                AccessTokenRepository = newAccessTokenRepository,
                AccessTokenResponseFactory = newAccessTokenResponseFactory,
                AuthorizationCodeFactory = newAuthorizationCodeFactory,
                AuthorizationCodeRepository = newAuthorizationCodeRepository,
                AuthorizationCodeResponseFactory = newAuthorizationCodeResponseFactory,
                ClientRepository = newClientRepository,
                ScopeRepository = scopes,
                ScopeParser = (p, s) => s.Split(' ', '-').Select(a => p.ScopeRepository.GetById(a)),
                ScopeFormatter = (s) => string.Join(" ", s)
            };

            Scope scope = new Scope
            {
                Id = "exampleScope",
                Description = "The first and only scope that can be requested"
            };

            scopes.Add(scope);

            Client client = new Client("secret")
            {
                Id = "bob",
                Name = "Client",
                RedirectUris = new Uri[]
                {
                    new Uri("http://example.com/oauth/response/token")
                }
            };

            newClientRepository.Add(client);

            User user = new User
            {
                Id = "Id"
            };

            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest
            {
                ClientId = "bob",
                ClientSecret = "secret",
                RedirectUri = new Uri("http://example.com/oauth/response/token"),
                ResponseType = AuthorizationCodeResponseType.Code,
                Scope = "exampleScope",
                State = "state"
            };

            var response = provider.InitiateAuthorizationCodeFlow(codeRequest);

            Debug.Assert(response != null);

            AccessTokenRequest tokenRequest = new AccessTokenRequest
            {
                ClientId = "bob",
                ClientSecret = "secret",
                RedirectUri = new Uri("http://example.com/oauth/response/token"),
                AuthorizationCode = response.Code
            };

            var tokenResponse = provider.RequestAccessToken(tokenRequest, user);

            Debug.Assert(tokenResponse != null);

            Debug.Assert(provider.HasAccess(user, client, scope));

            Client otherClient = new Client("otherSecret")
            {
                Id = "evilClient",
                Name = "BadGuy",
                RedirectUris = new Uri[]
                {
                    new Uri("http://sealinfosite.com/oauth/giveme/token")
                }
            };

            newClientRepository.Add(otherClient);

            Debug.Assert(!provider.HasAccess(user, otherClient, scope));
        }

    }
}
