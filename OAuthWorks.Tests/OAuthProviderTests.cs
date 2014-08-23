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
using Ninject;
using OAuthWorks.Implementation;

namespace OAuthWorks.Tests
{
    [TestFixture]
    public class OAuthProviderTests
    {
        private const string clientId = "bob";
        private const string exampleScopeId = "exampleScope";
        private const string otherScopeId = "otherScope";
        private const string clientName = "Client";
        private const string badClientId = "bill";
        private const string badClientName = "Evil";
        private const string clientRedirectUri = "http://example.com/oauth/response/token";
        private const string secret = "secret";
        private const string otherSecret = "otherSecret";
        private const string code = "code";
        private const string access_token = "access_token";
        private const string not_access_token = "not_access_token";
        OAuthProvider provider;
        DependencyInjector dependencyInjector = new DependencyInjector();

        Scope exampleScope;

        Scope otherScope;

        Client client;

        Client badClient;

        [TestFixtureSetUp]
        public void SetUp()
        {
            provider = (OAuthProvider)DependencyInjector.Kernel.Get<IOAuthProvider>();
            provider.ScopeParser = (p, s) => s.Split(' ', '-').Select(a => p.ScopeRepository.GetById(a));
            provider.ScopeFormatter = (s) => string.Join(" ", s.Select(sc => sc.Id));
            provider.DistributeRefreshTokens = true;
            provider.DeleteRevokedTokens = true;

            exampleScope = new Scope
            {
                Id = exampleScopeId,
                Description = "The first scope that can be requested"
            };

            otherScope = new Scope
            {
                Id = otherScopeId,
                Description = "Another scope that can be requested."
            };

            ((ScopeRepository)provider.ScopeRepository).Add(exampleScope);
            ((ScopeRepository)provider.ScopeRepository).Add(otherScope);

            client = new Client(secret)
            {
                Id = clientId,
                Name = clientName,
                RedirectUris = new Uri[]
                {
                    new Uri(clientRedirectUri)
                }
            };

            badClient = new Client(otherSecret)
            {
                Id = badClientId,
                Name = badClientName,
                RedirectUris = new Uri[]
                {
                    new Uri(clientRedirectUri)
                }
            };

            ((ClientRepository)provider.ClientRepository).Add(client);
            ((ClientRepository)provider.ClientRepository).Add(badClient);
        }

        private class InvalidAccessTokenRequest : IAuthorizationCodeGrantAccessTokenRequest
        {
            public string AuthorizationCode
            {
                get;
                set;
            }

            public string ClientId
            {
                get;
                set;
            }

            public string ClientSecret
            {
                get;
                set;
            }

            public string GrantType
            {
                get;
                set;
            }

            public Uri RedirectUri
            {
                get;
                set;
            }

            public string Scope
            {
                get;
                set;
            }
        }

        [Test]
        [TestCase(null, secret, code, access_token, clientRedirectUri, AccessTokenSpecificRequestError.NullClientId)]
        [TestCase("", secret, code, access_token, clientRedirectUri, AccessTokenSpecificRequestError.NullClientId)]
        [TestCase(clientId, null, code, access_token, clientRedirectUri, AccessTokenSpecificRequestError.NullClientSecret)]
        [TestCase(clientId, "", code, access_token, clientRedirectUri, AccessTokenSpecificRequestError.NullClientSecret)]
        [TestCase(clientId, secret, null, access_token, clientRedirectUri, AccessTokenSpecificRequestError.NullAuthorizationCode)]
        [TestCase(clientId, secret, "", access_token, clientRedirectUri, AccessTokenSpecificRequestError.NullAuthorizationCode)]
        [TestCase(clientId, secret, code, access_token, null, AccessTokenSpecificRequestError.NullRedirectUri)]
        [TestCase(clientId, secret, code, null, clientRedirectUri, AccessTokenSpecificRequestError.NullGrantType)]
        [TestCase(clientId, secret, code, "", clientRedirectUri, AccessTokenSpecificRequestError.NullGrantType)]
        [TestCase(clientId, secret, code, not_access_token, clientRedirectUri, AccessTokenSpecificRequestError.InvalidGrantType)]
        public void TestInvalidAccessTokenRequest(string clientId, string clientSecret, string authorizationCode, string grantType, string redirectUri, AccessTokenSpecificRequestError expectedError)
        {
            var request = new InvalidAccessTokenRequest { ClientId = clientId, ClientSecret = clientSecret, AuthorizationCode = authorizationCode, GrantType = grantType, RedirectUri = (redirectUri != null ? new Uri(redirectUri) : null) };

            IUnsuccessfulAccessTokenResponse response = provider.RequestAccessToken(request) as IUnsuccessfulAccessTokenResponse;

            Assert.NotNull(response);
            Assert.AreEqual(expectedError, response.SpecificError);
        }

