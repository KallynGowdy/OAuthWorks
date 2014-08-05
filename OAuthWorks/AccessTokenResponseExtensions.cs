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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a static class that provides extension methods for <see cref="IAccessTokenResponse"/> objects.
    /// </summary>
    public static class AccessTokenResponseExtensions
    {
        /// <summary>
        /// Gets the <see cref="HttpStatusCode"/> that represents whether the request was a success or failure.
        /// </summary>
        /// <param name="response">The response that the <see cref="HttpStatusCode"/> should be retrieved for.</param>
        /// <returns>Returns the <see cref="HttpStatusCode"/> representing the status code of the <see cref="IAccessTokenResponse"/>.</returns>
        public static HttpStatusCode StatusCode(this IAccessTokenResponse response)
        {
            return response is ISuccessfulAccessTokenResponse ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        }
    }
}
