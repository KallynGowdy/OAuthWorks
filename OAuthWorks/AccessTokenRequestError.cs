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
using System.Runtime.Serialization;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a list of values of the possible error codes that can be sent back to a client after a failed attempt.
    /// </summary>
    [DataContract]
    public enum AccessTokenRequestError
    {
        /// <summary>
        /// Defines that the client made an invalid request. That the request is missing a required parameter, includes an
        /// unsupported parameter value (other than grant type), repeats a parameter, includes multiple credentials, 
        /// utilizes more than one mechanism for authenticating the client, or is otherwise malformed. (RFC 6749 Section 5.1)
        /// </summary>
        [EnumMember(Value="invalid_request")]
        InvalidRequest,

        /// <summary>
        /// Defines that client authentication failed. 
        /// Client authentication failed (e.g., unknown client, no
        /// client authentication included, or unsupported
        /// authentication method).  The authorization server MAY
        /// return an HTTP 401 (Unauthorized) status code to indicate
        /// which HTTP authentication schemes are supported.  If the
        /// client attempted to authenticate via the "Authorization"
        /// request header field, the authorization server MUST
        /// respond with an HTTP 401 (Unauthorized) status code and
        /// include the "WWW-Authenticate" response header field
        /// matching the authentication scheme used by the client. (RFC 6749 Section 5.1)
        /// </summary>
        [EnumMember(Value="invalid_client")]
        InvalidClient,

        /// <summary>
        /// Defines that the provided authorization grant (authorization code, resource owner credentials) or refresh refreshToken is invalid,
        /// expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client. (RFC 6749 Section 5.1)
        /// </summary>
        [EnumMember(Value="invalid_grant")]
        InvalidGrant,

        /// <summary>
        /// Defines that the authenticated client is not authorized to use this authorization grant type. (RFC 6749 Section 5.1)
        /// </summary>
        [EnumMember(Value="unauthorized_client")]
        UnauthorizedClient,

        /// <summary>
        /// Defines that the authorization grant type is not supported by the authorization server. (RFC 6749 Section 5.1)
        /// </summary>
        [EnumMember(Value="unsupported_grant_type")]
        UnsupportedGrantType,

        /// <summary>
        /// Defines that the requested scope is invalid, unknown, malformed, or exceeds the scope granted by the resource owner.
        /// </summary>
        [EnumMember(Value="invalid_scope")]
        InvalidScope,

        /// <summary>
        /// Defines that the server encountered an unexpected error and the request could not be completed successfully. Not part of the OAuth 2.0 Spec.
        /// </summary>
        [EnumMember(Value="server_error")]
        ServerError
    }
}
