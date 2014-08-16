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
    /// Defines an interface for objects that define a Scope Authorization Request. That is, they contain
    /// a set of values that are used in the user scope authorization prompt.
    /// </summary>
    public interface IScopeAuthorizationRequest
    {
        /// <summary>
        /// Gets the <see cref="IClient"/> that is requesting access to the given scopes.
        /// </summary>
        /// <returns>Returns the <see cref="IClient"/> that is requesting authorization for the scopes.</returns>
        IClient Client
        {
            get;
        }

        /// <summary>
        /// Gets the list of <see cref="IScope"/> objects that the client is requesting authorization to.
        /// </summary>
        /// <returns>Returns a <see cref="IEnumerable{IScope}"/> object that contain the list of <see cref="IScope"/> objects that the client is requesting authorization to.</returns>
        IEnumerable<IScope> Scopes
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IUser"/> that the access is being requested from.
        /// </summary>
        /// <returns>Returns a <see cref="IUser"/> object that the authorization is being requested from.</returns>
        IUser User
        {
            get;
        }

        /// <summary>
        /// Gets the State that the Client sent in it's request to act as a CSRF token.
        /// </summary>
        /// <returns>Returns a <see cref="string"/> that the Client sent in it's request.</returns>
        string State
        {
            get;
        }

        /// <summary>
        /// Gets the redirect uri that the client gave in the request.
        /// </summary>
        /// <returns></returns>
        string RedirectUri
        {
            get;
        }
    }
}
