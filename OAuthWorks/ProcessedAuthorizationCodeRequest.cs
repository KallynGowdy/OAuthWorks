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
    /// Defines a class that provides a basic implementation of <see cref="IProcessedAuthorizationCodeRequest"/>.
    /// </summary>
    public class ProcessedAuthorizationCodeRequest : IProcessedAuthorizationCodeRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessedAuthorizationCodeRequest"/> class.
        /// </summary>
        /// <param name="originalRequest">The original request that was made by the <see cref="IClient"/>. (Required)</param>
        /// <param name="client">The client that made the request. (Optional)</param>
        /// <param name="user">The user that the request was made for. (Optional)</param>
        /// <param name="scopes">The scopes that were requested from the user. (Optional)</param>
        public ProcessedAuthorizationCodeRequest(IAuthorizationCodeRequest originalRequest, IClient client, IUser user, IEnumerable<IScope> scopes)
        {
            if (originalRequest == null) throw new ArgumentNullException("originalRequest");
            this.OriginalRequest = originalRequest;
            this.Client = client;
            this.User = user;
            this.Scopes = scopes;
        }

        /// <summary>
        /// Gets the <see cref="IClient" /> that made the original <see cref="Request" />.
        /// </summary>
        /// <returns>Returns a <see cref="IClient" /> object.</returns>
        public IClient Client
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="IAuthorizationCodeRequest" /> that the <see cref="Client" /> made.
        /// </summary>
        /// <returns>Returns a <see cref="IAuthorizationCodeRequest" /> object.</returns>
        public IAuthorizationCodeRequest OriginalRequest
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the scopes that the <see cref="Client" /> requested.
        /// </summary>
        /// <returns>Returns an <see cref="IEnumerable{T}" /> of <see cref="IScope" /> objects.</returns>
        public IEnumerable<IScope> Scopes
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="IUser" /> that the request was made for.
        /// </summary>
        /// <returns>Returns a <see cref="IUser" /> object.</returns>
        public IUser User
        {
            get;
            private set;
        }
    }
}
