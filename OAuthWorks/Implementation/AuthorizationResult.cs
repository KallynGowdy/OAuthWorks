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
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="IAuthorizationResult"/>
    /// </summary>
    public class AuthorizationResult : IAuthorizationResult
    {
        /// <summary>
        /// Gets a new <see cref="AuthorizationResult"/> value that represents successful authorization.
        /// </summary>
        /// <returns></returns>
        public static AuthorizationResult Success
        {
            get
            {
                return new AuthorizationResult(true);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        /// <summary>
        /// Defines a static class that provides different <see cref="AuthorizationResult"/> objects that represent different authorization
        /// failure cases.
        /// </summary>
        public static class Failure
        {
            /// <summary>
            /// Gets a new <see cref="AuthorizationResult"/> that describes that the token was missing in the database.
            /// </summary>
            /// <returns></returns>
            public static AuthorizationResult MissingToken
            {
                get
                {
                    return new AuthorizationResult(
                    isSuccessful: false,
                    errorDescription: new AuthorizationResultDescription
                    {
                        Error = "missing_grant",
                        ErrorDescription = "The given authorization grant (Access Token) does not exist."
                    });
                }
            }

            /// <summary>
            /// Gets a new <see cref="AuthorizationResult"/> that describes that the token was revoked from the client.
            /// </summary>
            /// <returns></returns>
            public static AuthorizationResult RevokedToken
            {
                get
                {
                    return new AuthorizationResult(
                    isSuccessful: false,
                    errorDescription: new AuthorizationResultDescription
                    {
                        Error = "revoked_grant",
                        ErrorDescription = "The given authorization grant (Access Token) has been revoked and therefore is no longer valid."
                    });
                }
            }

            /// <summary>
            /// Gets a new <see cref="AuthorizationResult"/> that describes that the token has expired.
            /// </summary>
            /// <returns></returns>
            public static AuthorizationResult ExpiredToken
            {
                get
                {
                    return new AuthorizationResult(
                    isSuccessful: false,
                    errorDescription: new AuthorizationResultDescription
                    {
                        Error = "expired_grant",
                        ErrorDescription = "The given authorization grant (Access Token) has expired and therefore is no longer valid. Use the granted refresh token to gain another grant."
                    });
                }
            }

            /// <summary>
            /// Gets a new <see cref="AuthorizationResult"/> that describes that the token's granted scopes did not cover what was needed to access the resource.
            /// </summary>
            /// <returns></returns>
            public static AuthorizationResult NotGrantedPermission
            {
                get
                {
                    return new AuthorizationResult(
                    isSuccessful: false,
                    errorDescription: new AuthorizationResultDescription
                    {
                        Error = "not_enough_permissions",
                        ErrorDescription = "The given authorization grant (Access Token) has not been granted access to one or more of the required scopes."
                    });
                }
            }

            /// <summary>
            /// Gets a new <see cref="AuthorizationResult"/> that describes that the specified type for the grant is not supported.
            /// </summary>
            /// <returns></returns>
            public static AuthorizationResult UnsupportedType
            {
                get
                {
                    return new AuthorizationResult(
                    isSuccessful: false,
                    errorDescription: new AuthorizationResultDescription
                    {
                        Error = "unsupported_grant_type",
                        ErrorDescription = "The specified type for the grant is not supported for authentication."
                    });
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationResult"/> class.
        /// </summary>
        /// <param name="isSuccessful">if set to <c>true</c> [is successful].</param>
        public AuthorizationResult(bool isSuccessful) : this(isSuccessful, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationResult"/> class.
        /// </summary>
        /// <param name="isSuccessful">if set to <c>true</c> [is successful].</param>
        /// <param name="errorDescription">The error description.</param>
        public AuthorizationResult(bool isSuccessful, IAuthorizationResultDescription errorDescription)
        {
            this.IsSuccessful = isSuccessful;
            this.ErrorDescription = errorDescription;
        }

        /// <summary>
        /// Gets a 
        /// <see cref="IAuthorizationResultDescription" /> object that describes the error that occured.
        /// Can be null if 
        /// <see cref="IsSuccessful" /> is true.
        /// </summary>
        /// <returns></returns>
        public IAuthorizationResultDescription ErrorDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether or not the authorization was successful.
        /// If true, the given access token/request was valid, otherwise it is not.
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessful
        {
            get;
            private set;
        }
    }
}
