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

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for an object that contains information on a Scope.
    /// </summary>
    public interface IScope
    {
        // Every scope has an identifier, it is what the client will provide to request access to the resources that the scope controls.

        /// <summary>
        /// Gets the identifier of the scope. This is the value that is refered to in the specification.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Gets or sets the description of the scope. This should be a human-readable summary of what the scope provides.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Determines if the two scopes equal each other.
        /// </summary>
        /// <param name="other">The other scope to determine equality to.</param>
        /// <returns>Returns true if the two scopes equal each other, otherwise false.</returns>
        bool Equals(IScope other);
    }
}
