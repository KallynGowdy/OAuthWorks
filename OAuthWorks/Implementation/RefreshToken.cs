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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines an abstract class that provides a basic implementation of <see cref="OAuthWorks.IRefreshToken"/>.
    /// </summary>
    public abstract class RefreshToken : IRefreshToken
    {
        protected RefreshToken(IUser user, IClient client, IEnumerable<IScope> scopes)
        {
            Contract.Requires(user != null);
            Contract.Requires(client != null);
            Contract.Requires(scopes != null);
            this.Client = client;
            this.User = user;
            this.Scopes = scopes;
        }

        /// <summary>
        /// Gets the client that this token belongs to.
        /// </summary>
        public virtual IClient Client
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets whether this token has been revoked by the user.
        /// </summary>
        public virtual bool Revoked
        {
            get;
            protected set;
        }

        /// <summary>
        /// Determines if the given token value matches the one stored internally.
        /// </summary>
        /// <param name="token">The token to compare to the internal one.</param>
        /// <returns>
        /// Returns true if the two tokens match, otherwise false.
        /// </returns>
        public abstract bool MatchesValue(string token);


        /// <summary>
        /// Gets the user that the refresh token belongs to.
        /// </summary>
        public virtual IUser User
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the list of scopes that this refresh token provides access to.
        /// </summary>
        public virtual IEnumerable<IScope> Scopes
        {
            get;
            set;
        }
    }
}
