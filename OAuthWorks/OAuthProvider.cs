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



using OAuthWorks.DataAccess.Repositories;
using OAuthWorks.Factories;
using OAuthWorks.Implementation.Factories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using OAuthWorks.Implementation;

namespace OAuthWorks
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Auth")]
    public class OAuthProvider : IOAuthProvider
    {
        /// <summary>
        /// Defines a class that contains tests for <see cref="OAuthProvider"/>.
        /// </summary>
        [TestFixture]
        public class Tests
        {
            OAuthProvider provider;

            IScope exampleScope;

            IScope otherScope;

            IClient client;

            IClient badClient;

            IUser user;

            [TestFixtureSetUp]
            public void SetUp()
            {
                var mockedScope = new Mock<IScope>();
                mockedScope.Setup(s => s.Id).Returns("exampleScope");

                exampleScope = mockedScope.Object;

                mockedScope = new Mock<IScope>();
                mockedScope.Setup(s => s.Id).Returns("otherScope");

                otherScope = mockedScope.Object;

                var scopeRepository = new Mock<IScopeRepository<IScope>>();
                scopeRepository.Setup(r => r.GetById("exampleScope")).Returns(exampleScope);
                scopeRepository.Setup(r => r.GetById("otherScope")).Returns(otherScope);

                var mockedClient = new Mock<IClient>();
                mockedClient.Setup(c => c.Name).Returns("bob");
                mockedClient.Setup(c => c.RedirectUris).Returns(new Uri[]
                {
                    new Uri("http://example.com/oauth/response/token")
                });
                mockedClient.Setup(c => c.IsValidRedirectUri(It.IsAny<Uri>())).Returns<Uri>(u => mockedClient.Object.RedirectUris.Contains(u));

                client = mockedClient.Object;

                var mockedBadClient = new Mock<IClient>();
                mockedClient.Setup(c => c.Name).Returns("Evil");
                mockedClient.Setup(c => c.RedirectUris).Returns(new Uri[]
                {
                    new Uri("http://example.com/oauth/response/token")
                });
                mockedClient.Setup(c => c.IsValidRedirectUri(It.IsAny<Uri>())).Returns<Uri>(u => mockedClient.Object.RedirectUris.Contains(u));
                mockedClient.Setup(c => c.MatchesSecret(It.IsAny<string>())).Returns(true);

                badClient = mockedBadClient.Object;

                var clientRepository = new Mock<IReadStore<string, IClient>>();
                Dictionary<string, IClient> cDict = new Dictionary<string, IClient>()
                {
                    { "bob", client },
                    { "Evil", badClient }
                };
                clientRepository.Setup(r => r.GetById(It.IsAny<string>())).Returns<string>(s => cDict[s]);

                Mock<IUser> mockedUser = new Mock<IUser>();
                mockedUser.Setup(u => u.Id).Returns("User");
                user = mockedUser.Object;

                Dictionary<string, IAccessToken> dict = new Dictionary<string, IAccessToken>();
                Mock<IAccessTokenRepository> accessTokenRepository = new Mock<IAccessTokenRepository>();
                accessTokenRepository.Setup(r => r.Add(It.IsAny<ICreatedToken<IAccessToken>>())).Callback<ICreatedToken<IAccessToken>>(t => dict.Add(((IHasId<string>)t.Token).Id, t.Token));
                accessTokenRepository.Setup(r => r.GetByToken(It.IsAny<string>())).Returns<string>(s => dict[s]);

                provider = new OAuthProvider
                {
                    ClientRepository = clientRepository.Object,
                    ScopeRepository = scopeRepository.Object,
                    AccessTokenRepository = accessTokenRepository.Object
                };
                provider.ScopeParser = (p, s) => s.Split(' ', '-').Select(a => p.ScopeRepository.GetById(a));
                provider.ScopeFormatter = (s) => string.Join(" ", s.Select(sc => sc.Id));
                provider.DistributeRefreshTokens = true;
                provider.DeleteRevokedTokens = true;
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
            [TestCase(null, "secret", "code", "access_token", "http://example.com/oauth/response/token", AccessTokenRequestError.InvalidRequest)]
            [TestCase("", "secret", "code", "access_token", "http://example.com/oauth/response/token", AccessTokenRequestError.InvalidRequest)]
            [TestCase("bob", null, "code", "access_token", "http://example.com/oauth/response/token", AccessTokenRequestError.InvalidRequest)]
            [TestCase("bob", "", "code", "access_token", "http://example.com/oauth/response/token", AccessTokenRequestError.InvalidRequest)]
            [TestCase("bob", "secret", null, "access_token", "http://example.com/oauth/response/token", AccessTokenRequestError.InvalidRequest)]
            [TestCase("bob", "secret", "", "access_token", "http://example.com/oauth/response/token", AccessTokenRequestError.InvalidRequest)]
            [TestCase("bob", "secret", "code", "access_token", null, AccessTokenRequestError.InvalidRequest)]
            [TestCase("bob", "secret", "code", null, "http://example.com/oauth/response/token", AccessTokenRequestError.InvalidRequest)]
            [TestCase("bob", "secret", "code", "", "http://example.com/oauth/response/token", AccessTokenRequestError.InvalidRequest)]
            [TestCase("bob", "secret", "code", "not_access_token", "http://example.com/oauth/response/token", AccessTokenRequestError.InvalidRequest)]
            public void TestInvalidAccessTokenRequest(string clientId, string clientSecret, string authorizationCode, string grantType, string redirectUri, AccessTokenRequestError expectedError)
            {
                var request = new InvalidAccessTokenRequest { ClientId = clientId, ClientSecret = clientSecret, AuthorizationCode = authorizationCode, GrantType = grantType, RedirectUri = (redirectUri != null ? new Uri(redirectUri) : null) };

                IUnsuccessfulAccessTokenResponse response = provider.RequestAccessToken(request) as IUnsuccessfulAccessTokenResponse;

                Assert.NotNull(response);
                Assert.AreEqual(expectedError, response.Error);
            }

            [Test]
            public void TestRefreshAccessToken()
            {

                ICreatedToken<IAccessToken> oldToken = provider.AccessTokenFactory.Create(user, client, new[] { exampleScope });
                provider.AccessTokenRepository.Add(oldToken);

                ICreatedToken<IRefreshToken> refresh = provider.RefreshTokenFactory.Create(user, client, new[] { exampleScope });
                provider.RefreshTokenRepository.Add(refresh);

                ITokenRefreshRequest refreshRequest = new TokenRefreshRequest
                (
                    refresh.TokenValue,
                    "bob",
                    "secret",
                    "refresh_token",
                    "exampleScope"
                );

                ISuccessfulAccessTokenResponse response = provider.RefreshAccessToken(refreshRequest) as ISuccessfulAccessTokenResponse;

                Assert.That(response, Is.Not.Null);
                Assert.That(response.AccessToken, Is.Not.Null);
                Assert.That(response.RefreshToken, Is.Not.Null);
                Assert.That(response.Scope, Is.EqualTo("exampleScope"));
                Assert.That(response.TokenType, Is.EqualTo("Bearer"));
                Assert.That(oldToken.Token.IsValid(), Is.False);
                Assert.That(refresh.Token.IsValid(), Is.False);
            }

            [Test]
            [TestCase("exampleScope", "state", "secret", "http://example.com/oauth/response/token")]
            [TestCase("nonExistantScope", "state", "secret", "http://example.com/oauth/response/token")]
            [TestCase("otherScope, exampleScope", "state", "secret", "http://example.com/oauth/response/token")]
            [TestCase("", "state", "secret", "http://example.com/oauth/response/token")]
            public void TestGetRequestedScopes(string scope, string state, string secret, string redirectUri)
            {
                AuthorizationCodeRequest request = new AuthorizationCodeRequest
                (
                    clientId: client.Name,
                    clientSecret: secret,
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
            [TestCase("exampleScope", "state", "secret", "otherSecret", "http://example.com/oauth/response/token")]
            public void TestInvalidClientTokenRetrieval(string scope, string state, string secret, string badSecret, string redirectUri)
            {
                AuthorizationCodeRequest request = new AuthorizationCodeRequest
                (
                    clientId: client.Name,
                    clientSecret: secret,
                    redirectUri: new Uri(redirectUri),
                    responseType: AuthorizationCodeResponseType.Code,
                    scope: scope,
                    state: state
                );

                ISuccessfulAuthorizationCodeResponse response = provider.RequestAuthorizationCode(request, user) as ISuccessfulAuthorizationCodeResponse;

                AuthorizationCodeGrantAccessTokenRequest tokenRequest = new AuthorizationCodeGrantAccessTokenRequest
                (
                    authorizationCode: response.Code,
                    clientId: badClient.Name,
                    clientSecret: badSecret,
                    redirectUri: new Uri(redirectUri)
                );

                Assert.That(provider.RequestAccessToken(tokenRequest) is IUnsuccessfulAccessTokenResponse);
            }

            [Test]
            [TestCase("exampleScope", "state", "secret", "http://example.com/oauth/response/token")]
            [TestCase("nonExistantScope", "state", "secret", "http://example.com/oauth/response/token")]
            [TestCase("exampleScope", "state", "badSecret", "http://example.com/oauth/response/token")]
            [TestCase("exampleScope", "state", "secret", "http://example.com/oauth/v1/response/token")]
            public void TestRequestAuthorizationCode(string scope, string state, string secret, string redirectUri)
            {
                AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest
                (
                    clientId: client.Name,
                    clientSecret: secret,
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
                    Assert.True(client.MatchesSecret(secret), "The client's secret was invalid even though it got through.");
                    Assert.NotNull(provider.GetRequestedScopes(codeRequest.Scope), "The requested scope was invalid even though it got through.");
                }
                else
                {
                    IUnsuccessfulAuthorizationCodeResponse unsuccess = (IUnsuccessfulAuthorizationCodeResponse)response;
                    switch (unsuccess.ErrorCode)
                    {
                        case AuthorizationCodeRequestErrorType.UnauthorizedClient:
                            Assert.False(client.MatchesSecret(secret));
                            break;
                        case AuthorizationCodeRequestErrorType.InvalidScope:
                            Assert.IsEmpty(provider.GetRequestedScopes(codeRequest.Scope));
                            break;
                        case AuthorizationCodeRequestErrorType.InvalidRequest:
                            Assert.False(client.IsValidRedirectUri(codeRequest.RedirectUri));
                            break;
                    }
                }
            }

            [Test]
            [TestCase("bob", "secret", "exampleScope", "state", "http://example.com/oauth/response/token")]
            public void TestRevokeAccess(string clientId, string clientSecret, string scope, string state, string redirectUri)
            {
                AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest(clientId, clientSecret, scope, state, new Uri(redirectUri), AuthorizationCodeResponseType.Code);

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

                token.Revoke();

                Assert.True(token.Revoked);
                Assert.True(!token.Expired);
            }

            [Test]
            public void TestEndToEnd()
            {
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
                ISuccessfulAuthorizationCodeResponse response = provider.RequestAuthorizationCode(codeRequest, user) as ISuccessfulAuthorizationCodeResponse;

                Assert.NotNull(response);

                AuthorizationCodeGrantAccessTokenRequest tokenRequest = new AuthorizationCodeGrantAccessTokenRequest
                (
                    clientId: "bob",
                    clientSecret: "secret",
                    redirectUri: new Uri("http://example.com/oauth/response/token"),
                    authorizationCode: response.Code
                );

                ISuccessfulAccessTokenResponse tokenResponse = provider.RequestAccessToken(tokenRequest) as ISuccessfulAccessTokenResponse;

                Assert.NotNull(tokenResponse);

                IAccessToken token = provider.AccessTokenRepository.GetByToken(tokenResponse.AccessToken);

                Assert.NotNull(token);

                Assert.True(!token.Expired && !token.Revoked);

                Assert.True(provider.HasAccess(user, client, exampleScope), "The provider does not have access when it should");

                Assert.False(provider.HasAccess(user, badClient, exampleScope), "The unathorized client had access when it shouldn't have.");

                TokenRefreshRequest refreshRequest = new TokenRefreshRequest
                (
                    refreshToken: tokenResponse.RefreshToken,
                    clientId: badClient.Name,
                    clientSecret: "secret",
                    grantType: "refresh_token",
                    scope: tokenResponse.Scope
                );

                ISuccessfulAccessTokenResponse newTokenResponse = provider.RefreshAccessToken(refreshRequest) as ISuccessfulAccessTokenResponse;

                Assert.NotNull(newTokenResponse);

                token = provider.AccessTokenRepository.GetByToken(newTokenResponse.AccessToken);

                Assert.NotNull(token);

                token.Revoke();

                Assert.False(provider.HasAccess(user, badClient, exampleScope));
            }

        }

        /// <summary>
        /// The default <see cref="IAccessTokenFactory{TAccessToken}"/> constructor.
        /// </summary>
        public static readonly Func<IAccessTokenFactory<IAccessToken>> DefaultAccessTokenFactoryConstructor = () => Implementation.Factories.AccessTokenFactory.String.DefaultFactory;

        /// <summary>
        /// The default <see cref="IAuthorizationCodeFactory{TAuthorizationCode}"/> constructor.
        /// </summary>
        public static readonly Func<IAuthorizationCodeFactory<IAuthorizationCode>> DefaultAuthorizationCodeFactoryConstructor = () => Implementation.Factories.AuthorizationCodeFactory.String.DefaultFactory;

        /// <summary>
        /// The default <see cref="IAccessTokenResponseFactory"/> constructor.
        /// </summary>
        public static readonly Func<IAccessTokenResponseFactory> DefaultAccessTokenResponseFactoryConstructor = () => new AccessTokenResponseFactory();

        /// <summary>
        /// The default <see cref="IAuthorizationCodeResponseFactory"/> constructor.
        /// </summary>
        public static readonly Func<IAuthorizationCodeResponseFactory> DefaultAuthorizationCodeReponseFactoryConstructor = () => new AuthorizationCodeResponseFactory();

        /// <summary>
        /// The default <see cref="IRefreshTokenFactory{TRefreshToken}"/> constructor.
        /// </summary>
        public static readonly Func<IRefreshTokenFactory<IRefreshToken>> DefaultRefreshTokenFactoryConstructor = () => Implementation.Factories.RefreshTokenFactory.String.DefaultFactory;

        /// <summary>
        /// Creates a new <see cref="OAuthProvider"/> using a default factories.
        /// </summary>
        public OAuthProvider() :
            this(
                DefaultAccessTokenFactoryConstructor(),
                DefaultAccessTokenResponseFactoryConstructor(),
                DefaultAuthorizationCodeFactoryConstructor(),
                DefaultAuthorizationCodeReponseFactoryConstructor(),
                DefaultRefreshTokenFactoryConstructor()
            )
        {
        }

        /// <summary>
        /// Creates a new <see cref="OAuthProvider"/> using the default factories and given <see cref="Action{OAuthProvider}"/> for initialization.
        /// </summary>
        /// <param name="initialization">A <see cref="Action{OAuthProvider}"/> that completes initializtion of the provider.</param>
        public OAuthProvider(Action<OAuthProvider> initialization) :
            this()
        {
            if (initialization == null)
            {
                throw new ArgumentNullException("initialization");
            }
            initialization(this);
        }

        public OAuthProvider(
            IAccessTokenFactory<IAccessToken> accessTokenFactory,
            IAccessTokenResponseFactory accessTokenResponseFactory,
            IAuthorizationCodeFactory<IAuthorizationCode> authorizationCodeFactory,
            IAuthorizationCodeResponseFactory authorizationCodeResponseFactory,
            IRefreshTokenFactory<IRefreshToken> refreshTokenFactory
            )
        {
            Contract.Requires(accessTokenFactory != null);
            Contract.Requires(accessTokenResponseFactory != null);
            Contract.Requires(authorizationCodeFactory != null);
            Contract.Requires(authorizationCodeResponseFactory != null);
            Contract.Requires(refreshTokenFactory != null);
            AccessTokenFactory = accessTokenFactory;
            AuthorizationCodeFactory = authorizationCodeFactory;
            AccessTokenResponseFactory = accessTokenResponseFactory;
            AuthorizationCodeResponseFactory = authorizationCodeResponseFactory;
            RefreshTokenFactory = refreshTokenFactory;
        }

        public OAuthProvider(
            IAccessTokenRepository accessTokenRepository,
            IAuthorizationCodeRepository authorizationCodeRepository,
            IScopeRepository<IScope> scopeRepository,
            IReadStore<string, IClient> clientRepository,
            IRefreshTokenRepository refreshTokenRepository
            )
            : this()
        {
            Contract.Requires(accessTokenRepository != null);
            Contract.Requires(authorizationCodeRepository != null);
            Contract.Requires(scopeRepository != null);
            Contract.Requires(clientRepository != null);
            AccessTokenRepository = accessTokenRepository;
            AuthorizationCodeRepository = authorizationCodeRepository;
            ScopeRepository = scopeRepository;
            ClientRepository = clientRepository;
            RefreshTokenRepository = refreshTokenRepository;
        }

        /// <summary>
        /// Gets or sets the repository of scopes that this provider has access to.
        /// </summary>
        /// <remarks>
        /// This property tries to retrieve it's value from the dependency injector when it is null.
        /// If accessing this value results in null being returned, then the provider could not retrieve a default value from the dependency injector.
        /// </remarks>
        public IScopeRepository<IScope> ScopeRepository
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the repository that contains <see cref="OAuthWorks.IClient"/> objects.
        /// </summary>
        public IReadStore<string, IClient> ClientRepository
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the factory used to create new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> objects.
        /// </summary>
        public IAuthorizationCodeResponseFactory AuthorizationCodeResponseFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the repository of Authorization Code objects that this provider has access to.
        /// </summary>
        public IAuthorizationCodeRepository AuthorizationCodeRepository
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the factory that creates Authorization Code objects for this provider.
        /// </summary>
        public IAuthorizationCodeFactory<IAuthorizationCode> AuthorizationCodeFactory
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the repository of Access Token objects that this provider has access to.
        /// </summary>
        public IAccessTokenRepository AccessTokenRepository
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the factory that creates Access Token objects for this provider.
        /// </summary>
        public IAccessTokenFactory<IAccessToken> AccessTokenFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the factory that creates <see cref="OAuthWorks.ISuccessfulAccessTokenResponse"/> objects for this provider.
        /// </summary>
        public IAccessTokenResponseFactory AccessTokenResponseFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the factory that creates <see cref="OAuthWorks.IRefreshToken"/> objects for this provider.
        /// </summary>
        public IRefreshTokenFactory<IRefreshToken> RefreshTokenFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the repository that is used to store OAuthWorks.IRefreshToken objects.
        /// </summary>
        public IRefreshTokenRepository RefreshTokenRepository
        {
            get;
            set;
        }

        private Func<IEnumerable<IScope>, string> scopeFormatter = s => string.Join(" ", s);

        /// <summary>
        /// Gets or sets the formatter for scopes. That is, a function that, given a list of scopes, returns a string representing those scopes.
        /// </summary>
        public Func<IEnumerable<IScope>, string> ScopeFormatter
        {
            get
            {
                return scopeFormatter;
            }
            set
            {
                Contract.Requires(value != null);
                scopeFormatter = value;
            }
        }

        private Func<OAuthProvider, string, IEnumerable<IScope>> scopeParser = (p, s) => s.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => p.ScopeRepository.GetById(c));

        /// <summary>
        /// Gets or sets the parser for the scopes. That is, a function that, given a string, returns a list of scopes representing that string.
        /// If any of the returned scopes are null, then the requested scope is considered invalid and will be treated as such. The parser should always return a non-null value.
        /// </summary>
        public Func<OAuthProvider, string, IEnumerable<IScope>> ScopeParser
        {
            get
            {
                return scopeParser;
            }
            set
            {
                Contract.Requires(value != null);
                scopeParser = value;
            }
        }

        private Func<AccessTokenRequestError, IClient, string> accessTokenErrorDescriptionProvider = (e, c) => "";

        private Func<AccessTokenRequestError, IClient, Uri> accessTokenErrorUriProvider = (e, c) => null;

        /// <summary>
        /// Gets or sets the access token error description provider. That is, a function that, given the error and client, returns a string describing the problem.
        /// </summary>
        public Func<AccessTokenRequestError, IClient, string> AccessTokenErrorDescriptionProvider
        {
            get
            {
                return accessTokenErrorDescriptionProvider;
            }
            set
            {
                value.ThrowIfNull("value");
                accessTokenErrorDescriptionProvider = value;
            }
        }

        private Func<AuthorizationCodeRequestErrorType, string> authorizationCodeErrorDescriptionProvider = e => null;

        /// <summary>
        /// Gets or sets the error description provider. That is, a function that, given the error, returns a string describing the problem.
        /// </summary>
        /// <returns></returns>
        public Func<AuthorizationCodeRequestErrorType, string> AuthorizationCodeErrorDescriptionProvider
        {
            get
            {
                return authorizationCodeErrorDescriptionProvider;
            }
            set
            {
                value.ThrowIfNull("value");
                authorizationCodeErrorDescriptionProvider = value;
            }
        }

        /// <summary>
        /// Gets or sets the error uri provider. That is, a function that, given the error and client, returns a string containing a uri that points to a web page providing information
        /// on the error.
        /// </summary>
        public Func<AccessTokenRequestError, IClient, Uri> AccessTokenErrorUriProvider
        {
            get
            {
                return accessTokenErrorUriProvider;
            }
            set
            {
                value.ThrowIfNull("value");
                accessTokenErrorUriProvider = value;
            }
        }

        /// <summary>
        /// Gets a list of the scopes that are being requested by the given OAuthWorks.IAuthorizationCodeRequest object.
        /// </summary>
        /// <param name="request">The request that is being used to request an authorization code.</param>
        /// <returns>Returns an enumerable list of scopes that define the permissions requested. Null if one of the requested scopes are invalid.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the given request object is null.</exception>
        public IEnumerable<IScope> GetRequestedScopes(string scopes)
        {
            IEnumerable<IScope> result = ScopeParser(this, scopes != null ? scopes : string.Empty);
            return result.All(s => s != null) ? result : new IScope[0];
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")] // Suppressed to be able to return IAuthorizationCodeResponse objects according to OAuth 2.0
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")] // Already validated by IsValidRequest
        /// <summary>
        /// Initiates the Authorization Code flow based on the given request and returns a response that defines what response to send back to the user agent.
        /// Be sure to authenticate the user and request consent before calling this. THIS METHOD ASSUMES THAT USER CONSENT WAS GIVEN.
        /// </summary>
        /// <param name="request">The request that contains the values that were sent by the client.</param>
        /// <param name="user">The user that the request is for.</param>
        /// <exception cref="OAuthWorks.AuthorizationCodeResponseExceptionBase">Thrown if an exception occurs inside this method or if the given request was invalid in some way.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the given request is null.</exception>
        /// <returns>Returns a new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> object that determines what values to put in the outgoing response.</returns>
        public IAuthorizationCodeResponse RequestAuthorizationCode(IAuthorizationCodeRequest request, IUser user)
        {
            if (IsValidRequest(request))
            {
                try
                {
                    IClient client = ClientRepository.GetById(request.ClientId);
                    if (IsValidClient(request, client))
                    {
                        if (client.IsValidRedirectUri(request.RedirectUri))
                        {
                            IEnumerable<IScope> scopes = GetRequestedScopes(request.Scope);

                            if (scopes != null && scopes.Any())
                            {
                                //Revoke all of the current authorization codes.
                                AuthorizationCodeRepository.GetByUserAndClient(user, client).ForEach(c => c.Revoke());

                                ICreatedToken<IAuthorizationCode> authCode = AuthorizationCodeFactory.Create(request.RedirectUri, user, client, scopes);
                                AuthorizationCodeRepository.Add(authCode);

                                //return a successful response
                                return AuthorizationCodeResponseFactory.Create(authCode.TokenValue, request.State, request.RedirectUri);
                            }
                            else
                            {
                                return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.InvalidScope, request.State, request.RedirectUri);
                            }
                        }
                        else
                        {
                            //Invalid redirect
                            return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.InvalidRequest, request.State, null);
                        }
                    }
                    else
                    {
                        return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.UnauthorizedClient, request.State, request.RedirectUri);
                    }
                }
                catch (SystemException e)
                {
                    return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.ServerError, request.State, request.RedirectUri, e);
                }
            }
            else
            {
                return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.InvalidRequest, null, null);
            }
        }

        /// <summary>
        /// Determines if the given <see cref="IAuthorizationCodeRequest"/> is valid for the given <see cref="IClient"/>.
        /// </summary>
        /// <param name="request">The request that should examined to see if the proper client credientials were given.</param>
        /// <param name="client">The client that the request should be validated against.</param>
        /// <returns>Returns whether the given request contains valid client credentials.</returns>
        private static bool IsValidClient(IAuthorizationCodeRequest request, IClient client)
        {
            return client != null && client.MatchesSecret(request.ClientSecret);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")] // Suppressed to be able to return IAccessTokenResponse objects according to OAuth 2.0
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")] // Already validated by IsValidRequest
        /// <summary>
        /// Requests an access refreshToken from the server with the request.
        /// </summary>
        /// <param name="request">The incoming request for an access refreshToken.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object that represents what to return to the client.</returns>
        public IAccessTokenResponse RequestAccessToken(IAuthorizationCodeGrantAccessTokenRequest request)
        {
            if (IsValidRequest(request))
            {
                try
                {
                    IClient client = ClientRepository.GetById(request.ClientId);
                    if (!IsValidClient(request, client))
                    {
                        return CreateAccessTokenError(AccessTokenRequestError.InvalidClient, client);
                    }

                    IAuthorizationCode code = AuthorizationCodeRepository.GetByValue(request.AuthorizationCode);
                    if (!IsValidAuthorizationCode(code, request, client))
                    {
                        return CreateAccessTokenError(AccessTokenRequestError.InvalidGrant, client);
                    }

                    //Authorized!

                    code.Revoke(); // Prevent this same code from being used again

                    ICreatedToken<IAccessToken> accessToken = AccessTokenFactory.Create(code.User, client, code.Scopes);
                    ICreatedToken<IRefreshToken> refreshToken = null;
                    if (RefreshTokenFactory != null && DistributeRefreshTokens)
                    {
                        refreshToken = RefreshTokenFactory.Create(code.User, client, code.Scopes);
                        RefreshTokenRepository.Add(refreshToken);
                    }

                    //store refresh and access tokens
                    AccessTokenRepository.Add(accessToken);

                    return AccessTokenResponseFactory.Create(
                        accessToken.TokenValue,
                        refreshToken != null ? refreshToken.TokenValue : null,
                        accessToken.Token.TokenType,
                        ScopeFormatter(accessToken.Token.Scopes),
                        accessToken.Token.ExpirationDateUtc);
                }
                catch (SystemException e)
                {
                    // Server error
                    return CreateAccessTokenError(AccessTokenRequestError.ServerError, null, e);
                }
            }
            else
            {
                return CreateAccessTokenError(AccessTokenRequestError.InvalidRequest, null, null);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")] // Suppressed to be able to return IAccessTokenResponse objects according to OAuth 2.0
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")] // Already validated by IsValidRequest
        /// <summary>
        /// Requests a new access refreshToken from the authorizaiton server based on the given request.
        /// </summary>
        /// <param name="request">The request that contains the required values for refreshing an access refreshToken.</param>
        /// <returns>
        /// Returns a new <see cref="OAuthWorks.IAccessTokenResponse" /> object that determines what values to put in the outgoing response.
        /// </returns>
        public IAccessTokenResponse RefreshAccessToken(ITokenRefreshRequest request)
        {
            if (IsValidRequest(request))
            {
                try
                {
                    IClient client = ClientRepository.GetById(request.ClientId);
                    if (!IsValidClient(request, client))
                    {
                        return CreateAccessTokenError(AccessTokenRequestError.InvalidClient, client);
                    }

                    IRefreshToken refreshToken = RefreshTokenRepository.GetByValue(request.RefreshToken);
                    if (!IsValidRefreshToken(refreshToken, request, client))
                    {
                        return CreateAccessTokenError(AccessTokenRequestError.InvalidClient, client);
                    }

                    foreach (IAccessToken oldToken in AccessTokenRepository.GetByUserAndClient(refreshToken.User, client).ToArray())
                    {
                        if (oldToken != null)
                        {
                            if (!oldToken.Revoked)
                            {
                                oldToken.Revoke();
                            }

                            if (DeleteRevokedTokens)
                            {
                                AccessTokenRepository.Remove(oldToken);
                            }
                        }
                    }

                    string refreshValue = request.RefreshToken;

                    ICreatedToken<IAccessToken> newToken = AccessTokenFactory.Create(refreshToken.User, client, refreshToken.Scopes);

                    AccessTokenRepository.Add(newToken);

                    if (!ReuseRefreshTokens)
                    {
                        ICreatedToken<IRefreshToken> newRefresh = RefreshTokenFactory.Create(refreshToken.User, client, refreshToken.Scopes);
                        refreshToken.Revoke();
                        if (DeleteRevokedTokens)
                        {
                            RefreshTokenRepository.Remove(refreshToken);
                        }
                        RefreshTokenRepository.Add(newRefresh);
                        refreshValue = newRefresh.TokenValue;
                    }

                    return AccessTokenResponseFactory.Create(newToken.TokenValue, refreshValue, newToken.Token.TokenType, ScopeFormatter(newToken.Token.Scopes), newToken.Token.ExpirationDateUtc);
                }
                catch (SystemException e)
                {
                    return CreateAccessTokenError(AccessTokenRequestError.ServerError, null, e);
                }
            }
            else
            {
                return CreateAccessTokenError(AccessTokenRequestError.InvalidRequest);
            }
        }

        /// <summary>
        /// Requests an access refreshToken from the authorization server based on the given request using the Resource Owner Password Credentials flow. (Section 4.3 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.3).
        /// </summary>
        /// <param name="request">The request that contains the required values.</param>
        /// <returns>
        /// Returns a new <see cref="OAuthWorks.ISuccessfulAccessTokenResponse" /> object that determines what values to put in the outgoing response.
        /// </returns>
        public virtual IAccessTokenResponse RequestAccessToken(IPasswordCredentialsAccessTokenRequest request)
        {
            if (IsValidRequest(request))
            {
                try
                {
                    IClient client = ClientRepository.GetById(request.ClientId);
                    if (!IsValidClient(request, client))
                    {
                        return CreateAccessTokenError(AccessTokenRequestError.InvalidClient, client);
                    }

                    IEnumerable<IScope> scopes = GetRequestedScopes(request.Scope);
                    if (scopes != null && scopes.Any())
                    {

                        AccessTokenRepository.GetByUserAndClient(request.User, client).ToArray().ForEach(t =>
                        {
                            t.Revoke();
                            if (DeleteRevokedTokens)
                                AccessTokenRepository.Remove(t);
                        });

                        RefreshTokenRepository.GetByUserAndClient(request.User, client).ForEach(t =>
                        {
                            t.Revoke();
                            if (DeleteRevokedTokens)
                                RefreshTokenRepository.Remove(t);
                        });

                        ICreatedToken<IAccessToken> token = AccessTokenFactory.Create(request.User, client, scopes);
                        ICreatedToken<IRefreshToken> refreshToken = null;
                        if (RefreshTokenFactory != null && DistributeRefreshTokens)
                        {
                            refreshToken = RefreshTokenFactory.Create(request.User, client, scopes);
                            RefreshTokenRepository.Add(refreshToken);
                        }

                        AccessTokenRepository.Add(token);

                        return AccessTokenResponseFactory.Create(token.TokenValue,
                            refreshToken != null ? refreshToken.TokenValue : null,
                            token.Token.TokenType,
                            ScopeFormatter(scopes),
                            token.Token.ExpirationDateUtc);
                    }
                    else
                    {
                        return CreateAccessTokenError(AccessTokenRequestError.InvalidScope, client);
                    }
                }
                catch (SystemException e)
                {
                    return CreateAccessTokenError(AccessTokenRequestError.ServerError, null, e);
                }
            }
            else
            {
                return CreateAccessTokenError(AccessTokenRequestError.InvalidRequest);
            }
        }

        /// <summary>
        /// Determines if the given request contains acceptable values for processing.
        /// </summary>
        /// <param name="request">The request the examine for faults.</param>
        /// <returns>Returns true if the request is valid and therefore relatively safe for use in database transactions.</returns>
        private static bool IsValidRequest(IAuthorizationCodeRequest request)
        {
            return request != null &&
                !string.IsNullOrEmpty(request.ClientId) &&
                !string.IsNullOrEmpty(request.ClientSecret) &&
                !string.IsNullOrEmpty(request.Scope) &&
                request.RedirectUri != null;
        }

        /// <summary>
        /// Determines if the given request contains acceptable values for processing.
        /// </summary>
        /// <param name="request">The request the examine for faults.</param>
        /// <returns>Returns true if the request is valid and therefore relatively safe for use in database transactions.</returns>
        private static bool IsValidRequest(ITokenRefreshRequest request)
        {
            return request != null &&
                !string.IsNullOrEmpty(request.ClientId) &&
                !string.IsNullOrEmpty(request.ClientSecret) &&
                !string.IsNullOrEmpty(request.RefreshToken) &&
                "refresh_token".Equals(request.GrantType, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines if the given request contains acceptable values for processing.
        /// </summary>
        /// <param name="request">The request the examine for faults.</param>
        /// <returns>Returns true if the request is valid and therefore relatively safe for use in database transactions.</returns>
        private static bool IsValidRequest(IAuthorizationCodeGrantAccessTokenRequest request)
        {
            return request != null &&
                request.RedirectUri != null &&
                !string.IsNullOrEmpty(request.ClientId) &&
                !string.IsNullOrEmpty(request.ClientSecret) &&
                !string.IsNullOrEmpty(request.AuthorizationCode) &&
                "authorization_code".Equals(request.GrantType, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines if the given <see cref="IPasswordCredentialsAccessTokenRequest"/> is valid. (Contains all required values)
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <returns>Returns whether the given requests is relatively safe for processing.</returns>
        private bool IsValidRequest(IPasswordCredentialsAccessTokenRequest request)
        {
            return request != null &&
                "password".Equals(request.GrantType, StringComparison.Ordinal) &&
                !string.IsNullOrEmpty(request.ClientId) &&
                !string.IsNullOrEmpty(request.ClientSecret) &&
                request.User != null;
        }

        /// <summary>
        /// Creates a new <see cref="IUnsuccessfulAuthorizationCodeResponse"/> object that represents the given error code with the given exception.
        /// </summary>
        /// <param name="errorCode">The <see cref="AuthorizationCodeRequestErrorType"/> object that specifies what basic error occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="innerException">The exception that caused the error to occur.</param>
        /// <returns>Returns a new <see cref="IUnsuccessfulAuthorizationCodeResponse"/> object that represents a valid OAuth 2.0 Authorization Code Error resposne.</returns>
        private IUnsuccessfulAuthorizationCodeResponse CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType errorCode, string state, Uri redirect, Exception innerException = null)
        {
            return AuthorizationCodeResponseFactory.CreateError(errorCode, AuthorizationCodeErrorDescriptionProvider(errorCode), null, state, redirect, innerException);
        }

        /// <summary>
        /// Creates a new <see cref="IUnsuccessfulAccessTokenResponse"/> object that represents the given error code, for the given client and exception.
        /// </summary>
        /// <param name="errorCode">The <see cref="AccessTokenRequestError"/> object that specifies what basic error occurred.</param>
        /// <param name="client">The client that the error occured to.</param>
        /// <param name="exception">The exception that caused the error to occur.</param>
        /// <returns>Returns a new <see cref="IUnsuccessfulAccessTokenResponse"/> object that represents a valid OAuth 2.0 Access Token Error response (http://tools.ietf.org/html/rfc6749#section-5.2).</returns>
        private IUnsuccessfulAccessTokenResponse CreateAccessTokenError(AccessTokenRequestError errorCode, IClient client = null, Exception exception = null)
        {
            return AccessTokenResponseFactory.CreateError(
                errorCode,
                AccessTokenErrorDescriptionProvider(errorCode, client),
                AccessTokenErrorUriProvider(errorCode, client),
                exception);
        }

        /// <summary>
        /// Determines if the given <see cref="IRefreshToken"/>, <see cref="ITokenRefreshRequest"/> and <see cref="IClient"/> are valid.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="request"></param>
        /// <param name="client"></param>
        private static bool IsValidRefreshToken(IRefreshToken refreshToken, ITokenRefreshRequest request, IClient client)
        {
            return (refreshToken != null && refreshToken.Client.Equals(client) && refreshToken.IsValid() && refreshToken.MatchesValue(request.RefreshToken));
        }

        /// <summary>
        /// Determines if the given <see cref="IAuthorizationCode"/>, <see cref="IAuthorizationCodeGrantAccessTokenRequest"/> and <see cref="IClient"/> are valid.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="request"></param>
        /// <param name="client"></param>
        private static bool IsValidAuthorizationCode(IAuthorizationCode code, IAuthorizationCodeGrantAccessTokenRequest request, IClient client)
        {
            return code != null && code.Client.Equals(client) && code.IsValid() && code.MatchesValue(request.AuthorizationCode) && Uri.Compare(code.RedirectUri, request.RedirectUri, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// Determines if the given <see cref="IAccessTokenRequest"/> and <see cref="IClient"/> are valid.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="client"></param>
        private static bool IsValidClient(IAccessTokenRequest request, IClient client)
        {
            return client != null && client.MatchesSecret(request.ClientSecret);
        }

        /// <summary>
        /// Revokes access from the given client on behalf of the given user.
        /// </summary>
        /// <param name="user">The user that wants to revoke access from the given client.</param>
        /// <param name="client">The client to revoke access from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if one of the given arguments is null.</exception>
        public void RevokeAccess(IUser user, IClient client)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            AuthorizationCodeRepository.GetByUserAndClient(user, client).Where(c => c != null && !c.Revoked).ForEach(c => c.Revoke());
            AccessTokenRepository.GetByUserAndClient(user, client).Where(t => t != null && !t.Revoked).ForEach(t => t.Revoke());
            RefreshTokenRepository.GetByUserAndClient(user, client).Where(t => t != null && !t.Revoked).ForEach(t => t.Revoke());
        }

        /// <summary>
        /// Determines if the given client has been given access to the given scope by the given user.
        /// </summary>
        /// <param name="user">The User that is currently logged in.</param>
        /// <param name="client">The client to determine access for.</param>
        /// <param name="scope">The scope that the client wants access to.</param>
        /// <returns>Returns true if the client has access to the given users resources restricted by the given scope, otherwise false.</returns>
        public bool HasAccess(IUser user, IClient client, IScope scope)
        {
            IAccessToken token = AccessTokenRepository.GetByUserAndClient(user, client).FirstOrDefault(t => !t.Revoked && !t.Expired);
            if (token != null)
            {
                return ScopeRepository.Any(a => a.Equals(scope) && token.Scopes.Contains(a));
            }
            return false;
        }

        /// <summary>
        /// Gets the <see cref="IOAuthProviderDefinition"/> that contains information on the different endpoints provided by this
        /// <see cref="IOAuthProvider"/>.
        /// </summary>
        public IOAuthProviderDefinition Definition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether to distribue refresh tokens.
        /// </summary>
        public bool DistributeRefreshTokens
        {
            get;
            set;
        }
        = true;

        /// <summary>
        /// Gets or sets whether to reuse refresh tokens across newly issued tokens.
        /// </summary>
        public bool ReuseRefreshTokens
        {
            get;
            set;
        }
        = false;

        /// <summary>
        /// Gets or sets whether newly revoked tokens (access or refresh) should be deleted from their respective repository.
        /// </summary>
        /// <remarks>
        /// When <see cref="IOAuthProvider.RevokeAccess(IUser, IClient)"/> or <see cref="IOAuthProvider.RefreshAccessToken(ITokenRefreshRequest)"/> is called,
        /// access tokens and/or refresh tokens become invalidated. When this value is true, access tokens and refresh tokens that are revoked by this provider
        /// will be deleted by calling <see cref="IAccessTokenRepository{IAccessToken}.Remove(IAccessToken)"/> or <see cref="IRefreshTokenRepository{IRefreshToken}.Remove(IRefreshToken)"/>
        /// </remarks>
        public bool DeleteRevokedTokens
        {
            get;
            set;
        }
        = false;

        ~OAuthProvider()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (ClientRepository != null)
                    {
                        ClientRepository.Dispose();
                        ClientRepository = null;
                    }
                    if (AccessTokenRepository != null)
                    {
                        AccessTokenRepository.Dispose();
                        AccessTokenRepository = null;
                    }
                    if (ScopeRepository != null)
                    {
                        ScopeRepository.Dispose();
                        ScopeRepository = null;
                    }
                    if (AuthorizationCodeRepository != null)
                    {
                        AuthorizationCodeRepository.Dispose();
                        AuthorizationCodeRepository = null;
                    }
                    if (RefreshTokenRepository != null)
                    {
                        RefreshTokenRepository.Dispose();
                        RefreshTokenRepository = null;
                    }
                }
            }
            disposed = true;
        }
    }
}
