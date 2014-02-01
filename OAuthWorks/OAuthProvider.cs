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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    public class OAuthProvider : IOAuthProvider
    {
        /// <summary>
        /// Gets the repository of scopes that this provider has access to.
        /// </summary>
        public IScopeRepository<IScope> ScopeRepository
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the repository that contains <see cref="OAuthWorks.IClient"/> objects.
        /// </summary>
        public IRepository<string, IClient> ClientRepository
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the factory used to create new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> objects.
        /// </summary>
        public IAuthorizationCodeResponseFactory<IAuthorizationCodeResponse> AuthorizationCodeResponseFactory
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
        /// Gets or sets the factory that creates <see cref="OAuthWorks.IAccessTokenResponse"/> objects for this provider.
        /// </summary>
        public IAccessTokenResponseFactory<IAccessTokenResponse> AccessTokenResponseFactory
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
        /// Gets or sets the formatter for scopes. That is, a function that, given a list of scopes, returns a string representing those scopes.
        /// </summary>
        public Func<IEnumerable<IScope>, string> ScopeFormatter
        {
            get;
            set;
        }

        private Func<string, IEnumerable<IScope>> scopeParser;

        /// <summary>
        /// Gets or sets the parser for the scopes. That is, a function that, given a string, returns a list of scopes representing that string.
        /// </summary>
        public Func<string, IEnumerable<IScope>> ScopeParser
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
        public Func<AccessTokenRequestError, IClient, string> AccessTokenErrorUriProvider
        {
            get;
            set;
        }

        public IEnumerable<IScope> GetRequestedScopes(IAuthorizationCodeRequest request)
        {
            return ScopeParser(request.Scope);
        }

        public IAuthorizationCodeResponse InitiateAuthorizationCodeFlow(IAuthorizationCodeRequest request)
        {
            IClient client = ClientRepository.GetById(request.ClientId);
            if (client != null && client.MatchesSecret(request.ClientSecret))
            {
                if (client.ValidRedirectUri(request.RedirectUri))
                {
                    IEnumerable<IScope> scopes = GetRequestedScopes(request);
                    string authoizationCodeValue;
                    IAuthorizationCode authCode = AuthorizationCodeFactory.Create(out authoizationCodeValue, scopes);

                    //put the authorization code in the repository
                    AuthorizationCodeRepository.Add(authCode);

                    //return a successful response
                    return AuthorizationCodeResponseFactory.Create(authoizationCodeValue, request.State);
                }
                else
                {
                    //Invalid redirect
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Requests an access token from the server with the request.
        /// </summary>
        /// <param name="request">The incoming request for an access token.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object that represents what to </returns>
        public IAccessTokenResponse RequestAccessToken(IAccessTokenRequest request, IUser currentUser)
        {
            IClient client = ClientRepository.GetById(request.ClientId);
            if (client != null && client.MatchesSecret(request.ClientSecret))
            {
                IAuthorizationCode code = AuthorizationCodeRepository.GetByValue(request.AuthorizationCode);
                if (code != null && !code.Expired && code.MatchesCode(request.AuthorizationCode))
                {
                    //TODO: Add Redirect Uri Check

                    string accessTokenValue;
                    string refreshTokenValue = null;
                    //Authorized!
                    IAccessToken accessToken = AccessTokenFactory.Create(out accessTokenValue, client, currentUser, code.Scopes);
                    IRefreshToken refreshToken = null;
                    if (RefreshTokenFactory != null)
                    {
                        refreshToken = RefreshTokenFactory.Create(out refreshTokenValue, client, currentUser, code.Scopes);
                    }
                    //store refresh and access tokens
                    AccessTokenRepository.Add(accessToken);
                    

                    return AccessTokenResponseFactory.Create(accessTokenValue, refreshTokenValue, accessToken.TokenType, ScopeFormatter(accessToken.Scopes), accessToken.ExpirationDateUtc);
                }
                else
                {
                    //Invalid authorization code grant
                    return AccessTokenResponseFactory.CreateError(AccessTokenRequestError.InvalidGrant, AccessTokenErrorDescriptionProvider(AccessTokenRequestError.InvalidGrant, client), AccessTokenErrorUriProvider(AccessTokenRequestError.InvalidGrant, client));
                }
            }
            else
            {
                //Unauthorized
                return AccessTokenResponseFactory.CreateError(AccessTokenRequestError.InvalidClient, AccessTokenErrorDescriptionProvider(AccessTokenRequestError.InvalidClient, client), AccessTokenErrorUriProvider(AccessTokenRequestError.InvalidClient, client));
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
                return ScopeRepository.GetByToken(token).Any(a => a.Equals(scope));
            }
            return false;
        }

        public IOAuthProviderDefinition Definition
        {
            get { throw new NotImplementedException(); }
        }

        public bool DistributeRefreshTokens
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerable<IScope> IOAuthProvider.GetRequestedScopes(IAuthorizationCodeRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