        [Test]
        public void TestRefreshAccessToken()
        {
            User user = new User
            {
                Id = "Id"
            };

            ICreatedToken<IAccessToken> oldToken = provider.AccessTokenFactory.Create(client, user, new[] { exampleScope });
            provider.AccessTokenRepository.Add(oldToken);

            ICreatedToken<IRefreshToken> refresh = provider.RefreshTokenFactory.Create(client, user, new[] { exampleScope });
            provider.RefreshTokenRepository.Add(refresh);

            ITokenRefreshRequest refreshRequest = new TokenRefreshRequest
            (
                refresh.TokenValue,
                clientId,
                "secret",
                "refresh_token",
                exampleScopeId
            );

            ISuccessfulAccessTokenResponse response = provider.RefreshAccessToken(refreshRequest) as ISuccessfulAccessTokenResponse;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.AccessToken, Is.Not.Null);
            Assert.That(response.RefreshToken, Is.Not.Null);
            Assert.That(response.Scope, Is.EqualTo(exampleScopeId));
            Assert.That(response.TokenType, Is.EqualTo("Bearer"));
            Assert.That(oldToken.Token.IsValid(), Is.False);
            Assert.That(refresh.Token.IsValid(), Is.False);
        }

        [Test]
        [TestCase(exampleScopeId, "state", "secret", clientRedirectUri)]
        [TestCase("nonExistantScope", "state", "secret", clientRedirectUri)]
        [TestCase("otherScope, exampleScope", "state", "secret", clientRedirectUri)]
        [TestCase("", "state", "secret", clientRedirectUri)]
        public void TestGetRequestedScopes(string scope, string state, string secret, string redirectUri)
        {
            AuthorizationCodeRequest request = new AuthorizationCodeRequest
            (
                clientId: client.Id,
                redirectUri: new Uri(redirectUri),
                responseType: AuthorizationCodeResponseType.Code,
                scope: scope,
                state: state
            );

            var scopes = provider.GetRequestedScopes(request.Scope);

            if (scopes != null && scopes.Count() > 0)
            {
                Assert.True(scopes.All(s => s != null), "The scopes contained a null entry when it shouldn't have");
            }
            else
            {
                Assert.True(provider.ScopeParser(provider, scope).Any(s => s == null));
            }
        }

        [Test]
        [TestCase(exampleScopeId, "state", "secret", otherSecret, clientRedirectUri)]
        public void TestInvalidClientTokenRetrieval(string scope, string state, string secret, string badSecret, string redirectUri)
        {
            User user = new User
            {
                Id = "Id",
                Scopes = new Dictionary<IClient, IEnumerable<IScope>>
                {
                    { client, new IScope[] { exampleScope } }
                }
            };

            AuthorizationCodeRequest request = new AuthorizationCodeRequest
            (
                clientId: client.Id,
                redirectUri: new Uri(redirectUri),
                responseType: AuthorizationCodeResponseType.Code,
                scope: scope,
                state: state
            );

            ISuccessfulAuthorizationCodeResponse response = provider.RequestAuthorizationCode(request, user) as ISuccessfulAuthorizationCodeResponse;

            AuthorizationCodeGrantAccessTokenRequest tokenRequest = new AuthorizationCodeGrantAccessTokenRequest
            (
                authorizationCode: response.Code,
                clientId: badClient.Id,
                clientSecret: badSecret,
                redirectUri: new Uri(redirectUri)
            );

            Assert.That(provider.RequestAccessToken(tokenRequest) is IUnsuccessfulAccessTokenResponse);
        }

        [Test]
        [TestCase(clientId, exampleScopeId, "state", clientRedirectUri)]
        [TestCase(clientId, "nonExistantScope", "state", clientRedirectUri)]
        [TestCase(clientId, exampleScopeId, "state", clientRedirectUri)]
        [TestCase(clientId, exampleScopeId, "state", "http://example.com/oauth/v1/response/token")]
        public void TestRequestAuthorizationCode(string clientId, string scope, string state, string redirectUri)
        {
            User user = new User
            {
                Id = "Id"
            };

            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest
            (
                clientId: client.Id,
                redirectUri: new Uri(redirectUri),
                responseType: AuthorizationCodeResponseType.Code,
                scope: scope,
                state: state
            );

            IAuthorizationCodeResponse response = provider.RequestAuthorizationCode(codeRequest, user);

            Assert.NotNull(response);

            if ((ISuccessfulAuthorizationCodeResponse success = response as ISuccessfulAuthorizationCodeResponse) != null)
            {
                Assert.AreEqual(success.State, state);

                Assert.True(client.IsValidRedirectUri(new Uri(redirectUri)), "The client did not have a valid redirect Uri even though it was able to retrieve a code.");
                Assert.NotNull(provider.GetRequestedScopes(codeRequest.Scope), "The requested scope was invalid even though it got through.");
            }
            else
            {
                IUnsuccessfulAuthorizationCodeResponse unsuccess = (IUnsuccessfulAuthorizationCodeResponse)response;
                switch (unsuccess.SpecificErrorCode)
                {
                    case AuthorizationCodeRequestSpecificErrorType.MissingOrUnknownScope:
                        Assert.IsEmpty(provider.GetRequestedScopes(codeRequest.Scope));
                        break;
                    case AuthorizationCodeRequestSpecificErrorType.InvalidRedirectUri:
                        Assert.False(client.IsValidRedirectUri(codeRequest.RedirectUri));
                        break;
                    case AuthorizationCodeRequestSpecificErrorType.MissingClient:
                        Assert.True(string.IsNullOrEmpty(clientId));
                        break;

                }
            }
        }

