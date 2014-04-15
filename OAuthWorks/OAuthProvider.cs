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



using OAuthWorks.Factories;
using OAuthWorks.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Auth")]
    public class OAuthProvider : IOAuthProvider
    {
        internal class DefaultDependencyInjector : IDependencyInjector
        {
            public T GetInstance<T>()
            {
                return default(T);
            }
        }

        /// <summary>
        /// Creates a new OAuthWorks.OAuthProvider using a default dependency injector instance.
        /// </summary>
        public OAuthProvider()
        {
            this.DependencyInjector = new DefaultDependencyInjector();
        }

        /// <summary>
        /// Creates a new OAuthWorks.OAuthProvider using the given OAuthWorks.IDependencyInjector to retrieve
        /// factories and repositories that are used by the provider.
        /// </summary>
        /// <param name="dependencyInjector">An object that implements the OAuthWorks.IDependencyInjector interface.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the given <paramref name="dependencyInjector"/> is null.</exception>
        public OAuthProvider(IDependencyInjector dependencyInjector)
        {
            if (dependencyInjector == null)
            {
                throw new ArgumentNullException("dependencyInjector");
            }
            this.DependencyInjector = dependencyInjector;
        }

        /// <summary>
        /// Gets the dependency injector that is used to retrieve all of the repositories and factories for this object.
        /// </summary>
        public IDependencyInjector DependencyInjector
        {
            get;
            private set;
        }

        private IAccessTokenFactory<IAccessToken> accessTokenFactory;

        private IScopeRepository<IScope> scopeRepository;

        /// <summary>
        /// Gets or sets the repository of scopes that this provider has access to.
        /// </summary>
        /// <remarks>
        /// This property tries to retrieve it's value from the dependency injector when it is null.
        /// If accessing this value results in null being returned, then the provider could not retrieve a default value from the dependency injector.
        /// </remarks>
        public IScopeRepository<IScope> ScopeRepository
        {
            get
            {
                if (this.scopeRepository == null)
                {
                    scopeRepository = DependencyInjector.GetInstance<IScopeRepository<IScope>>();
                }
                return scopeRepository;
            }
            set
            {
                this.scopeRepository = value;
            }
        }

        private IReadStore<string, IClient> clientRepository;

        /// <summary>
        /// Gets or sets the repository that contains <see cref="OAuthWorks.IClient"/> objects.
        /// </summary>
        public IReadStore<string, IClient> ClientRepository
        {
            get
            {
                if (this.clientRepository == null)
                {
                    this.clientRepository = DependencyInjector.GetInstance<IReadStore<string, IClient>>();
                }
                return clientRepository;
            }
            set
            {
                this.clientRepository = value;
            }
        }

        private IAuthorizationCodeResponseFactory<IAuthorizationCodeResponse, AuthorizationCodeResponseException> authorizationCodeResponseFactory;

        /// <summary>
        /// Gets or sets the factory used to create new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> objects.
        /// </summary>
        public IAuthorizationCodeResponseFactory<IAuthorizationCodeResponse, AuthorizationCodeResponseException> AuthorizationCodeResponseFactory
        {
            get
            {
                if (this.authorizationCodeResponseFactory == null)
                {
                    this.authorizationCodeResponseFactory = DependencyInjector.GetInstance<IAuthorizationCodeResponseFactory<IAuthorizationCodeResponse, AuthorizationCodeResponseException>>();
                }
                return this.authorizationCodeResponseFactory;
            }
            set
            {
                this.authorizationCodeResponseFactory = value;
            }
        }

        private IAuthorizationCodeRepository<IAuthorizationCode> authorizationCodeRepository;

        /// <summary>
        /// Gets or sets the repository of Authorization Code objects that this provider has access to.
        /// </summary>
        public IAuthorizationCodeRepository<IAuthorizationCode> AuthorizationCodeRepository
        {
            get
            {
                if (this.authorizationCodeRepository == null)
                {
                    this.authorizationCodeRepository = DependencyInjector.GetInstance<IAuthorizationCodeRepository<IAuthorizationCode>>();
                }
                return this.authorizationCodeRepository;
            }
            set
            {
                this.authorizationCodeRepository = value;
            }
        }

        private IAuthorizationCodeFactory<IAuthorizationCode> authorizationCodeFactory;

        /// <summary>
        /// Gets or sets the factory that creates Authorization Code objects for this provider.
        /// </summary>
        public IAuthorizationCodeFactory<IAuthorizationCode> AuthorizationCodeFactory
        {
            get
            {
                if (this.authorizationCodeFactory == null)
                {
                    this.authorizationCodeFactory = DependencyInjector.GetInstance<IAuthorizationCodeFactory<IAuthorizationCode>>();
                }
                return this.authorizationCodeFactory;
            }
            set
            {
                this.authorizationCodeFactory = value;
            }
        }

        private IAccessTokenRepository<IAccessToken> accessTokenRepository;

        /// <summary>
        /// Gets or sets the repository of Access Token objects that this provider has access to.
        /// </summary>
        public IAccessTokenRepository<IAccessToken> AccessTokenRepository
        {
            get
            {
                if (accessTokenRepository == null)
                {

                    accessTokenRepository = DependencyInjector.GetInstance<IAccessTokenRepository<IAccessToken>>();
                }
                return this.accessTokenRepository;
            }
            set
            {
                accessTokenRepository = value;
            }
        }

        /// <summary>
        /// Gets or sets the factory that creates Access Token objects for this provider.
        /// </summary>
        public IAccessTokenFactory<IAccessToken> AccessTokenFactory
        {
            get
            {
                if (accessTokenFactory == null)
                {
                    accessTokenFactory = DependencyInjector.GetInstance<IAccessTokenFactory<IAccessToken>>();
                }
                return accessTokenFactory;
            }
            set
            {
                accessTokenFactory = value;
            }
        }

        private IAccessTokenResponseFactory<IAccessTokenResponse, AccessTokenResponseException> accessTokenResponseFactory;

        /// <summary>
        /// Gets or sets the factory that creates <see cref="OAuthWorks.IAccessTokenResponse"/> objects for this provider.
        /// </summary>
        public IAccessTokenResponseFactory<IAccessTokenResponse, AccessTokenResponseException> AccessTokenResponseFactory
        {
            get
            {
                if (accessTokenResponseFactory == null)
                {
                    accessTokenResponseFactory = DependencyInjector.GetInstance<IAccessTokenResponseFactory<IAccessTokenResponse, AccessTokenResponseException>>();
                }
                return accessTokenResponseFactory;
            }
            set
            {
                value.ThrowIfNull("value");
                accessTokenResponseFactory = value;
            }
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
        public IRefreshTokenRepository<IRefreshToken> RefreshTokenRepository
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the formatter for scopes. That is, a function that, given a list of scopes, returns a string representing those scopes.
        /// </summary>
        public Func<IEnumerable<IScope>, string> ScopeFormatter
        {
            get;
            set;
        }

        private Func<OAuthProvider, string, IEnumerable<IScope>> scopeParser;

        /// <summary>
        /// Gets or sets the parser for the scopes. That is, a function that, given a string, returns a list of scopes representing that string.
        /// </summary>
        public Func<OAuthProvider, string, IEnumerable<IScope>> ScopeParser
        {
            get
            {
                return scopeParser;
            }
            set
            {
                if (value != null)
                {
                    scopeParser = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the error description provider. That is, a function that, given the error and client, returns a string describing the problem.
        /// </summary>
        public Func<AccessTokenRequestError, IClient, string> AccessTokenErrorDescriptionProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error uri provider. That is, a function that, given the error and client, returns a string containing a uri that points to a web page providing information
        /// on the error.
        /// </summary>
        public Func<AccessTokenRequestError, IClient, Uri> AccessTokenErrorUriProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of the scopes that are being requested by the given OAuthWorks.IAuthorizationCodeRequest object.
        /// </summary>
        /// <param name="request">The request that is being used to request an authorization code.</param>
        /// <returns>Returns a non-null enumerable list of scopes that define the permissions requested.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the given request object is null.</exception>
        public IEnumerable<IScope> GetRequestedScopes(IAuthorizationCodeRequest request)
        {
            request.ThrowIfNull("request");
            IEnumerable<IScope> result = ScopeParser(this, request.Scope != null ? request.Scope : string.Empty);
            return result != null ? result : new IScope[0];
        }

        /// <summary>
        /// Initiates the Authorization Code flow based on the given request and returns a response that defines what response to send back to the user agent.
        /// Be sure to authenticate the user and request consent before calling this. THIS METHOD ASSUMES THAT USER CONSENT WAS GIVEN.
        /// </summary>
        /// <param name="request">The request that contains the values that were sent by the client.</param>
        /// <param name="user">The user that the request is for.</param>
        /// <exception cref="OAuthWorks.AuthorizationCodeResponseException">Thrown if an exception occurs inside this method or if the given request was invalid in some way.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the given request is null.</exception>
        /// <returns>Returns a new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> object that determines what values to put in the outgoing response.</returns>
        public IAuthorizationCodeResponse RequestAuthorizationCode(IAuthorizationCodeRequest request, IUser user)
        {
            request.ThrowIfNull("request");
            try
            {
                IClient client = ClientRepository.GetById(request.ClientId);
                if (client != null && client.MatchesSecret(request.ClientSecret))
                {
                    if (client.IsValidRedirectUri(request.RedirectUri))
                    {
                        IEnumerable<IScope> scopes = GetRequestedScopes(request);

                        ICreatedToken<IAuthorizationCode> authCode = AuthorizationCodeFactory.Create(request.RedirectUri, user, scopes);

                        //put the authorization code in the repository
                        AuthorizationCodeRepository.Add(authCode.Token);

                        //return a successful response
                        return AuthorizationCodeResponseFactory.Create(authCode.TokenValue, request.State);
                    }
                    else
                    {
                        //Invalid redirect
                        throw AuthorizationCodeResponseFactory.CreateError(AuthorizationRequestCodeErrorType.InvalidRequest, "The given redirect uri was invalid", null, request.State, null);
                    }
                }
                else
                {
                    throw AuthorizationCodeResponseFactory.CreateError(AuthorizationRequestCodeErrorType.UnauthorizedClient, "The client is not authorized to access this resource.", null, request.State, null);
                }
            }
            catch (Exception e)
            {
                throw AuthorizationCodeResponseFactory.CreateError(AuthorizationRequestCodeErrorType.ServerError, "The server encountered an error while processing the request.", null, request.State, e);
            }
        }

        /// <summary>
        /// Requests an access token from the server with the request.
        /// </summary>
        /// <param name="request">The incoming request for an access token.</param>
        /// <exception cref="OAuthWorks.AccessTokenResponseException">Thrown if the client is unauthorized or if any other exception occured inside this method.</exception>
        /// <exception cref="System.ArgumentNullException">Throw if the given request is null or if the given user is null.</exception>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object that represents what to </returns>
        public IAccessTokenResponse RequestAccessToken(IAccessTokenRequest request, IUser currentUser)
        {
            request.ThrowIfNull("request");
            currentUser.ThrowIfNull("currentUser");
            try
            {
                IClient client = ClientRepository.GetById(request.ClientId);
                validateClient(request, client);

                IAuthorizationCode code = AuthorizationCodeRepository.GetByValue(request.AuthorizationCode);
                
                validateAuthorizationCode(code, request, client);

                //Authorized!
                ICreatedToken<IAccessToken> accessToken = AccessTokenFactory.Create(client, currentUser, code.Scopes);
                ICreatedToken<IRefreshToken> refreshToken = null;
                if (RefreshTokenFactory != null && DistributeRefreshTokens)
                {
                    refreshToken = RefreshTokenFactory.Create(client, currentUser, code.Scopes);
                }
                //store refresh and access tokens
                AccessTokenRepository.Add(accessToken.Token);
                if (RefreshTokenRepository != null && refreshToken != null)
                {
                    RefreshTokenRepository.Add(refreshToken.Token);
                }

                return AccessTokenResponseFactory.Create(
                    accessToken.TokenValue,
                    refreshToken != null ? refreshToken.TokenValue : null,
                    accessToken.Token.TokenType,
                    ScopeFormatter(accessToken.Token.Scopes),
                    accessToken.Token.ExpirationDateUtc);
            }
            catch (Exception e)
            {
                //Server error
                throw AccessTokenResponseFactory.CreateError(
                    AccessTokenRequestError.ServerError,
                    AccessTokenErrorDescriptionProvider(AccessTokenRequestError.ServerError, null),
                    AccessTokenErrorUriProvider(AccessTokenRequestError.ServerError, null),
                    e);
            }
        }

        /// <summary>
        /// Validates the given authorization code in the context of the request and client and throws a <see cref="OAuthWorks.AccessTokenResponseException"/> if the code is invalid.
        /// </summary>
        /// <param name="client"></param>
        private void validateAuthorizationCode(IAuthorizationCode code, IAccessTokenRequest request, IClient client)
        {
            if (!(code != null && !code.Expired && code.MatchesValue(request.AuthorizationCode) && Uri.Compare(code.RedirectUri, request.RedirectUri, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0))
            {
                throw AccessTokenResponseFactory.CreateError(
                    AccessTokenRequestError.InvalidClient,
                    AccessTokenErrorDescriptionProvider(AccessTokenRequestError.InvalidClient, client),
                    AccessTokenErrorUriProvider(AccessTokenRequestError.InvalidClient, client),
                    null);
            }
        }

        /// <summary>
        /// Validates the client based on the given request and throws a <see cref="OAuthWorks.AccessTokenResponseException"/> if the client is not valid.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="client"></param>
        private void validateClient(IAccessTokenRequest request, IClient client)
        {
            if (!(client != null && client.MatchesSecret(request.ClientSecret)))
            {
                //Invalid authorization code grant
                throw AccessTokenResponseFactory.CreateError(
                    AccessTokenRequestError.InvalidGrant,
                    AccessTokenErrorDescriptionProvider(AccessTokenRequestError.InvalidGrant, client),
                    AccessTokenErrorUriProvider(AccessTokenRequestError.InvalidGrant, client),
                    null);
            }
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

            IAccessToken token = AccessTokenRepository.GetByUserAndClient(user, client);
            if (token != null && !token.Revoked)
            {
                token.Revoke();
            }

            IRefreshToken refreshToken = RefreshTokenRepository.GetByUserAndClient(user, client);
            if (refreshToken != null && !refreshToken.Revoked)
            {
                token.Revoke();
            }
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
            IAccessToken token = AccessTokenRepository.GetByUserAndClient(user, client);
            if (token != null && !token.Revoked && !token.Expired)
            {
                return ScopeRepository.GetAllScopes().Any(a => a.Equals(scope) && token.Scopes.Contains(a));
            }
            return false;
        }

        /// <summary>
        /// Gets the OAuthWorks.IOAuthProviderDefintion that contains information on the different endpoints provided by this
        /// IOAuthProvider.
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

    }
}
