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
    /// Defines an interface that provides access to processed values that were given in an <see cref="IAuthorizationCodeRequest"/>.
    /// Provides access to resolved <see cref="IClient"/>, <see cref="IUser"/>, and <see cref="IEnumerable{IScope}"/> objects.
    /// </summary>
    public interface IProcessedAuthorizationCodeRequest
    {
        /// <summary>
        /// Gets the <see cref="IClient"/> that made the original <see cref="Request"/>.
        /// </summary>
        /// <returns>Returns a <see cref="IClient"/> object.</returns>
        IClient Client
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IUser"/> that the request was made for.
        /// </summary>
        /// <returns>Returns a <see cref="IUser"/> object.</returns>
        IUser User
        {
            get;
        }

        /// <summary>
        /// Gets the scopes that the <see cref="Client"/> requested.
        /// </summary>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of <see cref="IScope"/> objects.</returns>
        IEnumerable<IScope> Scopes
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IAuthorizationCodeRequest"/> that the <see cref="Client"/> made.
        /// </summary>
        /// <returns>Returns a <see cref="IAuthorizationCodeRequest"/> object.</returns>
        IAuthorizationCodeRequest OriginalRequest
        {
            get;
        }
    }
}
