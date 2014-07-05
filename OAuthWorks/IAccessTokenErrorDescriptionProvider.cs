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
    /// Defines an interface for objects that provide descriptions of errors that occurred during an access token request.
    /// </summary>
    public interface IAccessTokenErrorDescriptionProvider
    {
        /// <summary>
        /// Gets the human-readable description of the specific error.
        /// </summary>
        /// <param name="specificError">The error that the description should be retrived for.</param>
        /// <returns>Returns a string that represents the human-readable description for the given error.</returns>
        string GetDescription(AccessTokenSpecificRequestError specificError);

    }
}