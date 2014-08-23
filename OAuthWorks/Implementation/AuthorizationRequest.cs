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
    /// Defines a class that provides a basic implementation of <see cref="IAuthorizationRequest"/>.
    /// </summary>
    public class AuthorizationRequest : IAuthorizationRequest
    {
        /// <summary>
        /// Gets the value that was given in the 'Authorization' header of the request.
        /// </summary>
        /// <returns></returns>
        public string Authorization
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of scopes that are required to have been granted to the token in order for the request to be valid/authorized.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IScope> RequiredScopes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the authorization type that was given in the 'Authorization' header of the request.
        /// </summary>
        /// <returns></returns>
        public string Type
        {
            get;
            set;
        }
    }
}
