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

        // The Authorization Provider must have access to an Access Token and Authorization Code repository so that it can store and retrieve access tokens.

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
        IAccessTokenResponse RequestAccessToken(IAccessTokenRequest request);

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
