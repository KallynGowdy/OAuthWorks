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
    /// Defines an interface for a representation of an OAuth Client from the perspective of an OAuth provider.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Gets the name of the client.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the list of allowed redirect URIs for this client.
        /// </summary>
        IEnumerable<string> RedirectUris
        {
            get;
        }

        /// <summary>
        /// Determines if the given secret matches the one stored internally.
        /// </summary>
        /// <param name="secret">The secret to match to the internal one.</param>
        /// <returns>Returns true if the secrets match, otherwise false.</returns>
        bool MatchesSecret(string secret);

        /// <summary>
        /// Determines if this client equals the given other client.
        /// </summary>
        /// <param name="other">The other client to determine equality to.</param>
        /// <returns>Returns true if the two clients equal each other, otherwise returns false.</returns>
        bool Equals(IClient other);
    }
}
