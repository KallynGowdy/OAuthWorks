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
    /// Defines an interface for an object that contains values for a response to an unsuccessful access token request.
    /// </summary>
    public interface IUnsuccessfulAccessTokenResponse : IAccessTokenResponse
    {
        /// <summary>
        /// Gets the error code that describes what was wrong with the request.
        /// </summary>
        /// <returns>Returns a <see cref="AccessTokenRequestError"/> object that describes what was wrong with the request.</returns>
        AccessTokenRequestError Error
        {
            get;
        }

        /// <summary>
        /// Gets the human-readable ASCII text providing additional information, used to assist the client developer in understanding the error that occurred.
        /// </summary>
        /// <returns>Returns a <see cref="string"/> that represents the human-readable text.</returns>
        string ErrorDescription
        {
            get;
        }

        /// <summary>
        /// Gets a uri identifying a human-readable web page with information about the error, used to provide the client developer with additional information about
        /// the error.
        /// </summary>
        /// <returns>Returns a <see cref="Uri"/> that represents the linked web page.</returns>
        Uri ErrorUri
        {
            get;
        }
    }
}
