using OAuthWorks.Factories;
using OAuthWorks.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    public class OAuthProvider<TScope, TClient, TAuthorizationCode, TAccessToken, TUser> : IOAuthProvider<TScope, TClient, TAuthorizationCode, TAccessToken>
        where TScope : IScope
        where TClient : IClient
        where TAuthorizationCode : IAuthorizationCode
        where TAccessToken : IAccessToken
    {
        /// <summary>
        /// Gets the repository of scopes that this provider has access to.
        /// </summary>
        public IScopeRepository<TScope> ScopeRepository
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
        /// Gets the repository of Authorization Code objects that this provider has access to.
        /// </summary>
        public IAuthorizationCodeRepository<TAuthorizationCode> AuthorizationCodeRepository
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the factory that creates Authorization Code objects for this provider.
        /// </summary>
        public IFactory<TAuthorizationCode> AuthorizationCodeFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the repository of Access Token objects that this provider has access to.
        /// </summary>
        public IAccessTokenRepository<TAccessToken> AccessTokenRepository
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the factory that creates Access Token objects for this provider.
        /// </summary>
        public IAccessTokenFactory<TAccessToken> AccessTokenFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the facroty that creates <see cref="OAuthWorks.IAccessTokenResponse"/> objects for this provider.
        /// </summary>
        public IAccessTokenResponseFactory<IAccessTokenResponse> AccessTokenResponseFactory
        {
            get;
            private set;
        }

        public IEnumerable<TScope> GetRequestedScopes(IAuthorizationCodeRequest request)
        {
            return request.Scope.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Join(ScopeRepository.GetAllScopes(), s => s, s => s.Id, (s, scope) => scope);
        }

        public IAuthorizationCodeResponse InitiateAuthorizationCodeFlow(IAuthorizationCodeRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Requests an access token from the server with the request.
        /// </summary>
        /// <param name="request">The incoming request for an access token.</param>
        /// <returns></returns>
        public IAccessTokenResponse RequestAccessToken(IAccessTokenRequest request)
        {
            IClient client = ClientRepository.GetById(request.ClientId);
            if (client != null && client.MatchesSecret(request.ClientSecret))
            {
                Tuple<string, string> codeValues = IAuthorizationCode.Util.GetIdAndAuthorizationCode(request.AuthorizationCode);
                if (codeValues != null)
                {
                    IAuthorizationCode code = AuthorizationCodeRepository.GetById(codeValues.Item1);
                    if (code != null && !code.Expired && code.MatchesCode(codeValues.Item2))
                    {
                        //Authorized!
                        TAccessToken token = AccessTokenFactory.Get(client, code.Scopes);

                    }
                }
            }
            //Unauthorized
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

            foreach (TAccessToken token in AccessTokenRepository.GetByUser(user).Where(t => t.Client.Equals(client)))
            {
                if (!token.Revoked)
                {
                    token.Revoke();
                }
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
            IAccessToken token = AccessTokenRepository.GetByUser(user).SingleOrDefault(t => t.Client.Equals(client));
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
    }
}
