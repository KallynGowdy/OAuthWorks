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

namespace OAuthWorks.Factories
{
    /// <summary>
    /// Defines an interface for a factory that creates <see cref="OAuthWorks.IRefreshToken"/> objects.
    /// </summary>
    /// <typeparam name="TRefreshToken">The type of the <see cref="OAuthWorks.IRefreshToken"/> objects that are created with this object.</typeparam>
    public interface IRefreshTokenFactory<out TRefreshToken> : IFactory<TRefreshToken> where TRefreshToken : IRefreshToken
    {
        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IRefreshToken"/> that can be used by the given <see cref="OAuthWorks.IClient"/> for the given <see cref="OAuthWorks.IUser"/> for the given
        /// enumerable <see cref="OAuthWorks.IScope"/> objects.
        /// </summary>
        /// <param name="generatedToken">The refreshToken that was generated as it should be returned to the client.</param>
        /// <param name="client">The client that will be using the issued refresh refreshToken.</param>
        /// <param name="user">The user that is granting access to the given client for the given scopes.</param>
        /// <param name="scopes">The enumerable list of <see cref="OAuthWorks.IScope"/> objects that define what access the client has to the
        /// user's account and data.</param>
        /// <returns>Returns a new Refresh Token that can be used to request new Access Tokens.</returns>
        ICreatedToken<TRefreshToken> Create(IClient client, IUser user, IEnumerable<IScope> scopes);

    }
}
