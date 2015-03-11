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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OAuthWorks.ExtensionMethods;

namespace OAuthWorks
{

    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="IAuthorizationCodeErrorDescriptionProvider"/>.
    /// </summary>
    public class AuthorizationCodeErrorDescriptionProvider : IAuthorizationCodeErrorDescriptionProvider
    {
        /// <summary>
        /// Gets a human-readable description of the given generic error and specific error.
        /// </summary>
        /// <param name="specificError">The specific cause of the error that occurred.</param>
        /// <returns>
        /// Returns a human-readable string that describes the problem that occurred.
        /// </returns>
        public string GetDescription(AuthorizationCodeRequestSpecificErrorType specificError)
        {
			return specificError.GetDescription();
        }
    }
}
