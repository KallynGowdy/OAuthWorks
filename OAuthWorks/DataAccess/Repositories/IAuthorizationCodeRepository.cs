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

namespace OAuthWorks.DataAccess.Repositories
{
    /// <summary>
    /// Defines an interface for storing <see cref="OAuthWorks.IAuthorizationCode"/> objects.
    /// </summary>
    public interface IAuthorizationCodeRepository<T> : IRepository< T> where T : IAuthorizationCode
    {
        /// <summary>
        /// Gets the authorization code by it's actual internal value.
        /// </summary>
        /// <param name="authorizationCode">The authorization code that was issued to a client.</param>
        /// <returns>Returns the complete authorization code.</returns>
        /// <remarks>
        /// This method is used to allow flexibility to implementations in how they store Ids. 
        /// Authorization code factories can return authorization codes that contain Ids in them for easy retrieval.
        /// It is also possible to use different possiblities.
        /// </remarks>
        T GetByValue(string authorizationCode);

        /// <summary>
        /// Gets a list of Authorization codes by the given client.
        /// </summary>
        /// <param name="client">The client </param>
        /// <returns></returns>
        //IEnumerable<T> GetByClient(IClient client);
    }
}
