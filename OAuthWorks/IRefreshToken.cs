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
    /// Defines an interface for refresh tokens.
    /// </summary>
    public interface IRefreshToken
    {
        /// <summary>
        /// Determines if the given value matches the one stored in this object.
        /// </summary>
        /// <param name="token">The token to match to the value stored in this object.</param>
        /// <returns>Returns true if the tokens match, otherwise returns false.</returns>
        bool MatchesToken(string token);

        /// <summary>
        /// Gets the client that this token belongs to.
        /// </summary>
        IClient Client
        {
            get;
        }

        /// <summary>
        /// Gets whether this token has been revoked by the user.
        /// </summary>
        bool Revoked
        {
            get;
        }
    }
}
