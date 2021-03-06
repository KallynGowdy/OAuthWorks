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
    /// Defines an interface for objects that provide descriptions of specific problems that occurred in the OAuth 2.0 flow.
    /// </summary>
    public interface IAuthorizationCodeErrorDescriptionProvider
    {
        /// <summary>
        /// Gets a human-readable description of the given generic error and specific error.
        /// </summary>
        /// <param name="genericError">The generic type of the error that occurred.</param>
        /// <param name="specificError">The specific cause of the error that occurred.</param>
        /// <returns>Returns a human-readable string that describes the problem that occurred.</returns>
        string GetDescription(AuthorizationCodeRequestSpecificErrorType specificError);

    }
}