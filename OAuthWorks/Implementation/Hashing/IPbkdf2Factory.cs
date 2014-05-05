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

using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Hashing
{
    /// <summary>
    /// Defines an interface for an object that creates new <see cref="IPbkdf2"/> objects.
    /// </summary>
    [ContractClass(typeof(IPbkdf2FactoryContract))]
    public interface IPbkdf2Factory : IFactory<IPbkdf2>
    {
        /// <summary>
        /// Creates a new <see cref="OAuthWorks.Implementation.Hashing.IPbkdf2"/> object using the given password, salt and hash iterations.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt that should be used in the hashing process.</param>
        /// <param name="iterations">The number of iterations that should be used when hashing.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.Implementation.Hashing.IPbkdf2"/> object that creates a derived password(hash) from the given values.</returns>
        IPbkdf2 Create(byte[] password, byte[] salt, int iterations);

        /// <summary>
        /// Creates a new <see cref="OAuthWorks.Implementation.Hashing.IPbkdf2"/> object using the given password, salt length and hash iterations.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="saltLength">The length of the salt to generate.</param>
        /// <param name="iterations">The number of iterations that should be used when hashing.</param>
        /// <returns>Returns a new <see cref="OAuthWorks.Implementation.Hashing.IPbkdf2"/> object that creates a derived password(hash) from the given values.</returns>
        IPbkdf2 Create(byte[] password, int saltLength, int iterations);
    }

    [ContractClassFor(typeof(IPbkdf2Factory))]
    internal abstract class IPbkdf2FactoryContract : IPbkdf2Factory
    {
        IPbkdf2 IPbkdf2Factory.Create(byte[] password, byte[] salt, int iterations)
        {
            Contract.Requires(password != null);
            Contract.Requires(salt != null);
            Contract.Requires(iterations > 0);
            return default(IPbkdf2);
        }

        IPbkdf2 IPbkdf2Factory.Create(byte[] password, int saltLength, int iterations)
        {
            Contract.Requires(password != null);
            Contract.Requires(saltLength > 0);
            Contract.Requires(iterations > 0);
            return default(IPbkdf2);
        }

        IPbkdf2 IFactory<IPbkdf2>.Create()
        {
            return default(IPbkdf2);
        }
    }

}
