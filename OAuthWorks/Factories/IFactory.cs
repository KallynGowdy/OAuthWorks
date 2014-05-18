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
    /// Defines an interface for an object factory that produces objects of the given type.
    /// </summary>
    /// <typeparam name="T">The type of the objects that the factory produces.</typeparam>
    public interface IFactory<out T>
    {
        /// <summary>
        /// Gets the default object of the type T.
        /// </summary>
        /// <remarks>
        /// This method should not be confused with 
        /// <code>
        /// default(T);
        /// </code>
        /// It returns the default object as defined by the factory, which is up to the creator of the factory.
        /// Acceptable responses are null, an non-null empty object or an object that is fully initalized based on whatever the 
        /// implementation defines. Therefore beware when calling this method without proper documentation on the specific behaviour.
        /// </remarks>
        /// <returns>Returns the default value for objects of the type T.</returns>
        T Create();

    }
}
