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
using System.Collections.Specialized;
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
            NameValueCollection values = System.Web.HttpUtility.ParseQueryString(string.Empty);

            foreach(PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !p.IsSpecialName && p.CanRead))
            {
                values.Add(p.Name, (p.GetValue(obj) ?? "").ToString());
            }

            return "?" + values.ToString();
        }

        /// <summary>
        /// Determines if this enum value is a child of the given other enum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns>Returns true if this enum value contains an attribute that defines it as a subgroup of the given other.</returns>
        public static bool IsSubgroupOf(this Enum value, Enum other)
        {
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).First();
            IEnumerable<EnumSubgroupAttribute> attributes = member.GetCustomAttributes<EnumSubgroupAttribute>(false);
            return attributes.Any(a => a.SubgroupOf.Equals(other));
        }

        /// <summary>
        /// Gets the first subgroup that the given enum belongs to and returns it.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum that the value is a subgroup of.</typeparam>
        /// <param name="value">The enum value to get the subgroup that it belongs to.</param>
        /// <returns>Returns the enum value of the first subgroup of the given type that this value belongs to.</returns>
        public static TEnum GetSubgroup<TEnum>(this Enum value)
        {
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).First();
            IEnumerable<EnumSubgroupAttribute> attributes = member.GetCustomAttributes<EnumSubgroupAttribute>(false);
            return (TEnum) attributes.Where(e => e.SubgroupOf is TEnum).First().SubgroupOf;
        }
    }
}
