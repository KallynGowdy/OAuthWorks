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
    /// Defines an interface for a repository that contains <see cref="OAuthWorks.IAccessToken"/> objects.
    /// </summary>
    public interface IAccessTokenRepository<T> : IRepository<T> where T : IAccessToken
    {
        /// <summary>
        /// Gets a list of Access Tokens that belong to the given user.
        /// </summary>
        /// <param name="user">The User that owns the access tokens.</param>
        /// <returns>Returns a new enumerable list of <see cref="OAuthWorks.IAccessToken"/> objects that belong to the given user.</returns>
        //IEnumerable<T> GetByUser(IUser user);

        /// <summary>
        /// Gets an access token that matches the given token value.
        /// </summary>
        /// <remarks>
        /// This method is used to retrieve tokens from a repository based on the actual token as it would be given by a client.
        /// This allows for custom storage schemes that encode a token Id inside the access token returned to the client since the OAuth 2.0 spec
        /// does not define how access tokens should be stored or retrieved.
        /// 
        /// One method of storing tokens would be to encode the Id of the token at the beginning of the value:
        /// 
        /// <code>
        /// string returnedToken = string.Format("{0}-{1}", id, generatedAccessToken); 
        /// </code>
        /// 
        /// This would then allow quick lookup of the token in a database without comprimizing database security with cleartext tokens.
        /// The token could be stored in a database in seperate values 
        /// </remarks>
        /// <param name="token">The token value as it would be given in a request from a client.</param>
        /// <returns></returns>
        T GetByToken(string token);

        /// <summary>
        /// Gets an access token based on the given user and client.
        /// </summary>
        /// <param name="user">The user that owns access to the token.</param>
        /// <param name="client">The client that has been granted access to the client.</param>
        /// <returns>Returns the token that was granted to the client to access the user's account.</returns>
        T GetByUserAndClient(IUser user, IClient client);
    }
}
