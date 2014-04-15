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


using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    [TestFixture]
    public class OAuthProviderTests
    {
        OAuthProvider provider;
        DependencyInjector dependencyInjector = new DependencyInjector();

        Scope exampleScope;

        Scope otherScope;

        Client client;

        [TestFixtureSetUp]
        public void SetUp()
        {
            provider = new OAuthProvider(dependencyInjector)
            {
                ScopeParser = (p, s) => s.Split(' ', '-').Select(a => p.ScopeRepository.GetById(a)),
                ScopeFormatter = (s) => string.Join(" ", s)
            };

            exampleScope = new Scope
            {
                Id = "exampleScope",
                Description = "The first scope that can be requested"
            };

            otherScope = new Scope
            {
                Id = "otherScope",
                Description = "Another scope that can be requested."
            };

            ((ScopeRepository)provider.ScopeRepository).Add(exampleScope);
            ((ScopeRepository)provider.ScopeRepository).Add(otherScope);

            client = new Client("secret")
            {
                Id = "bob",
                Name = "Client",
                RedirectUris = new Uri[]
                {
                    new Uri("http://example.com/oauth/response/token")
                }
            };

            ((ClientRepository)provider.ClientRepository).Add(client);
        }

        [TestCase("exampleScope", "state", "secret", "http://example.com/oauth/response/token")]
        [TestCase("nonExistantScope", "state", "secret", "http://example.com/oauth/response/token")]
        [TestCase("otherScope, exampleScope", "state", "secret", "http://example.com/oauth/response/token")]
        [TestCase("", "state", "secret", "http://example.com/oauth/response/token")]
        public void TestGetRequestedScopes(string scope, string state, string secret, string redirectUri)
        {
            AuthorizationCodeRequest request = new AuthorizationCodeRequest
            {
                ClientId = client.Id,
                ClientSecret = secret,
                RedirectUri = new Uri(redirectUri),
                ResponseType = AuthorizationCodeResponseType.Code,
                Scope = scope,
                State = state
            };

            var scopes = provider.GetRequestedScopes(request);

            if (scopes != null)
            {
                Assert.True(scopes.All(s => s != null) && scopes.Count() > 0);
            }
            else
            {
                Assert.True(provider.ScopeParser(provider, scope).Any(s => s == null));
            }
        }

        [TestCase("exampleScope", "state", "secret", "http://example.com/oauth/response/token")]
        [TestCase("nonExistantScope", "state", "secret", "http://example.com/oauth/response/token")]
        [TestCase("exampleScope", "state", "badSecret", "http://example.com/oauth/response/token")]
        [TestCase("exampleScope", "state", "secret", "http://example.com/oauth/v1/response/token")]
        public void TestRequestAuthorizationCode(string scope, string state, string secret, string redirectUri)
        {
            User user = new User
            {
                Id = "Id"
            };

            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest
            {
                ClientId = client.Id,
                ClientSecret = secret,
                RedirectUri = new Uri(redirectUri),
                ResponseType = AuthorizationCodeResponseType.Code,
                Scope = scope,
                State = state
            };

            try
            {
                var response = provider.RequestAuthorizationCode(codeRequest, user);

                Assert.NotNull(response);

                Assert.AreEqual(response.State, state);

                Assert.True(client.IsValidRedirectUri(new Uri(redirectUri)), "The client did not have a valid redirect Uri even though it was able to retrieve a code.");
                Assert.True(client.MatchesSecret(secret), "The client's secret was invalid even though it got through.");
                Assert.NotNull(provider.GetRequestedScopes(codeRequest), "The requested scope was invalid even though it got through.");
            }
            catch (AuthorizationCodeResponseException e)
            {
                switch (e.ErrorCode)
                {
                    case AuthorizationRequestCodeErrorType.UnauthorizedClient:
                        Assert.False(client.MatchesSecret(secret));
                        break;
                    case AuthorizationRequestCodeErrorType.InvalidScope:
                        Assert.Null(provider.GetRequestedScopes(codeRequest));
                        break;
                    case AuthorizationRequestCodeErrorType.InvalidRequest:
                        Assert.False(client.IsValidRedirectUri(codeRequest.RedirectUri));
                        break;
                }
            }
        }

        [Test]
        public void TestOAuthProvider()
        {
            User user = new User
            {
                Id = "Id"
            };

            //A request from the client for an authorization code.
            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest
            {
                ClientId = "bob",
                ClientSecret = "secret",
                RedirectUri = new Uri("http://example.com/oauth/response/token"),
                ResponseType = AuthorizationCodeResponseType.Code,
                Scope = "exampleScope",
                State = "state"
            };

            //Normally you would authorize the client and user here.
            //You would also determine if the user needs to provide consent using
            //provider.HasAccess(user, client, scope)

            //Then retrieve the 
            IAuthorizationCodeResponse response = provider.RequestAuthorizationCode(codeRequest, user);

            Assert.NotNull(response);

            AccessTokenRequest tokenRequest = new AccessTokenRequest
            {
                ClientId = "bob",
                ClientSecret = "secret",
                RedirectUri = new Uri("http://example.com/oauth/response/token"),
                AuthorizationCode = response.Code
            };

            var tokenResponse = provider.RequestAccessToken(tokenRequest, user);

            Assert.NotNull(tokenResponse);

            Debug.Assert(tokenResponse != null);

            Assert.True(provider.HasAccess(user, client, exampleScope), "The provider does not have access when it should");

            Client otherClient = new Client("otherSecret")
            {
                Id = "evilClient",
                Name = "BadGuy",
                RedirectUris = new Uri[]
                {
                    new Uri("http://sealinfosite.com/oauth/giveme/token")
                }
            };

            ((ClientRepository)provider.ClientRepository).Add(otherClient);

            Assert.False(provider.HasAccess(user, otherClient, exampleScope), "The unathorized client had access when it shouldn't have.");
        }

    }
}
