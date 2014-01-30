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
    /// Defines an interface for a factory that creates access token objects.
    /// </summary>
    public interface IAccessTokenFactory<in TAccessToken> : IFactory<TAccessToken> where TAccessToken : IAccessToken
    {
        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessToken"/> given the token value and expiration date in Universal Coordinated Time.
        /// </summary>
        /// <param name="token">The value that defines the 'password' of the token which determines if a client should have access to a resource.</param>
        /// <param name="expirationDateUtc">The expiration date of the token in Universal Coordinated Time.</param>
        /// <returns></returns>
        TAccessToken Get(string token, DateTime expirationDateUtc);

        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessToken"/> given the Client and the list of scopes that the client has access to.
        /// </summary>
        /// <param name="clientId">The client that should have access to the new token.</param>
        /// <param name="scopes">The list of scopes that the client should have access to.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessToken"/> object.</returns>
        TAccessToken Get(IClient client, params IScope[] scopes);

        /// <summary>
        /// Gets a new <see cref="OAuthWorks.IAccessToken"/> object given the <see cref="OAuthWorks.IClient"/> and the list of scopes that the client has access to.
        /// </summary>
        /// <param name="client">The client that should have access to the new token.</param>
        /// <param name="scopes">The list of scopes that the client should have access to.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.IAccessToken"/> object.</returns>
        TAccessToken Get(IClient client, IEnumerable<IScope> scopes);
    }
}
