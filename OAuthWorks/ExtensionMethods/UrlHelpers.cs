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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.ExtensionMethods
{
    /// <summary>
    /// Defines a list of extension methods that provide helpers for creating URLs.
    /// </summary>
    public static class UrlHelpers
    {        
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
    }
}
