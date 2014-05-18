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



using OAuthWorks.DataAccess.Repositories;
using OAuthWorks.Factories;
using OAuthWorks.Implementation.Factories;
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
        /// <summary>
        /// Creates a new OAuthWorks.OAuthProvider using a default factories.
        /// </summary>
        public OAuthProvider() :
            this(
                new AccessTokenFactory(),
                new AccessTokenResponseFactory(),
                new AuthorizationCodeFactory(),
                new AuthorizationCodeResponseFactory(),
                new RefreshTokenFactory()
            )
        {
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
            this.AccessTokenFactory = accessTokenFactory;
            this.AuthorizationCodeFactory = authorizationCodeFactory;
            this.AccessTokenResponseFactory = accessTokenResponseFactory;
            this.AuthorizationCodeResponseFactory = authorizationCodeResponseFactory;
            this.RefreshTokenFactory = refreshTokenFactory;
        }

        public OAuthProvider(
            IAccessTokenRepository<IAccessToken> accessTokenRepository,
            IAuthorizationCodeRepository<IAuthorizationCode> authorizationCodeRepository,
            IScopeRepository<IScope> scopeRepository,
            IReadStore<string, IClient> clientRepository,
            IRefreshTokenRepository<IRefreshToken> refreshTokenRepository
            )
            : this()
        {
            Contract.Requires(accessTokenRepository != null);
            Contract.Requires(authorizationCodeRepository != null);
            Contract.Requires(scopeRepository != null);
            Contract.Requires(clientRepository != null);
            this.AccessTokenRepository = accessTokenRepository;
            this.AuthorizationCodeRepository = authorizationCodeRepository;
            this.ScopeRepository = scopeRepository;
            this.ClientRepository = clientRepository;
            this.RefreshTokenRepository = refreshTokenRepository;
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
        public IAuthorizationCodeRepository<IAuthorizationCode> AuthorizationCodeRepository
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
        public IAccessTokenRepository<IAccessToken> AccessTokenRepository
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
        public IRefreshTokenRepository<IRefreshToken> RefreshTokenRepository
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
        public IEnumerable<IScope> GetRequestedScopes(IAuthorizationCodeRequest request)
        {
            request.ThrowIfNull("request");
            IEnumerable<IScope> result = ScopeParser(this, request.Scope != null ? request.Scope : string.Empty);
            return result.All(s => s != null) ? result : new IScope[0];
        }

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
                    if (client != null && client.MatchesSecret(request.ClientSecret))
                    {
                        if (client.IsValidRedirectUri(request.RedirectUri))
                        {
                            IEnumerable<IScope> scopes = GetRequestedScopes(request);

                            if (scopes != null && scopes.Any())
                            {
                                ICreatedToken<IAuthorizationCode> authCode = AuthorizationCodeFactory.Create(request.RedirectUri, user, client, scopes);

                                //put the authorization code in the repository
                                AuthorizationCodeRepository.Add(authCode.Token);

                                //return a successful response
                                return AuthorizationCodeResponseFactory.Create(authCode.TokenValue, request.State);
                            }
                            else
                            {
                                return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.InvalidScope, request.State);
                            }
                        }
                        else
                        {
                            //Invalid redirect
                            return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.InvalidRequest, request.State);
                        }
                    }
                    else
                    {
                        return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.UnauthorizedClient, request.State);
                    }
                }
                catch (SystemException e)
                {
                    return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.ServerError, request.State, e);
                }
            }
            else
            {
                return CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType.InvalidRequest, null);
            }
        }

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
                    ICreatedToken<IAccessToken> accessToken = AccessTokenFactory.Create(client, code.User, code.Scopes);
                    ICreatedToken<IRefreshToken> refreshToken = null;
                    if (RefreshTokenFactory != null && DistributeRefreshTokens)
                    {
                        refreshToken = RefreshTokenFactory.Create(client, code.User, code.Scopes);
                        RefreshTokenRepository.Add(refreshToken.Token);
                    }
                    //store refresh and access tokens
                    AccessTokenRepository.Add(accessToken.Token);

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

                    if (!ReuseRefreshTokens)
                    {
                        ICreatedToken<IRefreshToken> newRefresh = RefreshTokenFactory.Create(client, refreshToken.User, refreshToken.Scopes);
                        refreshToken.Revoke();
                        if (DeleteRevokedTokens)
                        {
                            RefreshTokenRepository.Remove(refreshToken);
                        }
                        RefreshTokenRepository.Add(newRefresh.Token);
                        refreshValue = newRefresh.TokenValue;
                    }

                    ICreatedToken<IAccessToken> newToken = AccessTokenFactory.Create(client, refreshToken.User, refreshToken.Scopes);

                    AccessTokenRepository.Add(newToken.Token);

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
        /// Determines if the given request contains acceptable values for processing.
        /// </summary>
        /// <param name="request">The request the examine for faults.</param>
        /// <returns>Returns true if the request is valid and therefore relatively safe for use in database transactions.</returns>
        private bool IsValidRequest(IAuthorizationCodeRequest request)
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
                !string.IsNullOrEmpty(request.Scope) &&
                !string.IsNullOrEmpty(request.RefreshToken);
        }

        /// <summary>
        /// Determines if the given request contains acceptable values for processing.
        /// </summary>
        /// <param name="request">The request the examine for faults.</param>
        /// <returns>Returns true if the request is valid and therefore relatively safe for use in database transactions.</returns>
        private bool IsValidRequest(IAuthorizationCodeGrantAccessTokenRequest request)
        {
            return request != null &&
                request.RedirectUri != null &&
                !string.IsNullOrEmpty(request.ClientId) &&
                !string.IsNullOrEmpty(request.ClientSecret) &&
                !string.IsNullOrEmpty(request.AuthorizationCode) &&
                "access_token" == request.GrantType;
        }

        /// <summary>
        /// Creates a new <see cref="IUnsuccessfulAuthorizationCodeResponse"/> object that represents the given error code with the given exception.
        /// </summary>
        /// <param name="errorCode">The <see cref="AuthorizationCodeRequestErrorType"/> object that specifies what basic error occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="innerException">The exception that caused the error to occur.</param>
        /// <returns>Returns a new <see cref="IUnsuccessfulAuthorizationCodeResponse"/> object that represents a valid OAuth 2.0 Authorization Code Error resposne.</returns>
        private IUnsuccessfulAuthorizationCodeResponse CreateAuthorizationCodeError(AuthorizationCodeRequestErrorType errorCode, string state, Exception innerException = null)
        {
            return AuthorizationCodeResponseFactory.CreateError(errorCode, AuthorizationCodeErrorDescriptionProvider(errorCode), null, state, innerException);
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
        /// Requests an access refreshToken from the authorization server based on the given request using the Resource Owner Password Credentials flow. (Section 4.3 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.3).
        /// </summary>
        /// <param name="request">The request that contains the required values.</param>
        /// <returns>
        /// Returns a new <see cref="OAuthWorks.ISuccessfulAccessTokenResponse" /> object that determines what values to put in the outgoing response.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IAccessTokenResponse RequestAccessToken(IPasswordCredentialsAccessTokenRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if the given <see cref="IRefreshToken"/>, <see cref="ITokenRefreshRequest"/> and <see cref="IClient"/> are valid.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="request"></param>
        /// <param name="client"></param>
        private bool IsValidRefreshToken(IRefreshToken refreshToken, ITokenRefreshRequest request, IClient client)
        {
            return (refreshToken != null && refreshToken.Client.Equals(client) && refreshToken.IsValid() && refreshToken.MatchesValue(request.RefreshToken));
        }

        /// <summary>
        /// Determines if the given <see cref="IAuthorizationCode"/>, <see cref="IAuthorizationCodeGrantAccessTokenRequest"/> and <see cref="IClient"/> are valid.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="request"></param>
        /// <param name="client"></param>
        private bool IsValidAuthorizationCode(IAuthorizationCode code, IAuthorizationCodeGrantAccessTokenRequest request, IClient client)
        {
            return code != null && code.Client.Equals(client) && code.IsValid() && code.MatchesValue(request.AuthorizationCode) && Uri.Compare(code.RedirectUri, request.RedirectUri, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// Determines if the given <see cref="IAccessTokenRequest"/> and <see cref="IClient"/> are valid.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="client"></param>
        private bool IsValidClient(IAccessTokenRequest request, IClient client)
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

            foreach (IAccessToken token in AccessTokenRepository.GetByUserAndClient(user, client).Where(t => t != null && !t.Revoked))
            {
                token.Revoke();
            }

            RefreshTokenRepository.GetByUserAndClient(user, client).Where(t => t != null & !t.Revoked).ForEach(t => t.Revoke());
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

        /// <summary>
        /// Gets or sets whether to reuse refresh tokens across newly issued tokens.
        /// </summary>
        public bool ReuseRefreshTokens
        {
            get;
            set;
        }

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
            if (disposing)
            {
                if (!disposed)
                {
                    if (this.ClientRepository != null)
                    {
                        ClientRepository.Dispose();
                    }
                    if (this.AccessTokenRepository != null)
                    {
                        AccessTokenRepository.Dispose();
                    }
                    if (this.ScopeRepository != null)
                    {
                        ScopeRepository.Dispose();
                    }
                    if (this.AuthorizationCodeRepository != null)
                    {
                        this.AuthorizationCodeRepository.Dispose();
                    }
                    if (this.RefreshTokenRepository != null)
                    {
                        this.RefreshTokenRepository.Dispose();
                    }
                }
            }
            this.disposed = true;
        }
    }
}
