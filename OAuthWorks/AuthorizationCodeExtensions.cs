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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a static class that provides extension methods for <see cref="OAuthWorks.IAuthorizationCode"/> objects.
    /// </summary>
    public static class AuthorizationCodeExtensions
    {
        /// <summary>
        /// Determines if the authorization code is valid for use by the client.
        /// </summary>
        /// <param name="code">The code that should be validated.</param>
        /// <returns>
        /// Returns true if the code has not been revoked and has not expired. Otherwise false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the given code is null.</exception>
        public static bool IsValid(this IAuthorizationCode code)
        {
            if(code == null)
            {
                throw new ArgumentNullException("code");
            }
            return !code.Revoked && !code.Expired;
        }
    }
}
