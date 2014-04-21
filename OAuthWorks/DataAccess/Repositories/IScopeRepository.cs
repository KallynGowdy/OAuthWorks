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

namespace OAuthWorks.DataAccess.Repositories
{
    /// <summary>
    /// Defines an interface for a repository that contains <see cref="OAuthWorks.IScope"/> objects.
    /// </summary>
    public interface IScopeRepository<out TOut> : IReadStore<string, TOut>, IEnumerable<TOut> where TOut : IScope
    {
        /// <summary>
        /// Gets a list of all of the scopes that are provided by the given access token.
        /// </summary>
        /// <param name="token">The token to get all of the scopes for.</param>
        /// <returns>Returns an enumerable list of the scopes that are provided by the given token.</returns>
        IEnumerable<TOut> GetByToken(IAccessToken token);
    }
}
