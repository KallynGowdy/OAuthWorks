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
    /// Defines an interface for an object that retrieves other objects based on type.
    /// </summary>
    public interface IDependencyInjector
    {
        /// <summary>
        /// Gets a new object of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <returns>
        /// Returns a new object of the given type if the dependency injector can provide that type.
        /// Returns null if the dependency injector does not support the given type.
        /// </returns>
        T GetInstance<T>();
    }
}
