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

namespace OAuthWorks.ExtensionMethods
{
    /// <summary>
    /// Defines a static class that contains extension methods for <see cref="OAuthWorks.IRefreshToken"/> objects.
    /// </summary>
    public static class RefreshTokenExtensions
    {
        /// <summary>
        /// Determines if the token is in a valid, usable state for the client that was given it.
        /// </summary>
        /// <param name="token">The token to determine validity for.</param>
        /// <returns>Returns true if the token has not been revoked and if it has not expired. Otherwise false.</returns>
        public static bool IsValid(this IRefreshToken token)
        {
            if(token == null)
            {
                throw new ArgumentNullException("token");
            }
            return !token.Revoked && !token.Expired;
        }
    }
}
