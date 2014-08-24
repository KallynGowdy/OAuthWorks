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
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="IValidatedUser"/>.
    /// </summary>
    public class ValidatedUser : IValidatedUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedUser"/> class.
        /// </summary>
        /// <param name="isValidated">Whether the user's credentials have been validated.</param>
        /// <param name="user">The user whose credentials have been validated (either successfully or not).</param>
        public ValidatedUser(bool isValidated, IUser user)
        {
            this.IsValidated = isValidated;
            this.User = user;
        }

        /// <summary>
        /// Gets whether the user is validated.
        /// </summary>
        /// <returns></returns>
        public bool IsValidated
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user that was being validated.
        /// </summary>
        /// <returns></returns>
        public IUser User
        {
            get;
            private set;
        }
    }
}