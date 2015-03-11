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


using PortableOAuthWorks.DataAnnotations;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a list of values that specify the specific problem with a request.
    /// </summary>
    public enum AccessTokenSpecificRequestError
    {
        /// <summary>
        /// Defines that the client could not be found and was therefore missing.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidClient)]
        [Description("The client with the given ID could not be found.")]
        MissingClient,

        /// <summary>
        /// Defines that the client was unauthorized because the given secret was not valid.
        /// </summary>
        [Description("The given client secret ('client_secret' parameter) valid/did not match.")]
        [EnumSubgroup(AccessTokenRequestError.UnauthorizedClient)]
        UnauthorizedClient,

        /// <summary>
        /// Defines that the given authorization code grant was invalid.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidGrant)]
        [Description("The given authorization code grant was invalid.")]
        InvalidGrant,

        /// <summary>
        /// Defines that the authorization_code value was missing from the request.
        /// </summary>
        [Description("The authorization code ('authorization_code' parameter) value was missing from the request.")]
        [EnumSubgroup(AccessTokenRequestError.InvalidRequest)]
        MissingCode,

        /// <summary>
        /// Defines that a client with the wrong ID is trying to request an access token.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.UnauthorizedClient)]
        [Description("The authorization code/refresh token was issued to a different client and therefore cannot be used by your client.")]
        WrongClient,

        /// <summary>
        /// Defines that the given authorization code is no longer valid. (time expiration, user revoked, etc.)
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidGrant)]
        [Description("The issued authorization code has been invalidated in some way, either by expiration or by user revocation.")]
        CodeNoLongerValid,

        /// <summary>
        /// Defines that the given redirect URI was not the same redirect URI that was given in the authorization code request.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidGrant)]
        [Description("The given redirect URI (redirect_uri parameter) did not contain the same value that was given in the authorization code request.")]
        InvalidRedirect,

        /// <summary>
        /// Defines that the server encountered an error that has no direct/apparent relation to the given request.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.ServerError)]
        [Description("The server encountered and error that has no direct/apparent relation to the given request.")]
        ServerError,

        /// <summary>
        /// Defines that the access token request was not present or could not be parsed correctly.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidRequest)]
        [Description("The request either did not contain any request body or could not be parsed correctly.")]
        NullRequest,

        /// <summary>
        /// Defines that the 'client_id' parameter was either missing, named incorrectly, or equal to an empty string.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidClient)]
        [Description("The 'client_id' parameter was either missing, named incorrectly, or equal to an empty string.")]
        NullClientId,

        /// <summary>
        /// Defines that the 'client_secret' parameter was either missing, named incorrectly, or equal to an empty string.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidClient)]
        [Description("The 'client_secret' parameter was either missing, named incorrectly, or equal to an empty string.")]
        NullClientSecret,

        /// <summary>
        /// Defines that the 'authorization_code' parameter was either missing, named incorrectly, or equal to an empty string.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidRequest)]
        [Description("The 'authorization_code' parameter was either missing, named incorrectly, or equal to an empty string.")]
        NullAuthorizationCode,

        /// <summary>
        /// Defines that the 'grant_type' parameter was either missing, named incorrectly, or equal to an empty string.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.UnsupportedGrantType)]
        [Description("The 'grant_type' parameter was either missing, named incorrectly, or equal to an empty string.")]
        NullGrantType,

        /// <summary>
        /// Defines that the 'grant_type' parameter contains a value that is not supported.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.UnsupportedGrantType)]
        [Description("The given 'grant_type' is not supported.")]
        InvalidGrantType,

        /// <summary>
        /// Defines that the 'refresh_token' parameter was either missing, named incorrectly, or equal to an empty string.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidRequest)]
        [Description("The 'refresh_token' parameter was either missing, named incorrectly, or equal to an empty string.")]
        NullRefreshToken,

        /// <summary>
        /// Defines that the given refresh token is no longer valid. (time expiration, user revoked, etc.)
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidGrant)]
        [Description("The granted refresh token is not longer valid, it has either expired or been revoked by the user.")]
        TokenNoLongerValid,

        /// <summary>
        /// Defines that the user in the request was null.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidRequest)]
        [Description("The 'user' parameter in the request was null. (Server Error)")]
        NullUser,

        /// <summary>
        /// Defines that the given scope(s) is invalid. (Malformed, not found, etc.)
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidScope)]
        [Description("One or more of the given scopes is invalid. (Malformed, not found, etc.)")]
        InvalidScope,

        /// <summary>
        /// Defines that the given redirect URI was missing, named incorrectly, or malformed.
        /// </summary>
        [EnumSubgroup(AccessTokenRequestError.InvalidRequest)]
        [Description("The 'redirect_uri' parameter was either missing, named incorrectly, or malformed.")]
        NullRedirectUri
    }
}