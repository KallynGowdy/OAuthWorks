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

using System.ComponentModel;
using PortableOAuthWorks.DataAnnotations;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a list of values that represent the different specific errors that can occur in an authorization code request.
    /// </summary>
    public enum AuthorizationCodeRequestSpecificErrorType
    {
        /// <summary>
        /// Defines that the request requested a missing or unknown scope from the server.
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.InvalidScope)]
        [Description("One or more of the requested scopes was missing or not found.")]
        MissingOrUnknownScope,

        /// <summary>
        /// Defines that the request contained a redirect URI that was invalid in some way, shape or form. (Not an authorized URI, Malformed, etc.)
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.InvalidRequest)]
        [Description("The given redirect URI was invalid. (Not authorized, Malformed, etc.)")]
        InvalidRedirectUri,

        /// <summary>
        /// Defines that the request sent to the provider (<see cref="OAuthProvider"/>) was null and no values could be retrieved from it.
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.InvalidRequest)]
        [Description("The given request could not be parsed correctly.")]
        NullRequest,

        /// <summary>
        /// Defines that the redirect URI contained in the request was null/missing or was malformed.
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.InvalidRequest)]
        [Description("The given redirect was missing or malformed.")]
        NullRedirect,

        /// <summary>
        /// Defines that the client id contained in the request was null/missing or an empty string.
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.InvalidRequest)]
        [Description("The given client ID was missing or was an empty string.")]
        NullClientId,

        /// <summary>
        /// Defines that the client secret contained in the request was null/missing or an empty string.
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.InvalidRequest)]
        [Description("The given client secret was missing or was an empty string")]
        NullClientSecret,

        /// <summary>
        /// Defines that the scope contained in the request was null/missing or an empty string.
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.InvalidRequest)]
        [Description("The given scope was missing or was an empty string.")]
        NullScope,

        /// <summary>
        /// Defines that the server encountered an error that was not related to the request.
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.ServerError)]
        [Description("The server encountered an error that was not directly related to the request.")]
        ServerError,

        /// <summary>
        /// Defines that one or more scopes was not authorized by the user for use by the client.
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.AccessDenied)]
        [Description("One or more scopes was not authorized by the user for use by the client.")]
        UserUnauthorizedScopes,

        /// <summary>
        /// Defines that no client exists with the Client ID contained in the request.
        /// </summary>
        [EnumSubgroup(AuthorizationCodeRequestErrorType.UnauthorizedClient)]
        [Description("The client with the given ID could not be found.")]
        MissingClient
    }
}