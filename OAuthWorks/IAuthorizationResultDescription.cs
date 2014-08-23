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
    /// Defines an interface for objects that contain descriptive information about what occurred in a request that caused the given result.
    /// </summary>
    public interface IAuthorizationResultDescription
    {
        /// <summary>
        /// Gets the error code that was returned by the server.
        /// </summary>
        /// <returns></returns>
        string Error
        {
            get;
        }

        /// <summary>
        /// Gets the human-readable description of the error.
        /// </summary>
        /// <returns></returns>
        string ErrorDescription
        {
            get;
        }

        /// <summary>
        /// Gets the uri that points to a web page that provides a description of the error.
        /// </summary>
        /// <returns></returns>
        string ErrorUri
        {
            get;
        }
    }
}