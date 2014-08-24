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

namespace OAuthWorks.ExtensionMethods
{
    /// <summary>
    /// Defines a static class that contains extension methods for <see cref="IEnumerable{T}"/> objects.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Executes the given action on every one of the objects in this enumerable list.
        /// </summary>
        /// <typeparam name="T">The type of the objects being enumerated.</typeparam>
        /// <param name="objects">The enumerable list of objects to perform the action on.</param>
        /// <param name="action">The action that should be performed for each object in the list.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when either <paramref name="objects"/> or <paramref name="action"/> is null.
        /// </exception>
        public static IEnumerable<TReturn> ForEach<T, TReturn>(this IEnumerable<T> objects, Func<T, TReturn> action)
        {
            if (objects == null)
            {
                throw new ArgumentNullException("objects");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            foreach (T a in objects)
            {
                yield return action(a);
            }
        }

        /// <summary>
        /// Executes the given action on every one of the objects in this enumerable list.
        /// </summary>
        /// <typeparam name="T">The type of the objects being enumerated.</typeparam>
        /// <param name="objects">The enumerable list of objects to perform the action on.</param>
        /// <param name="action">The action that should be performed for each object in the list.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when either <paramref name="objects"/> or <paramref name="action"/> is null.
        /// </exception>
        public static void ForEach<T>(this IEnumerable<T> objects, Action<T> action)
        {
            if(objects == null)
            {
                throw new ArgumentNullException("objects");
            }
            if(action == null)
            {
                throw new ArgumentNullException("action");
            }
            foreach(T a in objects)
            {
                action(a);
            }
        }
    }
}
