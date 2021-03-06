﻿// Copyright 2014 Kallyn Gowdy
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
using System.Runtime.Serialization;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a list of the possible response types that can be requested from an OAuth Provider.
    /// </summary>
    [DataContract]
    public enum AuthorizationCodeResponseType
    {
        /// <summary>
        /// Defines that the response should be an Authorization Code that the client can exchange for a access token.
        /// </summary>
        [EnumMember(Value = "code")]
        [DataMember(Name = "code")]
        Code
    }
}
