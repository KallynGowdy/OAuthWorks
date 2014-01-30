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
    /// Defines an interface for a repository.
    /// </summary>
    /// <typeparam name="T">The type of the entities that are stored in this repository.</typeparam>
    /// <typeparam name="K">The type of the identifier that is used to retrieve objects from the respository.</typeparam>
    public interface IRepository<K, in T>
    {
        /// <summary>
        /// Gets an entity by it's identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>Returns the requested entity if it is contained in the repository, otherwise null.</returns>
        T GetById(K id);

        /// <summary>
        /// Adds the given object to the repository.
        /// </summary>
        /// <param name="obj">The object to add to the respository.</param>
        void Add(T obj);

        /// <summary>
        /// Updates the given object in the repository.
        /// </summary>
        /// <param name="obj">The object that should be updated.</param>
        void Update(T obj);

        /// <summary>
        /// Removes the authorization code with the given id from the repository.
        /// </summary>
        /// <param name="id">The Id of the object to remove.</param>
        void RemoveById(K id);

    }
}
