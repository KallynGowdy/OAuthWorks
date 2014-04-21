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
    /// Defines an interface for a repository.
    /// </summary>
    /// <remarks>
    /// A repository is an abstraction of basic database operations. It allows a library to function without specific database code.
    /// Repositories are used in a unit-of-work pattern. This means that they should be easily created and disposed, since every different
    /// transaction requires a new repository. Repositories are usually requested through the use of IoC containers, this allows the implementation to easily
    /// be swapped out by another implementation for testing.
    /// </remarks>
    /// <typeparam name="TValue">The type of the entities that are stored in this repository.</typeparam>
    public interface IRepository<TValue> : IReadStore<string, TValue>, IWriteStore<string, TValue>, IEnumerable<TValue>, IDisposable
    {
    }
}
