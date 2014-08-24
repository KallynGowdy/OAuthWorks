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

using System.Collections.Generic;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for objects that represent a request/requirement for authorization of an access token in order to
    /// access a protected resource.
    /// </summary>
    public interface IAuthorizationRequest
    {
        /// <summary>
        /// Gets the value that was given in the 'Authorization' header of the request.
        /// </summary>
        /// <returns></returns>
        string Authorization
        {
            get;
        }

        /// <summary>
        /// Gets the authorization type that was given in the 'Authorization' header of the request.
        /// </summary>
        /// <returns></returns>
        string Type
        {
            get;
        }

        /// <summary>
        /// Gets the list of scope groups that are required for the request to be authorized.
        /// If the request contains all of the required scopes in at least one of the groups, then
        /// the request is authorized. For Example: With the two scope groups {"all", "viewUser, updateUser"},
        /// if the given authorization has access to either the "all" scope or both the "viewUser" and "updateUser"
        /// scopes, then it is allowed.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IEnumerable<IScope>> RequiredScopes
        {
            get;
        }
    }
}