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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface that contains values and contextual information that is sent in the Authorization Request 
    /// of the Authorization Code flow in the OAuth 2.0 specification.
    /// (RFC 6749, Section 4.1, http://tools.ietf.org/html/rfc6749#section-4.1)
    /// </summary>
    /// <remarks>
    /// This interface requires all of the values that must be passed to a <see cref="OAuthProvider"/> when the client requests an Authorization Code as a part
    /// of the Authorization Code Flow (RFC 6749, Section 4.1, http://tools.ietf.org/html/rfc6749#section-4.1). Notice that the Client ID is required but the
    /// Client Secret is not, this is to prevent it from being stolen by the user as the authorization request must be issued from a front-end framework. Once the
    /// Authorization Code is granted and sent back to the client's server, then it can be exchanged for an Access Token which requires the Client Secret.
    /// </remarks>
    public interface IAuthorizationCodeRequest
    {
        // As per the spec, the Authorization Code Request MUST contain the following values:
        //
        // -- response_type,    Enum,   MUST be set to 'code'.
        // -- client_id,        string, The identifier of the client. (RFC 6749, Section 2.2, http://tools.ietf.org/html/rfc6749#section-2.2)
        //
        // Other values are optional:
        //
        // -- redirect_uri,     string, The URI that the user agent should be redirected to with the response. (RFC 6749, Section 3.1.2, http://tools.ietf.org/html/rfc6749#section-3.1.2)
        // -- scope,            string, The scope of the access that is requested by the client. (RFC 6749, Section 3.3, http://tools.ietf.org/html/rfc6749#section-3.3)
        // -- state,            string, An opaque value used by the client to maintain
        //                              state between the request and callback.  The authorization
        //                              server includes this value when redirecting the user-agent back
        //                              to the client. (RFC 6749, Section 10.12, http://tools.ietf.org/html/rfc6749#section-10.12)

        /// <summary>
        /// Gets the response_type that defines what the OAuth Provider should return. Required to be 'Code'.
        /// </summary>
        AuthorizationCodeResponseType ResponseType
        {
            get;
        }

        /// <summary>
        /// Gets the Id of the client. This value is required to be non-null and not empty.
        /// </summary>
        string ClientId
        {
            get;
        }

        /// <summary>
        /// Gets the redirect Uri that defines where the OAuth Provider should redirect the user agent after processing the request.
        /// This value can be null.
        /// </summary>
        Uri RedirectUri
        {
            get;
        }

        /// <summary>
        /// Gets the scope of the permissions that are requested by the client. This value can be null.
        /// </summary>
        string Scope
        {
            get;
        }

        /// <summary>
        /// Gets the state that was sent by the client as a possible form of CSRF. This value can be null unless required by specific implementation.
        /// </summary>
        string State
        {
            get;
        }
    }
}
