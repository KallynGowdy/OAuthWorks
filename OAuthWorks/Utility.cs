﻿// Copyright 2014 Kallyn Gowdy
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
    }
}