        [Test]
        [TestCase(clientId, "secret", exampleScopeId, "state", clientRedirectUri)]
        public void TestRevokeAccess(string clientId, string clientSecret, string scope, string state, string redirectUri)
        {
            User user = new User
            {
                Id = "Id",
                Scopes = new Dictionary<IClient, IEnumerable<IScope>>
                {
                    { client, new IScope[] { exampleScope } }
                }
            };

            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest(clientId, scope, state, new Uri(redirectUri), AuthorizationCodeResponseType.Code);

            ISuccessfulAuthorizationCodeResponse response = provider.RequestAuthorizationCode(codeRequest, user) as ISuccessfulAuthorizationCodeResponse;

            Assert.NotNull(response);

            var tokenResponse = provider.RequestAccessToken(
                new AuthorizationCodeGrantAccessTokenRequest(response.Code, clientId, clientSecret, new Uri(redirectUri))
                ) as ISuccessfulAccessTokenResponse;

            Assert.NotNull(tokenResponse);

            Assert.True(provider.HasAccess(user, client, exampleScope));

            IAccessToken token = provider.AccessTokenRepository.GetByToken(tokenResponse.AccessToken);

            Assert.NotNull(token);

            Assert.True(!token.Revoked);
            Assert.True(!token.Expired);

            provider.RevokeAccess(user, client);

            Assert.True(token.Revoked);
            Assert.True(!token.Expired);
        }

        [Test]
        public void TestEndToEnd()
        {
            User user = new User
            {
                Id = "Id",
                Scopes = new Dictionary<IClient, IEnumerable<IScope>>
                {
                    { client, new IScope[] { exampleScope } }
                }
            };

            //A request from the client for an authorization code.
            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest
            (
                clientId: clientId,
                redirectUri: new Uri(clientRedirectUri),
                responseType: AuthorizationCodeResponseType.Code,
                scope: exampleScopeId,
                state: "state"
            );

            //Normally you would authorize the client and user here.
            //You would also determine if the user needs to provide consent using
            //provider.HasAccess(user, client, scope)

            //Then retrieve the code
            ISuccessfulAuthorizationCodeResponse response = provider.RequestAuthorizationCode(codeRequest, user) as ISuccessfulAuthorizationCodeResponse;

            Assert.NotNull(response);

            AuthorizationCodeGrantAccessTokenRequest tokenRequest = new AuthorizationCodeGrantAccessTokenRequest
            (
                clientId: clientId,
                clientSecret: "secret",
                redirectUri: new Uri(clientRedirectUri),
                authorizationCode: response.Code
            );

            ISuccessfulAccessTokenResponse tokenResponse = provider.RequestAccessToken(tokenRequest) as ISuccessfulAccessTokenResponse;

            Assert.NotNull(tokenResponse);

            IAccessToken token = provider.AccessTokenRepository.GetByToken(tokenResponse.AccessToken);

            Assert.NotNull(token);

            Assert.True(!token.Expired && !token.Revoked);

            Assert.True(provider.HasAccess(user, client, exampleScope), "The provider does not have access when it should");

            Client otherClient = new Client(otherSecret)
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

            TokenRefreshRequest refreshRequest = new TokenRefreshRequest
            (
                refreshToken: tokenResponse.RefreshToken,
                clientId: client.Id,
                clientSecret: "secret",
                grantType: "refresh_token",
                scope: tokenResponse.Scope
            );

            ISuccessfulAccessTokenResponse newTokenResponse = provider.RefreshAccessToken(refreshRequest) as ISuccessfulAccessTokenResponse;

            Assert.NotNull(newTokenResponse);

            token = provider.AccessTokenRepository.GetByToken(newTokenResponse.AccessToken);

            Assert.NotNull(token);

            token.Revoke();

            Assert.False(provider.HasAccess(user, client, exampleScope));
        }

    }
}
