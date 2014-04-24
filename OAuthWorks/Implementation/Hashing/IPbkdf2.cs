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

namespace OAuthWorks.Implementation.Hashing
{
    /// <summary>
    /// Defines an interface for an object that implements PBKDF2 (RFC 2898).
    /// </summary>
    public interface IPbkdf2 : IDisposable
    {
        /// <summary>
        /// Gets the salt that is used for hashing.
        /// </summary>
        byte[] Salt
        {
            get;
        }

        /// <summary>
        /// Gets the number of iterations used in creating the derived hash.
        /// </summary>
        int Iterations
        {
            get;
        }

        /// <summary>
        /// Returns the pseudo-random key for this object.
        /// </summary>
        /// <param name="length">The number of pseudo-random bytes to return.</param>
        /// <returns></returns>
        byte[] GetBytes(int length);
    }
}
