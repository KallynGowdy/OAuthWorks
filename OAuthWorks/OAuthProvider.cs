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
using OAuthWorks.Implementation;
using OAuthWorks.ExtensionMethods;

namespace OAuthWorks
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Auth")]
    public class OAuthProvider : IOAuthProvider
    {
        #region Defaults
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
        public static readonly Func<IAuthorizationCodeResponseFactory> DefaultAuthorizationCodeResponseFactoryConstructor = () => new AuthorizationCodeResponseFactory();

        /// <summary>
        /// The default <see cref="IRefreshTokenFactory{TRefreshToken}"/> constructor.
        /// </summary>
        public static readonly Func<IRefreshTokenFactory<IRefreshToken>> DefaultRefreshTokenFactoryConstructor = () => Implementation.Factories.RefreshTokenFactory.String.DefaultFactory;

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new <see cref="OAuthProvider"/> using a default factories.
        /// </summary>
        public OAuthProvider() :
            this(
                DefaultAccessTokenFactoryConstructor(),
                DefaultAccessTokenResponseFactoryConstructor(),
                DefaultAuthorizationCodeFactoryConstructor(),
                DefaultAuthorizationCodeResponseFactoryConstructor(),
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

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthProvider"/> class.
        /// </summary>
        /// <param name="accessTokenFactory">The access token factory.</param>
        /// <param name="accessTokenResponseFactory">The access token response factory.</param>
        /// <param name="authorizationCodeFactory">The authorization code factory.</param>
        /// <param name="authorizationCodeResponseFactory">The authorization code response factory.</param>
        /// <param name="refreshTokenFactory">The refresh token factory.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthProvider"/> class.
        /// </summary>
        /// <param name="accessTokenRepository">The access token repository.</param>
        /// <param name="authorizationCodeRepository">The authorization code repository.</param>
        /// <param name="scopeRepository">The scope repository.</param>
        /// <param name="clientRepository">The client repository.</param>
        /// <param name="refreshTokenRepository">The refresh token repository.</param>
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

        #endregion

        #region Data Members

        #region Repositories
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
        /// Gets or sets the repository of Authorization Code objects that this provider has access to.
        /// </summary>
        public IAuthorizationCodeRepository AuthorizationCodeRepository
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
        /// Gets or sets the repository that is used to store OAuthWorks.IRefreshToken objects.
        /// </summary>
        public IRefreshTokenRepository RefreshTokenRepository
        {
            get;
            set;
        }
        #endregion

        #region Factories
        /// <summary>
        /// Gets or sets the factory used to create new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> objects.
        /// </summary>
        public IAuthorizationCodeResponseFactory AuthorizationCodeResponseFactory
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
        #endregion

        #region Scope Utilities
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
        #endregion

        #region Description/Uri Providers
        private IAccessTokenErrorDescriptionProvider accessTokenErrorDescriptionProvider = new AccessTokenErrorDescriptionProvider();

        private Func<AccessTokenRequestError, IClient, Uri> accessTokenErrorUriProvider = (e, c) => null;

        /// <summary>
        /// Gets or sets the access token error description provider. That is, a function that, given the error and client, returns a string describing the problem.
        /// </summary>
        public IAccessTokenErrorDescriptionProvider AccessTokenErrorDescriptionProvider
        {
            get
            {
                return accessTokenErrorDescriptionProvider;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                accessTokenErrorDescriptionProvider = value;
            }
        }

        private IAuthorizationCodeErrorDescriptionProvider authorizationCodeErrorDescriptionProvider = new AuthorizationCodeErrorDescriptionProvider();

        /// <summary>
        /// Gets or sets the error description provider. That is, a function that, given the error, returns a string describing the problem.
        /// </summary>
        /// <returns>Returns the authorization code error description provider used to provide descriptions for errors.</returns>
        public IAuthorizationCodeErrorDescriptionProvider AuthorizationCodeErrorDescriptionProvider
        {
            get
            {
                return authorizationCodeErrorDescriptionProvider;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
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
                if (value == null) throw new ArgumentNullException("value");
                accessTokenErrorUriProvider = value;
            }
        }
        #endregion

        #region Token Distribution/Cleanup Options
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
        #endregion

        #endregion

        #region IOAuthProvider Implementation

        /// <summary>
        /// Gets a list of the scopes that are being requested by the given <see cref="IAuthorizationCodeRequest"/> object.
        /// </summary>
        /// <param name="request">The request that is being used to request an authorization code.</param>
        /// <returns>Returns an enumerable list of scopes that define the permissions requested. Null if one of the requested scopes are invalid.</returns>
        public IEnumerable<IScope> GetRequestedScopes(string scopes)
        {
            IEnumerable<IScope> result = ScopeParser(this, scopes != null ? scopes : string.Empty);
            return result.All(s => s != null) ? result : new IScope[0];
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")] // Validated by call to 'GetRequestError()'
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")] // Suppressed to be able to return IAuthorizationCodeResponse objects according to OAuth 2.0
        /// <summary>
        /// Initiates the Authorization Code flow based on the given request and returns a response that defines what response to send back to the user agent.
        /// Be sure to authenticate the user and request consent before calling this. THIS METHOD ASSUMES THAT USER CONSENT WAS GIVEN.
        /// </summary>
        /// <param name="request">The request that contains the values that were sent by the client.</param>
        /// <param name="user">The user that the request is for.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> object that determines what values to put in the outgoing response.</returns>
        public IAuthorizationCodeResponse RequestAuthorizationCode(IAuthorizationCodeRequest request, IUser user)
        {
            AuthorizationCodeRequestSpecificErrorType? error = GetRequestError(request);
            if (!error.HasValue)
            {
                try
                {
                    IClient client = ClientRepository.GetById(request.ClientId);
                    if ((error = GetClientError(request, client)) == null)
                    {
                        if (client.IsValidRedirectUri(request.RedirectUri))
                        {
                            IEnumerable<IScope> scopes = GetRequestedScopes(request.Scope);

                            if (scopes != null && scopes.Any())
                            {
                                if (scopes.All(s => user.HasGrantedScope(client, s)))
                                {

                                    //Revoke all of the current authorization codes.
                                    AuthorizationCodeRepository.GetByUserAndClient(user, client).ForEach(c => c.Revoke());

                                    ICreatedToken<IAuthorizationCode> authCode = AuthorizationCodeFactory.Create(request.RedirectUri, user, client, scopes);
                                    AuthorizationCodeRepository.Add(authCode);

                                    //return a successful response
                                    return AuthorizationCodeResponseFactory.Create(authCode.TokenValue, request.RedirectUri, new ProcessedAuthorizationCodeRequest
                                        (
                                            request,
                                            client,
                                            user,
                                            scopes
                                        ));
                                }
                                else
                                {
                                    return CreateAuthorizationCodeError(
                                        AuthorizationCodeRequestSpecificErrorType.UserUnauthorizedScopes,
                                        request.RedirectUri,
                                        request,
                                        client,
                                        user,
                                        scopes);
                                }
                            }
                            else
                            {
                                return CreateAuthorizationCodeError(
                                    AuthorizationCodeRequestSpecificErrorType.MissingOrUnknownScope,
                                    request.RedirectUri,
                                    request,
                                    client,
                                    user,
                                    scopes);
                            }
                        }
                        else
                        {
                            //Invalid redirect
                            return CreateAuthorizationCodeError(
                                AuthorizationCodeRequestSpecificErrorType.InvalidRedirectUri,
                                null,
                                request,
                                client,
                                user);
                        }
                    }
                    else
                    {
                        return CreateAuthorizationCodeError(
                            error.Value,
                            request.RedirectUri,
                            request,
                            user: user,
                            client: client);
                    }
                }
                catch (SystemException e)
                {
                    return CreateAuthorizationCodeError(
                        AuthorizationCodeRequestSpecificErrorType.ServerError,
                        request.RedirectUri,
                        request,
                        user: user,
                        innerException: e);
                }
            }
            else
            {
                return CreateAuthorizationCodeError(error.Value, null, null, user: user);
            }
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
            AccessTokenSpecificRequestError? requestError = GetRequestError(request);
            if (requestError == null)
            {
                try
                {
                    IClient client = ClientRepository.GetById(request.ClientId);
                    if ((requestError = GetClientError(request, client)).HasValue)
                    {
                        return CreateAccessTokenError(requestError.Value, client);
                    }

                    IAuthorizationCode code = AuthorizationCodeRepository.GetByValue(request.AuthorizationCode);
                    if ((requestError = GetAuthorizationCodeError(code, request, client)).HasValue)
                    {
                        return CreateAccessTokenError(requestError.Value, client);
                    }

                    //Authorized!

                    code.Revoke(); // Prevent this same code from being used again

                    ICreatedToken<IAccessToken> accessToken = AccessTokenFactory.Create(client, code.User, code.Scopes);
                    ICreatedToken<IRefreshToken> refreshToken = null;
                    if (RefreshTokenFactory != null && DistributeRefreshTokens)
                    {
                        refreshToken = RefreshTokenFactory.Create(client, code.User, code.Scopes);
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
                    return CreateAccessTokenError(AccessTokenSpecificRequestError.ServerError, null, e);
                }
            }
            else
            {
                return CreateAccessTokenError(requestError.Value, null, null);
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
            AccessTokenSpecificRequestError? error = GetRequestError(request);
            if (!error.HasValue)
            {
                try
                {
                    IClient client = ClientRepository.GetById(request.ClientId);
                    if ((error = GetClientError(request, client)) != null)
                    {
                        return CreateAccessTokenError(error.Value, client);
                    }

                    IRefreshToken refreshToken = RefreshTokenRepository.GetByValue(request.RefreshToken);
                    if ((error = GetRefreshTokenError(refreshToken, request, client)) != null)
                    {
                        return CreateAccessTokenError(error.Value, client);
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

                    ICreatedToken<IAccessToken> newToken = AccessTokenFactory.Create(client, refreshToken.User, refreshToken.Scopes);

                    AccessTokenRepository.Add(newToken);

                    if (!ReuseRefreshTokens)
                    {
                        ICreatedToken<IRefreshToken> newRefresh = RefreshTokenFactory.Create(client, refreshToken.User, refreshToken.Scopes);
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
                    return CreateAccessTokenError(AccessTokenSpecificRequestError.ServerError, null, e);
                }
            }
            else
            {
                return CreateAccessTokenError(error.Value);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")] // Validated by call to 'GetRequestError()'
        /// <summary>
        /// Requests an access refreshToken from the authorization server based on the given request using the Resource Owner Password Credentials flow. (Section 4.3 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.3).
        /// </summary>
        /// <param name="request">The request that contains the required values.</param>
        /// <returns>
        /// Returns a new <see cref="OAuthWorks.ISuccessfulAccessTokenResponse" /> object that determines what values to put in the outgoing response.
        /// </returns>
        public virtual IAccessTokenResponse RequestAccessToken(IPasswordCredentialsAccessTokenRequest request)
        {
            AccessTokenSpecificRequestError? error = GetRequestError(request);
            if (!error.HasValue)
            {
                try
                {
                    IClient client = ClientRepository.GetById(request.ClientId);
                    if ((error = GetClientError(request, client)).HasValue)
                    {
                        return CreateAccessTokenError(error.Value, client);
                    }

                    IEnumerable<IScope> scopes = GetRequestedScopes(request.Scope);
                    if (scopes != null && scopes.Any())
                    {
                        AccessTokenRepository.GetByUserAndClient(request.User.User, client).ToArray().ForEach(t =>
                        {
                            t.Revoke();
                            if (DeleteRevokedTokens)
                                AccessTokenRepository.Remove(t);
                        });

                        RefreshTokenRepository.GetByUserAndClient(request.User.User, client).ToArray().ForEach(t =>
                        {
                            t.Revoke();
                            if (DeleteRevokedTokens)
                                RefreshTokenRepository.Remove(t);
                        });

                        ICreatedToken<IAccessToken> token = AccessTokenFactory.Create(client, request.User.User, scopes);
                        ICreatedToken<IRefreshToken> refreshToken = null;
                        if (RefreshTokenFactory != null && DistributeRefreshTokens)
                        {
                            refreshToken = RefreshTokenFactory.Create(client, request.User.User, scopes);
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
                        return CreateAccessTokenError(AccessTokenSpecificRequestError.InvalidScope, client);
                    }
                }
                catch (SystemException e)
                {
                    return CreateAccessTokenError(AccessTokenSpecificRequestError.ServerError, null, e);
                }
            }
            else
            {
                return CreateAccessTokenError(error.Value);
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

            AuthorizationCodeRepository.GetByUserAndClient(user, client).Where(c => c != null && !c.Revoked).ForEach(c => c.Revoke());
            AccessTokenRepository.GetByUserAndClient(user, client).Where(t => t != null && !t.Revoked).ForEach(t => t.Revoke());
            RefreshTokenRepository.GetByUserAndClient(user, client).Where(t => t != null && !t.Revoked).ForEach(t => t.Revoke());
        }

        /// <summary>
        /// Validates the given authorization values (access token) and returns a result representing whether or not it was successful and what was wrong with it.
        /// </summary>
        /// <param name="request">An object that contains values that were provided by the client to be used for authorization.</param>
        /// <returns>
        /// Returns a new <see cref="IAuthorizationResult" />
        /// </returns>
        public IAuthorizationResult ValidateAuthorization(IAuthorizationRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            if ("bearer".Equals(request.Type, StringComparison.OrdinalIgnoreCase))
            {
                IAccessToken token = AccessTokenRepository.GetByToken(request.Authorization);

                if (token != null)
                {
                    if (!token.Expired)
                    {
                        if (!token.Revoked)
                        {
                            if (request.RequiredScopes.Any(scopes => scopes.All(s => token.Scopes.Contains(s))))
                            {
                                return AuthorizationResult.Success(token);
                            }
                            else
                            {
                                return AuthorizationResult.Failure.NotGrantedPermission(token);
                            }
                        }
                        else
                        {
                            return AuthorizationResult.Failure.RevokedToken(token);
                        }
                    }
                    else
                    {
                        return AuthorizationResult.Failure.ExpiredToken(token);
                    }
                }
                else
                {
                    return AuthorizationResult.Failure.MissingToken;
                }
            }
            else
            {
                return AuthorizationResult.Failure.UnsupportedType;
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
            IAccessToken token = AccessTokenRepository.GetByUserAndClient(user, client).FirstOrDefault(t => !t.Revoked && !t.Expired);
            if (token != null)
            {
                return ScopeRepository.Any(a => a.Equals(scope) && token.Scopes.Contains(a));
            }
            return false;
        }
        #endregion

        #region Validation
        /// <summary>
        /// Determines if the given <see cref="IAuthorizationCodeRequest"/> is valid for the given <see cref="IClient"/> and returns a value specifiying what was wrong if it wasn't valid.
        /// </summary>
        /// <param name="request">The request that should examined to see if the proper client credientials were given.</param>
        /// <param name="client">The client that the request should be validated against.</param>
        /// <returns>Returns a new <see cref="AuthorizationCodeRequestSpecificErrorType"/> object that specifies what was wrong with the given client. Returns null if nothing was wrong.</returns>
        protected virtual AuthorizationCodeRequestSpecificErrorType? GetClientError(IAuthorizationCodeRequest request, IClient client)
        {
            if (client == null)
            {
                return AuthorizationCodeRequestSpecificErrorType.MissingClient;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the first error that is wrong with the given request, returns null if nothing is directly wrong.
        /// </summary>
        /// <param name="request">The request to examine for faults.</param>
        /// <returns>Returns the error that identifies what is wrong with the given request, returns null if nothing is wrong.</returns>
        protected virtual AccessTokenSpecificRequestError? GetRequestError(IAuthorizationCodeGrantAccessTokenRequest request)
        {
            if (request == null)
            {
                return AccessTokenSpecificRequestError.NullRequest;
            }
            else if (request.RedirectUri == null)
            {
                return AccessTokenSpecificRequestError.NullRedirectUri;
            }
            else if (string.IsNullOrEmpty(request.ClientId))
            {
                return AccessTokenSpecificRequestError.NullClientId;
            }
            else if (string.IsNullOrEmpty(request.ClientSecret))
            {
                return AccessTokenSpecificRequestError.NullClientSecret;
            }
            else if (string.IsNullOrEmpty(request.AuthorizationCode))
            {
                return AccessTokenSpecificRequestError.NullAuthorizationCode;
            }
            else if(string.IsNullOrEmpty(request.GrantType))
            {
                return AccessTokenSpecificRequestError.NullGrantType;
            }
            else if (!"authorization_code".Equals(request.GrantType, StringComparison.Ordinal))
            {
                return AccessTokenSpecificRequestError.InvalidGrantType;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines if the given request contains acceptable values for processing and returns the reason why a given request isn't acceptable.
        /// </summary>
        /// <param name="request">The request the examine for faults.</param>
        /// <returns>Returns the <see cref="AuthorizationCodeRequestSpecificErrorType"/> object that specifies what was wrong, returns null if nothing was wrong.</returns>
        private static AuthorizationCodeRequestSpecificErrorType? GetRequestError(IAuthorizationCodeRequest request)
        {
            if (request == null)
            {
                return AuthorizationCodeRequestSpecificErrorType.NullRequest;
            }
            else if (request.RedirectUri == null)
            {
                return AuthorizationCodeRequestSpecificErrorType.NullRedirect;
            }
            else if (string.IsNullOrEmpty(request.ClientId))
            {
                return AuthorizationCodeRequestSpecificErrorType.NullClientId;
            }
            else if (string.IsNullOrEmpty(request.Scope))
            {
                return AuthorizationCodeRequestSpecificErrorType.NullScope;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the first error that is wrong with the given refresh token, request and client, returns null if nothing is directly wrong.
        /// </summary>
        /// <param name="refreshToken">The token to examine with the request.</param>
        /// <param name="request">The request to examine with the token and client.</param>
        /// <param name="client">The client to examine with the token and request.</param>
        /// <returns>Returns the <see cref="AuthorizationCodeRequestSpecificErrorType"/> object that specifies what was wrong, returns null if nothing was wrong.</returns>
        protected virtual AccessTokenSpecificRequestError? GetRefreshTokenError(IRefreshToken refreshToken, ITokenRefreshRequest request, IClient client)
        {
            if (refreshToken == null)
            {
                return AccessTokenSpecificRequestError.NullRefreshToken;
            }
            else if (!refreshToken.Client.Equals(client))
            {
                return AccessTokenSpecificRequestError.WrongClient;
            }
            else if (!refreshToken.IsValid())
            {
                return AccessTokenSpecificRequestError.TokenNoLongerValid;
            }
            else if (!refreshToken.MatchesValue(request.RefreshToken))
            {
                return AccessTokenSpecificRequestError.InvalidGrant;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the first error that is wrong with the given refresh token, request and client, returns null if nothing is directly wrong.
        /// </summary>
        /// <param param name="request">The request to examine for faults.</param>
        /// <returns>Returns the <see cref="AuthorizationCodeRequestSpecificErrorType"/> object that specifies what was wrong, returns null if nothing was wrong.</returns>
        protected virtual AccessTokenSpecificRequestError? GetRequestError(ITokenRefreshRequest request)
        {
            if (request == null)
            {
                return AccessTokenSpecificRequestError.NullRequest;
            }
            else if (string.IsNullOrEmpty(request.ClientId))
            {
                return AccessTokenSpecificRequestError.NullClientId;
            }
            else if (string.IsNullOrEmpty(request.ClientSecret))
            {
                return AccessTokenSpecificRequestError.NullClientSecret;
            }
            else if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return AccessTokenSpecificRequestError.NullRefreshToken;
            }
            else if (!"refresh_token".Equals(request.GrantType, StringComparison.Ordinal))
            {
                return AccessTokenSpecificRequestError.InvalidGrantType;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the first specific error for the given request, if no errors are present, the code is valid.
        /// </summary>
        /// <param name="request">The incomming access token request.</param>
        /// <returns>Returns a new <see cref="AccessTokenSpecificRequestError"/> object that specifies the first thing wrong with the given parameters, returns null if none exist.</returns>
        protected virtual AccessTokenSpecificRequestError? GetRequestError(IPasswordCredentialsAccessTokenRequest request)
        {
            if (request == null)
            {
                return AccessTokenSpecificRequestError.NullRequest;
            }
            else if (!"password".Equals(request.GrantType, StringComparison.Ordinal))
            {
                return AccessTokenSpecificRequestError.InvalidGrantType;
            }
            else if (string.IsNullOrEmpty(request.ClientId))
            {
                return AccessTokenSpecificRequestError.NullClientId;
            }
            else if (string.IsNullOrEmpty(request.ClientSecret))
            {
                return AccessTokenSpecificRequestError.NullClientSecret;
            }
            else if (request.User == null)
            {
                return AccessTokenSpecificRequestError.NullUser;
            }
            else if (!request.User.IsValidated)
            {
                return AccessTokenSpecificRequestError.InvalidGrant;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new <see cref="IUnsuccessfulAuthorizationCodeResponse" /> object that represents the given error code with the given exception.
        /// </summary>
        /// <param name="specificError">The specific error.</param>
        /// <param name="redirect">The redirect.</param>
        /// <param name="request">The request.</param>
        /// <param name="innerException">The exception that caused the error to occur.</param>
        /// <returns>
        /// Returns a new <see cref="IUnsuccessfulAuthorizationCodeResponse" /> object that represents a valid OAuth 2.0 Authorization Code Error resposne.
        /// </returns>
        private IUnsuccessfulAuthorizationCodeResponse CreateAuthorizationCodeError(AuthorizationCodeRequestSpecificErrorType specificError, Uri redirect, IAuthorizationCodeRequest request, IClient client = null, IUser user = null, IEnumerable<IScope> scopes = null, Exception innerException = null)
        {
            return AuthorizationCodeResponseFactory.CreateError(
                errorCode: specificError,
                request: new ProcessedAuthorizationCodeRequest
                (
                    originalRequest: request,
                    client: client,
                    user: user,
                    scopes: scopes
                ),
                errorDescription: AuthorizationCodeErrorDescriptionProvider.GetDescription(specificError) ?? specificError.GetDescription(),
                errorUri: null,
                redirectUri: redirect,
                innerException: innerException);
        }

        /// <summary>
        /// Creates a new <see cref="IUnsuccessfulAuthorizationCodeResponse" /> object that represents the given error code with the given exception.
        /// </summary>
        /// <param name="specificError">The specific error.</param>
        /// <param name="redirect">The redirect.</param>
        /// <param name="request">The request.</param>
        /// <param name="innerException">The exception that caused the error to occur.</param>
        /// <returns>
        /// Returns a new <see cref="IUnsuccessfulAuthorizationCodeResponse" /> object that represents a valid OAuth 2.0 Authorization Code Error resposne.
        /// </returns>
        protected virtual IUnsuccessfulAuthorizationCodeResponse CreateAuthorizationCodeError(AuthorizationCodeRequestSpecificErrorType specificError, Uri redirect, IProcessedAuthorizationCodeRequest request, Exception innerException = null)
        {
            return AuthorizationCodeResponseFactory.CreateError(
                errorCode: specificError,
                request: request,
                errorDescription: AuthorizationCodeErrorDescriptionProvider.GetDescription(specificError) ?? specificError.GetDescription(),
                errorUri: null,
                redirectUri: redirect,
                innerException: innerException);
        }

        /// <summary>
        /// Creates a new <see cref="IUnsuccessfulAccessTokenResponse"/> object that represents the given error code, for the given client and exception.
        /// </summary>
        /// <param name="errorCode">The <see cref="AccessTokenRequestError"/> object that specifies what basic error occurred.</param>
        /// <param name="client">The client that the error occured to.</param>
        /// <param name="exception">The exception that caused the error to occur.</param>
        /// <returns>Returns a new <see cref="IUnsuccessfulAccessTokenResponse"/> object that represents a valid OAuth 2.0 Access Token Error response (http://tools.ietf.org/html/rfc6749#section-5.2).</returns>
        private IUnsuccessfulAccessTokenResponse CreateAccessTokenError(AccessTokenSpecificRequestError errorCode, IClient client = null, Exception exception = null)
        {
            return AccessTokenResponseFactory.CreateError(
                errorCode,
                AccessTokenErrorDescriptionProvider.GetDescription(errorCode),
                AccessTokenErrorUriProvider(errorCode.GetSubgroup<AccessTokenRequestError>(), client),
                exception);
        }

        /// <summary>
        /// Gets the first specific error for the given request, if no errors are present, the code is valid.
        /// </summary>
        /// <param name="code">The authorization code to examine.</param>
        /// <param name="request">The incomming access token request.</param>
        /// <param name="client">The client that is requesting the access toking.</param>
        /// <returns>Returns a new <see cref="AccessTokenSpecificRequestError"/> object that specifies the first thing wrong with the given parameters, returns null if none exist.</returns>
        protected virtual AccessTokenSpecificRequestError? GetAuthorizationCodeError(IAuthorizationCode code, IAuthorizationCodeGrantAccessTokenRequest request, IClient client)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (code == null)
            {
                return AccessTokenSpecificRequestError.MissingCode;
            }
            else if (!code.Client.Equals(client))
            {
                return AccessTokenSpecificRequestError.WrongClient;
            }
            else if (!code.IsValid())
            {
                return AccessTokenSpecificRequestError.CodeNoLongerValid;
            }
            else if (!code.MatchesValue(request.AuthorizationCode))
            {
                return AccessTokenSpecificRequestError.InvalidGrant;
            }
            else if (Uri.Compare(code.RedirectUri, request.RedirectUri, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.Ordinal) != 0)
            {
                return AccessTokenSpecificRequestError.InvalidRedirect;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines if the given <see cref="IAuthorizationCode"/>, <see cref="IAuthorizationCodeGrantAccessTokenRequest"/> and <see cref="IClient"/> are valid.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="request"></param>
        /// <param name="client"></param>
        protected virtual bool IsValidAuthorizationCode(IAuthorizationCode code, IAuthorizationCodeGrantAccessTokenRequest request, IClient client)
        {
            if (request == null) throw new ArgumentNullException("request");
            return code != null && code.Client.Equals(client) && code.IsValid() && code.MatchesValue(request.AuthorizationCode) && Uri.Compare(code.RedirectUri, request.RedirectUri, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// Determines if the given <see cref="IAccessTokenRequest"/> and <see cref="IClient"/> are valid.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="client"></param>
        protected virtual AccessTokenSpecificRequestError? GetClientError(IAccessTokenRequest request, IClient client)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (client == null)
            {
                return AccessTokenSpecificRequestError.MissingClient;
            }
            else if (!client.MatchesSecret(request.ClientSecret))
            {
                return AccessTokenSpecificRequestError.UnauthorizedClient;
            }
            return null;
        }

        #endregion

        #region IDisposable Implementation
        /// <summary>
        /// Finalizes an instance of the <see cref="OAuthProvider" /> class.
        /// </summary>
        ~OAuthProvider()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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
        #endregion
    }
}
