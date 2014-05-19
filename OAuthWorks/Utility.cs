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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a list of utility extension methods for objects.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Throws a new System.ArgumentNullException if the value of the object is null.
        /// </summary>
        /// <param name="value">The object to check for null-ness.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        [ContractArgumentValidator]
        public static void ThrowIfNull(this object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            Contract.EndContractBlock();
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
            foreach (T a in objects)
            {
                action(a);
            }
        }

        /// <summary>
        /// Converts the given object into a query string representation.
        /// </summary>
        /// <param name="obj">The object to retreive to query string for.</param>
        /// <returns></returns>
        public static string ToQueryString(this object obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            Type t = obj.GetType();
            List<Tuple<string, string>> values = new List<Tuple<string, string>>();

            foreach(PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !p.IsSpecialName && p.CanRead))
            {
                values.Add(new Tuple<string, string>(p.Name, (p.GetValue(obj) ?? "").ToString()));
            }

            return "?" + string.Join("&", values.Where(v => v.Item2 != null).Select(v => string.Format("{0}={1}", v.Item1, Uri.EscapeDataString(v.Item2))));
        }
    }
}
