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

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for objects that represent the result of the validation of an access token that was given during a request
    /// to protected resources.
    /// </summary>
    public interface IAuthorizationResult
    {
        /// <summary>
        /// Gets whether or not the authorization was successful and therefore validated.
        /// If true, the given access token/request was valid, otherwise it is not.
        /// </summary>
        /// <returns></returns>
        bool IsValidated
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IAccessToken"/> that represents the validated authorization.
        /// </summary>
        /// <returns></returns>
        IAccessToken Token
        {
            get;
        }

        /// <summary>
        /// Gets a <see cref="IAuthorizationResultDescription"/> object that describes the error that occured.
        /// Can be null if <see cref="IsValidated"/> is true.
        /// </summary>
        /// <returns></returns>
        IAuthorizationResultDescription ErrorDescription
        {
            get;
        }
    }
}