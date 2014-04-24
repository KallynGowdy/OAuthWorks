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
    /// Defines an interface for an object that can read other objects from it's store but does not need to provide Write operations.
    /// </summary>
    /// <typeparam name="TValue">The type of the values that can be read from this store.</typeparam>
    /// <typeparam name="TKey">The type of the keys/ids that are used to get values from this store.</typeparam>
    public interface IReadStore<in TKey, out TValue> : IDisposable
    {
        /// <summary>
        /// Gets an entity by it's identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>Returns the requested entity if it is contained in the repository, otherwise null.</returns>
        TValue GetById(TKey id);
    }
}
