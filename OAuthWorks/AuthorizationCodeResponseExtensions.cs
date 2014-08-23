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

using OAuthWorks.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a class that contains extension methods for <see cref="IAuthorizationCodeResponse"/> objects.
    /// </summary>
    public static class AuthorizationCodeResponseExtensions
    {
        /// <summary>
        /// Determines if the user should be redirected to the <see cref="Uri"/> contained in the response.
        /// </summary>
        /// <param name="response">The response to examine whether redirection is allowed.</param>
        /// <returns>Returns true if the user should be redirect, otherwise false.</returns>
        public static bool ShouldRedirect(this IAuthorizationCodeResponse response)
        {
            if (response == null) throw new ArgumentNullException("response");
            return response.Redirect != null &&
                response.IsSuccessful;
        }

        /// <summary>
        /// Determines if the user should be prompted to authorize all of the requested scopes contained in the request.
        /// </summary>
        /// <param name="response">The response from the <see cref="IOAuthProvider"/> object.</param>
        /// <returns>Returns true if the user should be prompted, otherwise false.</returns>
        public static bool ShouldValidateScopes(this IAuthorizationCodeResponse response)
        {
            return response is IUnsuccessfulAuthorizationCodeResponse &&
                ((IUnsuccessfulAuthorizationCodeResponse)response).SpecificErrorCode == AuthorizationCodeRequestSpecificErrorType.UserUnauthorizedScopes;
        }

        /// <summary>
        /// Gets a new <see cref="IScopeAuthorizationRequest"/> object that contains values used in the user scope authorization if authorization is required,
        /// otherwise returns null.
        /// </summary>
        /// <param name="response">The <see cref="IAuthorizationCodeResponse"/> returned from the <see cref="IOAuthProvider"/>.</param>
        /// <returns>Returns a new <see cref="IScopeAuthorizationRequest"/> object if user authorization is required, otherwise null.</returns>
        public static IScopeAuthorizationRequest GetScopeAuthorizationRequest(this IAuthorizationCodeResponse response)
        {
            if ((IUnsuccessfulAuthorizationCodeResponse unsuccessfulResponse = response as IUnsuccessfulAuthorizationCodeResponse) != null &&
                (unsuccessfulResponse.SpecificErrorCode == AuthorizationCodeRequestSpecificErrorType.UserUnauthorizedScopes))
            {
                return new ScopeAuthorizationRequest
                (
                    client: unsuccessfulResponse.Request.Client,
                    scopes: unsuccessfulResponse.Request.Scopes,
                    state: unsuccessfulResponse.State,
                    user: unsuccessfulResponse.Request.User,
                    redirectUri: unsuccessfulResponse.Redirect.AbsoluteUri
                );
            }
            else
            {
                return null;
            }
        }
    }
}
