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
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface that contains several values that define OAuth provider properties such as endpoints and supported scopes.
    /// </summary>
    public interface IOAuthProviderDefinition
    {
        /// <summary>
        /// Gets the scopes that can be requested by a third party client.
        /// Note that excluding a scope from this list does not prevent it from being requested and given,
        /// it just prevents it from being advertized.
        /// </summary>
        IEnumerable<IScope> Scopes
        {
            get;
        }

    }
}