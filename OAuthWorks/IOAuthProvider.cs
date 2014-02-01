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


using System;
using System.Collections;
using System.Collections.Generic;
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
    public interface IOAuthProvider
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

        // 1).  An authorization code request comes from the client.
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
        /// Gets the scopes that are requested by the client based on the given request.
        /// </summary>
        /// <param name="request">The <see cref="OAuthWorks.AuthorizationCode"/> object that represents the request from the client.</param>
        /// <returns>Returns a list of <see cref="OAuthWorks.IScope"/> objects.</returns>
        IEnumerable<IScope> GetRequestedScopes(IAuthorizationCodeRequest request);

        /// <summary>
        /// Initiates the Authorization Code flow based on the given request and returns a response that defines what response to send back to the user agent.
        /// Be sure to authenticate the user and request consent before calling this. THIS METHOD ASSUMES THAT USER CONSENT WAS GIVEN.
        /// </summary>
        /// <param name="request">The request that contains the values that were sent by the client.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAuthorizationCodeResponse"/> object that determines what values to put in the outgoing response.</returns>
        IAuthorizationCodeResponse InitiateAuthorizationCodeFlow(IAuthorizationCodeRequest request);

        /// <summary>
        /// Requests an access token from the authorization server based on the given request.
        /// </summary>
        /// <param name="request">The request that contains the required values.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessTokenResponse"/> object that determines what values to put in the outgoing response.</returns>
        IAccessTokenResponse RequestAccessToken(IAccessTokenRequest request, IUser currentUser);

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
        /// Gets the <see cref="OAuthWorks.IOAuthProviderDefintion"/> that contains information on the different endpoints provided by this
        /// IOAuthProvider.
        /// </summary>
        IOAuthProviderDefinition Definition
        {
            get;
        }
    }
}