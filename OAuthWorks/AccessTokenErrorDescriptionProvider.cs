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
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using OAuthWorks.ExtensionMethods;
using PortableOAuthWorks.DataAnnotations;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a basic implementation of <see cref="IAccessTokenErrorDescriptionProvider"/>.
    /// </summary>
    public class AccessTokenErrorDescriptionProvider : IAccessTokenErrorDescriptionProvider
    {
        /// <summary>
        /// Gets the human-readable description of the specific error.
        /// </summary>
        /// <param name="specificError">The error that the description should be retrieved for.</param>
        /// <returns>
        /// Returns a string that represents the human-readable description for the given error.
        /// </returns>
        public string GetDescription(AccessTokenSpecificRequestError specificError)
        {
			return specificError.GetDescription();
        }
    }
}
