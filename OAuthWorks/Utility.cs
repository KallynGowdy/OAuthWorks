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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a list of utility extension methods for objects.
    /// </summary>
    public static class Utility
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        /// <summary>
        /// Throws a new System.ArgumentNullException if the value of the object is null.
        /// </summary>
        /// <param name="value">The object to check for null-ness.</param>
        /// <param name="parameterName">The name of the parameter that is being checked. Defaults to the name of the member that this method is called on.</param>
        [ContractArgumentValidator]
        public static void ThrowIfNull(this object value, [CallerMemberName] string parameterName = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            Contract.EndContractBlock();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        /// <summary>
        /// Throws a new <see cref="ArgumentOutOfRangeException"/> if the value is not between the two given values.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="minInclusive">The minimum value that the value is allowed to equal.</param>
        /// <param name="maxExclusive">The minimum value that the value can equal while still being invalid.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        [ContractArgumentValidator]
        public static void ThrowIfNotInRange(this int value, int? minInclusive, int? maxExclusive, [CallerMemberName] string parameterName = null)
        {
            if((minInclusive.HasValue && value < minInclusive) || (maxExclusive.HasValue && value >= maxExclusive))
            {
                throw new ArgumentOutOfRangeException(parameterName, string.Format("{0} must be greater than or equal to {1} and less than {2}", 
                    parameterName,
                    minInclusive.HasValue ? minInclusive.Value.ToString() : "-Infinity",
                    maxExclusive.HasValue ? maxExclusive.Value.ToString() : "Infinity"));
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
        /// <param name="value">The object to retreive to query string for.</param>
        /// <returns></returns>
        public static string ToQueryString(this object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            Type t = value.GetType();
            NameValueCollection values = System.Web.HttpUtility.ParseQueryString(string.Empty);

            foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !p.IsSpecialName && p.CanRead))
            {
                values.Add(p.Name, (p.GetValue(value) ?? "").ToString());
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
            if (value == null) throw new ArgumentNullException("value");
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
            if (value == null) throw new ArgumentNullException("value");
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).First();
            IEnumerable<EnumSubgroupAttribute> attributes = member.GetCustomAttributes<EnumSubgroupAttribute>(false);
            return (TEnum)attributes.Where(e => e.SubgroupOf is TEnum).First().SubgroupOf;
        }

        /// <summary>
        /// Gets the description of this enum value, returns null if no description attribute was applied.
        /// </summary>
        /// <param name="value">The value to get the description of.</param>
        /// <returns>Returns a string representing the description of the value.</returns>
        public static string GetDescription(this Enum value)
        {
            if (value == null) throw new ArgumentNullException("value");
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).First();
            return member.GetCustomAttribute<DescriptionAttribute>().Description;
        }
    }
}
