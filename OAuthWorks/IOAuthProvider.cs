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


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for an OAuth 2.0 Provider.
    /// </summary>
    /// <remarks>
    /// The OAuth Providers that are specified by this interface are Authentication-less. 
    /// This means that all authentication is handled outside of the provider by a seperate, not-related, implementation.
    /// This allows for state-less authorization, which is more flexible than requiring an AuthorizationProvider to be given.
    /// There are helper functions for determining whether a client requires authorization by the user or not.
    /// In the case that the Resource Owner Password Credentials flow is allowed by the provider, 
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Auth")]
    [ContractClass(typeof(IOAuthProviderContract))]
    public interface IOAuthProvider : IDisposable
    {
        //The provider needs to provide support for the different OAuth flows
        //There are four(4) different OAuth 2.0 flows that are defined by the spec.
        //
        //1). Authorization Code
        //2). Implicit
        //3). Resource Owner Password Credentials
        //4). Client Credentials

        //The Provider also needs to provide simple tools for discovery such as:
        //
        // -- Authorization Endpoint
        // -- Token Endpoint

        //We will provide all of this functionality without touching implementation-specific Http services, such as ASP.net MVC or Web API.
        //This is achieved through the use of wrapper interfaces.  The flows use a message request/response model that can easily be adapted to WebRequest/Response objects.

        // The Authorization Provider should not implement Authentication.  The interfaces specifically avoid Authentication to allow for simpler implementation.
        // Because the providers don't provide authentication mechanizims, all authentication and user consent must be provided outside of the functions.

        //From RFC 6749:
        //
        //  The authorization code grant type is used to obtain both access
        //  tokens and refresh tokens and is optimized for confidential clients.
        //  Since this is a redirection-based flow, the client must be capable of
        //  interacting with the resource owner's user-agent (typically a web
        //  browser) and capable of receiving incoming requests (via redirection)
        //  from the authorization server.
        //
        //      +----------+
        //      | Resource |
        //      |   Owner  |
        //      |          |
        //      +----------+
        //           ^
        //           |
        //          (B)
        //      +----|-----+          Client Identifier      +---------------+
        //      |         -+----(A)-- & Redirection URI ---->|               |
        //      |  User-   |                                 | Authorization |
        //      |  Agent  -+----(B)-- User authenticates --->|     Server    |
        //      |          |                                 |               |
        //      |         -+----(C)-- Authorization Code ---<|               |
        //      +-|----|---+                                 +---------------+
        //        |    |                                         ^      v
        //       (A)  (C)                                        |      |
        //        |    |                                         |      |
        //        ^    v                                         |      |
        //      +---------+                                      |      |
        //      |         |>---(D)-- Authorization Code ---------'      |
        //      |  Client |          & Redirection URI                  |
        //      |         |                                             |
        //      |         |<---(E)----- Access Token -------------------'
        //      +---------+       (w/ Optional Refresh Token)
        //
        //  Note: The lines illustrating steps (A), (B), and (C) are broken into
        //  two parts as they pass through the user-agent.
        //
        //
        //

        // ------------------------
        // Authorization Code Flow
        // ------------------------
        //                                                                                                 +- yes? --------
        //      +-----------+                   +-----------------+                                        |
        //      |           |                   |                 |>--- Is Authorization Already Given? ---+
        //      |  Request  |>----------------->|    Web Front    |                                        |
        //      |           |                   |       End       |                                        |
        //      +-----------+                   |                 |                                        +- no? --------
        //                                      +-----------------+
        //

        // 1).  An authorization code request comes from the client. By redirecting the user to the specified endpoint.
        //
        // 2).  The web framework determines if the resource owner
        //      has already given consent or if consent is required.
        //
        // 3).  If consent is required, the front end displays a consent screen.
        //      Otherwise a new authorization code is generated by the authorization provider
        //      and the user agent is redirected back to the client with the code.

        // The Authorization Provider must have access to an Access Token and Authorization Code repository so that it can store and retrieve access tokens.

        /// <summary>
        /// Gets whether the OAuthProvider distributes refresh tokens.
        /// </summary>
        bool DistributeRefreshTokens
        {
            get;
        }

        /// <summary>
        /// Gets the scopes that are requested by the client based on the scopes from the given request.
        /// </summary>
        /// <param name="scopes">The string of scopes that the client requested.</param>
        /// <returns>Returns a list of <see cref="OAuthWorks.IScope"/> objects.</returns>
        IEnumerable<IScope> GetRequestedScopes(string scopes);

        /// <summary>
        /// Initiates the Authorization Code flow based on the given request and returns a response that defines what response to send back to the user agent.
        /// Be sure to authenticate the user and request consent before calling this. THIS METHOD ASSUMES THAT USER CONSENT WAS GIVEN.
        /// </summary>
        /// <param name="request">The request that contains the values that were sent by the client.</param>
        /// <param name="user">The user that the request is for.</param>
        /// <exception cref="OAuthWorks.AuthorizationCodeResponseExceptionBase">Thrown if an exception occurs inside this method or if the given request was invalid in some way.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the given request is null.</exception>
        /// <returns>Returns a new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> object that determines what values to put in the outgoing response.</returns>
        IAuthorizationCodeResponse RequestAuthorizationCode(IAuthorizationCodeRequest request, IUser user);

        /// <summary>
        /// Requests an access refreshToken from the authorization server based on the given request using the Authorization Code Grant flow (Section 4.1 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.1).
        /// </summary>
        /// <remarks>
        /// This method is used to distribute access tokens according to the Authorization Code Grant flow (Section 4.1 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.1).
        /// Classes implementing this method should be able to handle any input and return a valid output without throwing an exception.
        /// Not only would that cause a performance improvement for handling invalid requests, but it makes it easier to pass on to the client.
        /// Make sure to save/commit any changes recorded by the unit of work object (DataContext) after calling this method so that it is saved to the datastore.
        /// </remarks>
        /// <param name="request">The request that contains the required values.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object that determines what values to put in the outgoing response.</returns>
        IAccessTokenResponse RequestAccessToken(IAuthorizationCodeGrantAccessTokenRequest request);

        /// <summary>
        /// Requests an access refreshToken from the authorization server based on the given request using the Resource Owner Password Credentials flow. (Section 4.3 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.3).
        /// </summary>
        /// <param name="request">The request that contains the required values.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object that determines what values to put in the outgoing response.</returns>
        IAccessTokenResponse RequestAccessToken(IPasswordCredentialsAccessTokenRequest request);

        /// <summary>
        /// Requests a new access refreshToken from the authorizaiton server based on the given request.
        /// </summary>
        /// <param name="request">The request that contains the required values for refreshing an access refreshToken.</param>
        /// <remarks>
        /// This method distributes new access tokens according to the OAuth 2.0 refresh token flow. (Section 6 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-6)
        /// Classes implementing this method should be able to handle any input and return a valid (non-null) output without throwing an exception.
        /// Make sure to save/commit any changes recorded by the unit or work object (DataContext) after callin this method so that it is saved to the datastore.
        /// </remarks>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object that determines what values to put in the outgoing response.</returns>
        IAccessTokenResponse RefreshAccessToken(ITokenRefreshRequest request);

        /// <summary>
        /// Revokes access to the given user's account from the given client.
        /// </summary>
        /// <param name="user">The user whose account should not be accessible by the client.</param>
        /// <param name="client">The client to revoke access from.</param>
        void RevokeAccess(IUser user, IClient client);

        /// <summary>
        /// Determines if the given client has been given access to the given scope by the given user.
        /// </summary>
        /// <param name="user">The User that is currently logged in.</param>
        /// <param name="client">The client to determine access for.</param>
        /// <param name="scope">The scope that the client wants access to.</param>
        /// <returns>Returns true if the client has access to the given users resources restricted by the given scope, otherwise false.</returns>
        bool HasAccess(IUser user, IClient client, IScope scope);


        /// <summary>
        /// Validates the given authorization values (access token) and returns a result representing whether or not it was successful and what was wrong with it.
        /// </summary>
        /// <param name="request">An object that contains values that were provided by the client to be used for authorization.</param>
        /// <returns>Returns a new <see cref="IAuthorizationResult"/>An object that contains values provided by the client for authorization.</returns>
        IAuthorizationResult ValidateAuthorization(IAuthorizationRequest request);
    }

    [ContractClassFor(typeof(IOAuthProvider))]
    internal abstract class IOAuthProviderContract : IOAuthProvider
    {

        bool IOAuthProvider.DistributeRefreshTokens
        {
            get { return default(bool); }
        }

        IAuthorizationCodeResponse IOAuthProvider.RequestAuthorizationCode(IAuthorizationCodeRequest request, IUser user)
        {
            Contract.Requires(user != null);
            Contract.Ensures(Contract.Result<IAuthorizationCodeResponse>() != null);
            return default(IAuthorizationCodeResponse);
        }

        IAccessTokenResponse IOAuthProvider.RequestAccessToken(IAuthorizationCodeGrantAccessTokenRequest request)
        {
            Contract.Ensures(Contract.Result<IAccessTokenResponse>() != null);
            return default(IAccessTokenResponse);
        }

        IAccessTokenResponse IOAuthProvider.RefreshAccessToken(ITokenRefreshRequest request)
        {
            Contract.Ensures(Contract.Result<IAccessTokenResponse>() != null);
            return default(IAccessTokenResponse);
        }

        void IOAuthProvider.RevokeAccess(IUser user, IClient client)
        {
            Contract.Requires(user != null);
            Contract.Requires(client != null);
        }

        bool IOAuthProvider.HasAccess(IUser user, IClient client, IScope scope)
        {
            Contract.Requires(user != null);
            Contract.Requires(client != null);
            Contract.Requires(scope != null);
            return default(bool);
        }

        void IDisposable.Dispose()
        {
        }

        IAccessTokenResponse IOAuthProvider.RequestAccessToken(IPasswordCredentialsAccessTokenRequest request)
        {
            return default(IAccessTokenResponse);
        }

        IEnumerable<IScope> IOAuthProvider.GetRequestedScopes(string scopes)
        {
            Contract.Ensures(Contract.Result<IEnumerable<IScope>>() != null);
            return default(IEnumerable<IScope>);
        }

        IAuthorizationResult IOAuthProvider.ValidateAuthorization(IAuthorizationRequest request)
        {
            Contract.Requires(request != null);
            Contract.Ensures(Contract.Result<IAuthorizationResult>() != null);
            return default(IAuthorizationResult);
        }
    }
}
