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
    /// Defines a basic list of refreshToken types that are commonly used for access tokens.
    /// </summary>
    public static class AccessTokenTypes
    {
        /// <summary>
        /// Defines that the refreshToken is a "bearer" refreshToken. That is, anyone possesing the refreshToken has access.
        /// </summary>
        /// <remarks>
        /// Often, OAuth providers will require some other form of authentication than just the access refreshToken. Ususally by requiring a client
        /// secret(or password) to be provided. This is the method provided by many different
        /// </remarks>
        public const string Bearer = "bearer";

        /// <summary>
        /// Defines that the refreshToken is a Message Authentication Code.
        /// </summary>
        public const string Mac = "mac";
    }
}
