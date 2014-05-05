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
    /// Defines an interface for an object that contains a value that is used as a representation of granted authorization.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Determines if the given refreshToken value matches the one stored internally.
        /// </summary>
        /// <param name="refreshToken">The refreshToken to compare to the internal one.</param>
        /// <returns>Returns true if the two tokens match, otherwise false.</returns>
        bool MatchesValue(string token);
    }
}
