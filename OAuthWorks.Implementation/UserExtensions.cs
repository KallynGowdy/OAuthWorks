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

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a static class that contains extension methods for <see cref="IUser"/> objects.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Creates and returns a new <see cref="IValidatedUser"/> object that specifies that the given user's credentials were valid.
        /// </summary>
        /// <param name="user">The user who's credentials were valid.</param>
        /// <returns>Returns a new <see cref="IValidatedUser"/> whose <see cref="IValidatedUser.IsValidated"/> property returns <c>true</c>.</returns>
        public static IValidatedUser AsValidUser(this IUser user)
        {
            return new ValidatedUser(true, user);
        }

        /// <summary>
        /// Creates and returns a new <see cref="IValidatedUser"/> object that specifies that the given user's credentials were invalid.
        /// </summary>
        /// <param name="user">The user who's credentials were invalid.</param>
        /// <returns>Returns a new <see cref="IValidatedUser"/> whose <see cref="IValidatedUser.IsValidated"/> property returns <c>false</c>.</returns>
        public static IValidatedUser AsInvalidUser(this IUser user)
        {
            return new ValidatedUser(false, user);
        }

        /// <summary>
        /// Creates and returns a new <see cref="IValidatedUser"/> object that specifies whether the given user's credentials were valid.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="isValidated">Whether the user's credentials were valid.</param>
        /// <returns>Returns a new <see cref="IValidatedUser"/> whose <see cref="IValidatedUser.IsValidated"/> property returns <paramref name="isValidated"/>.</returns>
        public static IValidatedUser ForTokenRequest(this IUser user, bool isValidated)
        {
            return isValidated ? user.AsValidUser() : user.AsInvalidUser();
        }
    }
}
