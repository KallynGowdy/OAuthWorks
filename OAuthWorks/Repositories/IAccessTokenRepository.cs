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

namespace OAuthWorks.Repositories
{
    /// <summary>
    /// Defines an interface for a repository that contains <see cref="OAuthWorks.IAccessToken"/> objects.
    /// </summary>
    public interface IAccessTokenRepository<T> : IRepository<string, T> where T : IAccessToken
    {
        /// <summary>
        /// Gets a list of Access Tokens that belong to the given user.
        /// </summary>
        /// <param name="user">The User that owns the access tokens.</param>
        /// <returns>Returns a new enumerable list of <see cref="OAuthWorks.IAccessToken"/> objects that belong to the given user.</returns>
        IEnumerable<T> GetByUser(IUser user);
    }
}
