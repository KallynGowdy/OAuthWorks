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
using OAuthWorks;
using OAuthWorks.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace ExampleMvcWebApplication.Repositories
{
    /// <summary>
    /// Defines a repository that provides access to <see cref="IClient"/> objects.
    /// </summary>
    public class ClientRepository : IRepository<IClient>
    {
        /// <summary>
        /// The <see cref="IDisposableObject{DatabaseContext}"/> that this repository uses to access the database.
        /// It's an <see cref="IDisposableObject{DatabaseContext}"/> to allow for passed-in <see cref="DatabaseContext"/> objects to be not disposed by this
        /// object.
        /// </summary>
        IDisposableObject<DatabaseContext> context;

        /// <summary>
        /// Whether this object is disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository"/> class.
        /// </summary>
        public ClientRepository() : this(new DisposableObject<DatabaseContext>(new DatabaseContext(), shouldDispose: true))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository"/> class.
        /// </summary>
        /// <param name="context">The context that this repository should use to access the database, will not be disposed with this object.</param>
        public ClientRepository(DatabaseContext context) : this(new DisposableObject<DatabaseContext>(context, shouldDispose: false))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository"/> class.
        /// </summary>
        /// <param name="context">The context that this repository should use to access the database.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public ClientRepository(IDisposableObject<DatabaseContext> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.context = context;
        }

        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> that this repository uses for database access.
        /// </summary>
        /// <returns></returns>
        public DatabaseContext Context
        {
            get
            {                 
                return context.Value;
            }
        }

        /// <summary>
        /// Gets a <see cref="IClient"/> object from the database using the given identifier.
        /// </summary>
        /// <param name="id">The identifier that the <see cref="IClient"/> should be retrieved with.</param>
        /// <returns></returns>
        public IClient GetById(string id)
        {
            return Context.Clients.Find(id);
        }

        /// <summary>
        /// Adds the given object to the repository.
        /// </summary>
        /// <param name="obj">The object to add to the respository.</param>
        public void Add(IClient obj)
        {
            string str = ((IObjectContextAdapter)Context).ObjectContext.Connection.ConnectionString;
            Context.Clients.Add((Client)obj);
            Context.SaveChanges();
        }

        /// <summary>
        /// Updates the given object in the repository.
        /// </summary>
        /// <param name="obj">The object that should be updated.</param>
        public void Update(IClient obj)
        {
            Context.Clients.Attach((Client)obj);
            Context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            Context.SaveChanges();
        }

        /// <summary>
        /// Removes the authorization code with the given id from the repository.
        /// </summary>
        /// <param name="id">The Id of the object to remove.</param>
        public void RemoveById(string id)
        {
            var client = Context.Clients.Find(id);
            Context.Clients.Remove(client);
            Context.SaveChanges();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IClient> GetEnumerator()
        {
            return Context.Clients.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Context.Clients.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ClientRepository"/> class.
        /// </summary>
        ~ClientRepository()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                    if(context != null)
                    {
                        context.Dispose();
                        context = null;
                    }
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if this object has been disposed.
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