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



using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Factories
{
    /// <summary>
    /// Defines an interface for a factory that creates access refreshToken objects.
    /// </summary>
    public interface IAccessTokenFactory<out TAccessToken> : IFactory<TAccessToken> where TAccessToken : IAccessToken
    {
        /// <summary>
        /// Gets a new <see cref="ICreatedToken{IAccessToken}"/> object given the <see cref="OAuthWorks.IClient"/>, <see cref="OAuthWorks.IUser"/> and the list of scopes that the client has access to.
        /// </summary>
        /// <param name="client">The client that should have access to the new refreshToken.</param>
        /// <param name="user">The user that is giving access to the client.</param>
        /// <param name="scopes">The list of scopes that the client should have access to.</param>
        /// <returns>Returns a new OAuthWorks.CreatedToken(of TAccessToken) object.</returns>
        ICreatedToken<TAccessToken> Create(IClient client, IUser user, IEnumerable<IScope> scopes);
    }
}
