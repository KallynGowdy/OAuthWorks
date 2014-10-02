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
    /// Defines a static class that contains extension methods for <see cref="OAuthWorks.IAccessToken"/> objects.
    /// </summary>
    public static class AccessTokenExtensions
    {
        /// <summary>
        /// Determines if this token is valid. That is, not revoked or expired.
        /// </summary>
        /// <param name="token">The token to determine validity for.</param>
        /// <returns>
        /// Returns true if the token has not expired or been revoked. Otherwise returns false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the given token is null.</exception>
        public static bool IsValid(this IAccessToken token)
        {
            if(token == null)
            {
                throw new ArgumentNullException("token");
            }
            return !token.Expired && !token.Revoked;
        }

    }
}
