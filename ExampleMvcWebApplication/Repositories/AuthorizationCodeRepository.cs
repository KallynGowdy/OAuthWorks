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
using OAuthWorks;
using ExampleMvcWebApplication.Models;
using OAuthWorks.DataAccess.Repositories;
using OAuthWorks.Implementation.Factories;
using ExampleMvcWebApplication;

namespace ExampleMvcWebApplication.Repositories
{
    /// <summary>
    /// Defines a class that provides an example implementation of <see cref="IAuthorizationCodeRepository{IAuthorizationCode}" />.
    /// </summary>
    public class AuthorizationCodeRepository : IAuthorizationCodeRepository
    {
        /// <summary>
        /// The <see cref="IDisposableObject{DatabaseContext}"/> that this repository uses to access the database.
        /// It's an <see cref="IDisposableObject{DatabaseContext}"/> to allow for passed-in <see cref="DatabaseContext"/> objects to be not disposed by this
        /// object.
        /// </summary>
        IDisposableObject<DatabaseContext> context;

        /// <summary>
        /// The <see cref="IValueIdFormatter{string}"/> that this repository uses to retrieve the ID from an authorization code value.
        /// </summary>
        private IValueIdFormatter<string> valueIdFormatter = AuthorizationCodeFactory.String.DefaultIdFormatter;

        /// <summary>
        /// Whether this object has been disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> that this repository uses.
        /// </summary>
        /// <returns>Returns the <see cref="DatabaseContext"/> object that this repository uses.</returns>
        public DatabaseContext Context
        {
            get
            {
                ThrowIfDisposed();
                return context.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeRepository"/> class.
        /// </summary>
        public AuthorizationCodeRepository() : this(new DisposableObject<DatabaseContext>(new DatabaseContext(), shouldDispose: true))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeRepository" /> class.
        /// </summary>
        /// <param name="context">The context that this <see cref="AuthorizationCodeRepository" /> should use.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public AuthorizationCodeRepository(DatabaseContext context) : this(new DisposableObject<DatabaseContext>(context, shouldDispose: false))
        {
            if(context == null)
            {
                throw new ArgumentNullException("context");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeRepository"/> class.
        /// </summary>
        /// <param name="context">The context that this repository should use.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public AuthorizationCodeRepository(IDisposableObject<DatabaseContext> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.context = context;
        }

        /// <summary>
        /// Gets the authorization code by it's actual internal value.
        /// </summary>
        /// <param name="authorizationCode">The authorization code that was issued to a client.</param>
        /// <returns>Returns the complete authorization code.</returns>
        /// <remarks>
        /// This method is used to allow flexibility to implementations in how they store Ids. 
        /// Authorization code factories can return authorization codes that contain Ids in them for easy retrieval.
        /// It is also possible to use different possiblities.
        /// </remarks>
        public IAuthorizationCode GetByValue(string authorizationCode)
        {
            ThrowIfDisposed();
            return GetById(valueIdFormatter.GetId(authorizationCode));
        }

        /// <summary>
        /// Gets a <see cref="IAuthorizationCode"/> object from the database by the given identifier.
        /// </summary>
        /// <param name="id">The identifier that the <see cref="IAuthorizationCode"/> should be retrieved with.</param>
        /// <returns></returns>
        public IAuthorizationCode GetById(string id)
        {
            ThrowIfDisposed();
            return Context.AuthorizationCodes.Find(id);
        }

        /// <summary>
        /// Gets a list of <see cref="IAuthorizationCode"/> objects that were granted to the given client by the given user.
        /// </summary>
        /// <param name="user">The user that granted the client access to the codes.</param>
        /// <param name="client">The client that the codes were granted to.</param>
        /// <returns>
        /// Returns a list of <see cref="OAuthWorks.IAuthorizationCode" /> objects that belong to the user and were granted to the client.
        /// </returns>
        public IEnumerable<IAuthorizationCode> GetByUserAndClient(IUser user, IClient client)
        {
            ThrowIfDisposed();
            return Context.AuthorizationCodes.Where(c => c.User.Id.Equals(user.Id) && c.Client.Name.Equals(client.Name));
        }

        /// <summary>
        /// Adds the given authorization code to this repository.
        /// </summary>
        /// <param name="authorizationCode">The authorization code to add.</param>
        public void Add(ICreatedToken<IAuthorizationCode> authorizationCode)
        {
            ThrowIfDisposed();
            Context.AuthorizationCodes.Add(new AuthorizationCode(authorizationCode));
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AuthorizationCodeRepository"/> class.
        /// </summary>
        ~AuthorizationCodeRepository()
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
                    if (context != null)
                    {
                        context.Dispose();
                        context = null;
                    }
                    if(valueIdFormatter != null)
                    {
                        valueIdFormatter = null;
                    }
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if this object has been disposed.
        /// </summary>
        protected virtual void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}