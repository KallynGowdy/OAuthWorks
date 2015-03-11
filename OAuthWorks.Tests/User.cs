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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class User : IUser, IHasId<string>
    {
		/// <summary>
		/// Gets or sets the ID of the user.
		/// </summary>
        public string Id
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the list of scopes that the user has granted.
		/// </summary>
        public Dictionary<IClient, IEnumerable<IScope>> Scopes
        {
            get;
            set;
        }

	    /// <summary>
	    /// Determines if the given scope has been granted to the given client by this user.
	    /// </summary>
	    /// <remarks>
	    /// This method should only determine if access to the given scope was granted, not if an Authorization Code was issued.
	    /// This allows the <see cref="IOAuthProvider"/> to determine if an Authorization Code can be issued or if the user needs to provide consent first.
	    /// </remarks>
	    /// <param name="client">The client that is used to determine whether it has been granted the scope by this user.</param>
	    /// <param name="scope">The scope that may or may not have been granted to the given client.</param>
	    /// <returns>Returns true if the given client has been granted the given scope, otherwise false.</returns>
	    public bool HasGrantedScope(IClient client, IScope scope)
        {
			IEnumerable<IScope> scopes;
            return Scopes.TryGetValue(client, out scopes) && scopes.Contains(scope);
        }
    }
}
