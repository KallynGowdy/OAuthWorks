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

namespace ExampleMvcWebApplication
{
    /// <summary>
    /// Defines an interface utility that provides a simple wrapper around an object that provides an easy way to determine whether an object
    /// should be disposed of or not.
    /// </summary>
    /// <typeparam name="T">The type of object wrapped by this object.</typeparam>
    public interface IDisposableObject<T> : IDisposable where T : class, IDisposable
    {
        /// <summary>
        /// Gets whether this object has been disposed already.
        /// </summary>
        /// <returns>Returns <c>true</c> if <see cref="Dispose"/> has already been called on this object, otherwise <c>false</c>.</returns>
        bool Disposed { get; }

        /// <summary>
        /// Gets or sets whether this object should dispose of the <see cref="Value"/>.
        /// </summary>
        /// <returns>Returns <c>true</c> if the <see cref="Value"/> should be disposed of, otherwise <c>false</c>.</returns>
        bool ShouldDisposeValue { get; set; }

        /// <summary>
        /// Gets the value that is wrapped by this object.
        /// </summary>
        /// <returns>Returns the value that is wrapped by this object.</returns>
        T Value { get; }
    }
}