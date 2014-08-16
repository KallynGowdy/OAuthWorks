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

using ExampleMvcWebApplication.Models;
using OAuthWorks.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleMvcWebApplication.Repositories
{
    /// <summary>
    /// Defines a repository that provides access to <see cref="Scope" /> objects.
    /// </summary>
    public class ScopeRepository : IScopeRepository<Scope>
    {
        /// <summary>
        /// The <see cref="IDisposableObject{DatabaseContext}"/> that this repository uses to access the database.
        /// It's an <see cref="IDisposableObject{DatabaseContext}"/> to allow for passed-in <see cref="DatabaseContext"/> objects to be not disposed by this
        /// object.
        /// </summary>
        IDisposableObject<DatabaseContext> context;

        /// <summary>
        /// Whether this repository has been disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeRepository"/> class.
        /// </summary>
        public ScopeRepository() : this(new DisposableObject<DatabaseContext>(new DatabaseContext(), shouldDispose: true))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeRepository"/> class.
        /// </summary>
        /// <param name="context">The context that this repository should use, will not be disposed.</param>
        public ScopeRepository(DatabaseContext context) : this(new DisposableObject<DatabaseContext>(context, shouldDispose: false))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// context.Value
        /// </exception>
        public ScopeRepository(IDisposableObject<DatabaseContext> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.Value == null)
            {
                throw new ArgumentNullException("context.Value");
            }
            this.context = context;
        }

        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> that this repository is using.
        /// </summary>
        /// <returns></returns>
        public DatabaseContext Context
        {
            get
            {
                ThrowIfDisposed();
                return context.Value;
            }
        }

        /// <summary>
        /// Gets all of the scopes contained in the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Scope> GetAllScopes()
        {
            ThrowIfDisposed();
            return Context.Scopes.AsEnumerable();
        }

        /// <summary>
        /// Gets a list of all of the scopes that are provided by the given access refresh token.
        /// </summary>
        /// <param name="refreshToken">The refreshToken to get all of the scopes for.</param>
        /// <returns>Returns an enumerable list of the scopes that are provided by the given refreshToken.</returns>
        public IEnumerable<Scope> GetByToken(OAuthWorks.IAccessToken token)
        {
            ThrowIfDisposed();
            return Context.Scopes.Where(s => s.ReferencedAccessTokens.Contains(token));
        }

        /// <summary>
        /// Gets a <see cref="Scope"/> object from the database using the given identifier.
        /// </summary>
        /// <param name="id">The identifier that the <see cref="Scope"/> should be retrieved with.</param>
        /// <returns></returns>
        public Scope GetById(string id)
        {
            ThrowIfDisposed();
            return Context.Scopes.Find(id);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Scope> GetEnumerator()
        {
            ThrowIfDisposed();
            return Context.Scopes.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            ThrowIfDisposed();
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds the given scope to this repository.
        /// </summary>
        /// <param name="scope">The scope that should be added to the repository.</param>
        internal void Add(OAuthWorks.IScope scope)
        {
            ThrowIfDisposed();
            Context.Scopes.Add((Scope)scope);
            Context.SaveChanges();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ScopeRepository"/> class.
        /// </summary>
        ~ScopeRepository()
        {
            Dispose(false);
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
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (context != null)
                    {
                        context.Dispose();
                        context = null;
                    }
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if this object is disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}