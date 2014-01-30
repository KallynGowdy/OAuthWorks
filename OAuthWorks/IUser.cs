using System;
using System.Collections.Generic;
using System.Linq;
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


using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for a User of an OAuth 2.0 provider.
    /// </summary>
    public interface IUser
    {
        // The User needs to be able to retrieve information such as,
        // 
        // - Id
        // - Granted Permissions

        /// <summary>
        /// Gets the Id of the user.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Determines if the given scope has been granted to the given client by this user.
        /// </summary>
        /// <param name="client">The client that is used to determine whether it has been granted the scope by this user.</param>
        /// <param name="scope">The scope that may or may not have been granted to the given client.</param>
        /// <returns>Returns true if the given client has been granted the given scope, otherwise false.</returns>
        bool HasGrantedScope(IClient client, IScope scope);
    }
}
