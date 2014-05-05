﻿// Copyright 2014 Kallyn Gowdy
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

            badClient = new Client("otherSecret")
            {
                Id = "bill",
                Name = "Evil",
                RedirectUris = new Uri[]
                {
                    new Uri("http://example.com/oauth/response/token")
                }
            };

            ((ClientRepository)provider.ClientRepository).Add(client);
            ((ClientRepository)provider.ClientRepository).Add(badClient);
        }

        [Test]
        public void TestRefreshAccessToken()
        {
            User user = new User
            {
                Id = "Id"
            };

            ICreatedToken<IAccessToken> oldToken = provider.AccessTokenFactory.Create(client, user, new[] { exampleScope });
            provider.AccessTokenRepository.Add(oldToken.Token);

            ICreatedToken<IRefreshToken> refresh = provider.RefreshTokenFactory.Create(client, user, new[] { exampleScope });
            provider.RefreshTokenRepository.Add(refresh.Token);

            ITokenRefreshRequest refreshRequest = new TokenRefreshRequest
            (
                refresh.TokenValue,
                "bob",
                "secret",
                "refresh_token",
                "exampleScope"
            );

            var response = provider.RefreshAccessToken(refreshRequest);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.AccessToken, Is.Not.Null);
            Assert.That(response.RefreshToken, Is.Not.Null);
            Assert.That(response.Scope, Is.EqualTo("exampleScope"));
            Assert.That(response.TokenType, Is.EqualTo("Bearer"));
            Assert.That(oldToken.Token.IsValid(), Is.False);
            Assert.That(refresh.Token.IsValid(), Is.False);
        }

        [TestCase("exampleScope", "state", "secret", "http://example.com/oauth/response/token")]
        [TestCase("nonExistantScope", "state", "secret", "http://example.com/oauth/response/token")]
        [TestCase("otherScope, exampleScope", "state", "secret", "http://example.com/oauth/response/token")]
        [TestCase("", "state", "secret", "http://example.com/oauth/response/token")]
        public void TestGetRequestedScopes(string scope, string state, string secret, string redirectUri)
        {
            AuthorizationCodeRequest request = new AuthorizationCodeRequest
            (
                clientId: client.Id,
                clientSecret: secret,
                redirectUri: new Uri(redirectUri),
                responseType: AuthorizationCodeResponseType.Code,
                scope: scope,
                state: state
            );

            var scopes = provider.GetRequestedScopes(request);

            if (scopes != null && scopes.Count() > 0)
            {
                Assert.True(scopes.All(s => s != null), "The scopes contained a null entry when it shouldn't have");
            }
            else
            {
                Assert.True(provider.ScopeParser(provider, scope).Any(s => s == null));
            }
        }

        [TestCase("exampleScope", "state", "secret", "otherSecret", "http://example.com/oauth/response/token")]
        public void TestInvalidClientTokenRetrieval(string scope, string state, string secret, string badSecret, string redirectUri)
        {
            User user = new User
            {
                Id = "Id"
            };

            AuthorizationCodeRequest request = new AuthorizationCodeRequest
            (
                clientId: client.Id,
                clientSecret: secret,
                redirectUri: new Uri(redirectUri),
                responseType: AuthorizationCodeResponseType.Code,
                scope: scope,
                state: state
            );

            var response = provider.RequestAuthorizationCode(request, user);

            AuthorizationCodeGrantAccessTokenRequest tokenRequest = new AuthorizationCodeGrantAccessTokenRequest
            (
                authorizationCode: response.Code,
                clientId: badClient.Id,
                clientSecret: badSecret,
                redirectUri: new Uri(redirectUri)
            );

            Assert.Throws<AccessTokenResponseException>(() => provider.RequestAccessToken(tokenRequest));
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
            (
                clientId: client.Id,
                clientSecret: secret,
                redirectUri: new Uri(redirectUri),
                responseType: AuthorizationCodeResponseType.Code,
                scope: scope,
                state: state
            );

            try
            {
                var response = provider.RequestAuthorizationCode(codeRequest, user);

                Assert.NotNull(response);

                Assert.AreEqual(response.State, state);

                Assert.True(client.IsValidRedirectUri(new Uri(redirectUri)), "The client did not have a valid redirect Uri even though it was able to retrieve a code.");
                Assert.True(client.MatchesSecret(secret), "The client's secret was invalid even though it got through.");
                Assert.NotNull(provider.GetRequestedScopes(codeRequest), "The requested scope was invalid even though it got through.");
            }
            catch (AuthorizationCodeResponseExceptionBase e)
            {
                switch (e.ErrorCode)
                {
                    case AuthorizationRequestCodeErrorType.UnauthorizedClient:
                        Assert.False(client.MatchesSecret(secret));
                        break;
                    case AuthorizationRequestCodeErrorType.InvalidScope:
                        Assert.IsEmpty(provider.GetRequestedScopes(codeRequest));
                        break;
                    case AuthorizationRequestCodeErrorType.InvalidRequest:
                        Assert.False(client.IsValidRedirectUri(codeRequest.RedirectUri));
                        break;
                }
            }
        }

        [TestCase("bob", "secret", "exampleScope", "state", "http://example.com/oauth/response/token")]
        public void TestRevokeAccess(string clientId, string clientSecret, string scope, string state, string redirectUri)
        {
            User user = new User
            {
                Id = "Id"
            };

            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest(clientId, clientSecret, scope, state, new Uri(redirectUri), AuthorizationCodeResponseType.Code);

            var response = provider.RequestAuthorizationCode(codeRequest, user);

            var tokenResponse = provider.RequestAccessToken(new AuthorizationCodeGrantAccessTokenRequest(response.Code, clientId, clientSecret, new Uri(redirectUri)));

            Assert.True(provider.HasAccess(user, client, exampleScope));

            IAccessToken token = provider.AccessTokenRepository.GetByToken(tokenResponse.AccessToken);

            Assert.NotNull(token);

            Assert.True(!token.Revoked);
            Assert.True(!token.Expired);

            token.Revoke();

            Assert.True(token.Revoked);
            Assert.True(!token.Expired);
        }

        [Test]
        public void TestEndToEnd()
        {
            User user = new User
            {
                Id = "Id"
            };

            //A request from the client for an authorization code.
            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest
            (
                clientId: "bob",
                clientSecret: "secret",
                redirectUri: new Uri("http://example.com/oauth/response/token"),
                responseType: AuthorizationCodeResponseType.Code,
                scope: "exampleScope",
                state: "state"
            );

            //Normally you would authorize the client and user here.
            //You would also determine if the user needs to provide consent using
            //provider.HasAccess(user, client, scope)

            //Then retrieve the code
            IAuthorizationCodeResponse response = provider.RequestAuthorizationCode(codeRequest, user);

            Assert.NotNull(response);

            AuthorizationCodeGrantAccessTokenRequest tokenRequest = new AuthorizationCodeGrantAccessTokenRequest
            (
                clientId: "bob",
                clientSecret: "secret",
                redirectUri: new Uri("http://example.com/oauth/response/token"),
                authorizationCode: response.Code
            );

            var tokenResponse = provider.RequestAccessToken(tokenRequest);

            Assert.NotNull(tokenResponse);

            IAccessToken token = provider.AccessTokenRepository.GetByToken(tokenResponse.AccessToken);

            Assert.NotNull(token);

            Assert.True(!token.Expired && !token.Revoked);

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

            TokenRefreshRequest refreshRequest = new TokenRefreshRequest
            (
                refreshToken: tokenResponse.RefreshToken,
                clientId: client.Id,
                clientSecret: "secret",
                grantType: "refresh_token",
                scope: tokenResponse.Scope
            );

            var newTokenResponse = provider.RefreshAccessToken(refreshRequest);

            Assert.NotNull(newTokenResponse);


            token = provider.AccessTokenRepository.GetByToken(newTokenResponse.AccessToken);

            Assert.NotNull(token);

            token.Revoke();

            Assert.False(provider.HasAccess(user, client, exampleScope));
        }

    }
}
