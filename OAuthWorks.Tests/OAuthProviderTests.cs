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
            OAuthProvider provider = new OAuthProvider
            {
                AccessTokenFactory = new AccessTokenFactory(),
                AccessTokenRepository = new AccessTokenRepository(),
                AccessTokenResponseFactory = new AccessTokenResponseFactory(),
                AuthorizationCodeFactory = new AuthorizationCodeFactory(),
                AuthorizationCodeRepository = new AuthorizationCodeRepository(),
                AuthorizationCodeResponseFactory = new AuthorizationCodeResponseFactory(),
                ClientRepository = new ClientRepository(),
                ScopeRepository = new ScopeRepository(),
                ScopeParser = (p, s) => s.Split(' ', '-').Select(a => p.ScopeRepository.GetById(a)),
                ScopeFormatter = (s) => string.Join(" ", s)
            };

            Scope scope = new Scope
            {
                Id = "exampleScope",
                Description = "The first and only scope that can be requested"
            };

            provider.ScopeRepository.Add(scope);

            Client client = new Client("secret")
            {
                Id = "bob",
                Name = "Client",
                RedirectUris = new string[]
                {
                    "http://example.com/oauth/response/token"
                }
            };

            provider.ClientRepository.Add(client);

            User user = new User
            {
                Id = "Id"
            };

            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest
            {
                ClientId = "bob",
                ClientSecret = "secret",
                RedirectUri = "http://example.com/oauth/response/token",
                ResponseType = AuthorizationCodeResponseType.Code,
                Scope = "exampleScope",
                State = "state"
            };

            var response = provider.InitiateAuthorizationCodeFlow(codeRequest);

            Debug.Assert(response != null && !response.IsError);

            AccessTokenRequest tokenRequest = new AccessTokenRequest
            {
                ClientId = "bob",
                ClientSecret = "secret",
                RedirectUri = "http://example.com/oauth/response/token",
                AuthorizationCode = response.Code
            };

            var tokenResponse = provider.RequestAccessToken(tokenRequest, user);

            Debug.Assert(tokenResponse != null);

            Debug.Assert(provider.HasAccess(user, client, scope));

            Client otherClient = new Client("otherSecret")
            {
                Id = "evilClient",
                Name = "BadGuy",
                RedirectUris = new string[]
                {
                    "http://sealinfosite.com/oauth/giveme/token"
                }
            };

            provider.ClientRepository.Add(otherClient);

            Debug.Assert(!provider.HasAccess(user, otherClient, scope));
        }

    }
}
