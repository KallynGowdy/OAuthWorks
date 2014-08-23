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
using System.Web;

namespace ExampleMvcWebApplication
{
    /// <summary>
    /// Defines a utility class that provides a simple wrapper around an object that provides an easy way to determine whether an object
    /// should be disposed of or not.
    /// </summary>
    /// <typeparam name="T">The type of object wrapped by this object.</typeparam>
    public class DisposableObject<T> : IDisposableObject<T> where T : class, IDisposable
    {
        /// <summary>
        /// Gets whether this object has been disposed already.
        /// </summary>
        /// <returns>Returns <c>true</c> if <see cref="Dispose"/> has already been called on this object, otherwise <c>false</c>.</returns>
        public bool Disposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value that is wrapped by this object.
        /// </summary>
        /// <returns>Returns the value that is wrapped by this object.</returns>
        public T Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets whether this object should dispose of the <see cref="Value"/>.
        /// </summary>
        /// <returns>Returns <c>true</c> if the <see cref="Value"/> should be disposed of, otherwise <c>false</c>.</returns>
        public bool ShouldDisposeValue
        {
            get;
            set;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isItAlsoSafeToFreeManagedObjects)
        {
            if (isItAlsoSafeToFreeManagedObjects && ShouldDisposeValue && !Disposed && this.Value != null)
            {
                Value.Dispose();
                Value = null;
            }
            Disposed = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObject{T}"/> class.
        /// </summary>
        /// <param name="value">The potential value to dispose.</param>
        /// <param name="shouldDispose">Whether this object should dispose the given value when this object is disposed.</param>
        public DisposableObject(T value, bool shouldDispose)
        {
            this.Value = value;
            this.ShouldDisposeValue = shouldDispose;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="T"/> to <see cref="DisposableObject{T}"/>.
        /// </summary>
        /// <param name="value">The value that should be wrapped.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DisposableObject<T>(T value)
        {
            return new DisposableObject<T>(value, true);
        }
    }

    /// <summary>
    /// Defines a class that contains a set of extension methods for <see cref="IDisposable"/> objects.
    /// </summary>
    public static class DisposeableObjectExtensions
    {

        /// <summary>
        /// Gets an <see cref="IDisposableObject{T}"/> that represents the given value.
        /// </summary>
        /// <typeparam name="T">The type of the value represented.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IDisposableObject<T> AsDisposable<T>(this T value) where T : class, IDisposable
        {
            return new DisposableObject<T>(value, true);
        }

    }
}