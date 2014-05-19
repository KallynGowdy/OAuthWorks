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
        /// <param name="response">The response to examine whether redirection is valid.</param>
        /// <returns>Returns true if the user should be redirect, otherwise false.</returns>
        public static bool ShouldRedirect(this IAuthorizationCodeResponse response)
        {
            return response.Redirect != null;
        }
    }
}
