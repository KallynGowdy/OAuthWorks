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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a generic interface for an object that is identifiable.
    /// </summary>
    [ContractClass(typeof(IHasIdContract<>))]
    public interface IHasId<T>
    {
        /// <summary>
        /// Gets the Id of this object.
        /// </summary>
        T Id
        {
            get;
        }
    }

    [ContractClassFor(typeof(IHasId<>))]
    internal abstract class IHasIdContract<T> : IHasId<T>
    {
        T IHasId<T>.Id
        {
            get
            {
                Contract.Ensures(Contract.Result<T>() != null);
                return default(T);
            }
        }
    }
}
