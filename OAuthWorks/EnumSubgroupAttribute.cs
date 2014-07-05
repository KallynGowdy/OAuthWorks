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
    /// Defines an attribute that specifies that particular enum value belongs to a subgroup of another enum value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumSubgroupAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the enum value that this Enum member is a subgroup of.
        /// </summary>
        /// <returns></returns>
        public object SubgroupOf
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumSubgroupAttribute"/> class.
        /// </summary>
        /// <param name="subgroupOf">The subgroup that this enum member is a member of.</param>
        public EnumSubgroupAttribute(object subgroupOf)
        {
            this.SubgroupOf = subgroupOf;
        }
    }
}
