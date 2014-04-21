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
    /// Defines an interface for an object that can store and manipulate objects permanently without needing to provide read capabilities.
    /// </summary>
    /// <typeparam name="TValue">The type of objects that are stored in this object.</typeparam>
    public interface IWriteStore<in TKey, in TValue> : IDisposable
    {
        /// <summary>
        /// Adds the given object to the repository.
        /// </summary>
        /// <param name="obj">The object to add to the respository.</param>
        void Add(TValue obj);

        /// <summary>
        /// Updates the given object in the repository.
        /// </summary>
        /// <param name="obj">The object that should be updated.</param>
        void Update(TValue obj);

        /// <summary>
        /// Removes the authorization code with the given id from the repository.
        /// </summary>
        /// <param name="id">The Id of the object to remove.</param>
        void RemoveById(TKey id);
    }
}
