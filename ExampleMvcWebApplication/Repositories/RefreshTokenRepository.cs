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
using OAuthWorks.Implementation;
using OAuthWorks.Implementation.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleMvcWebApplication.Repositories
{
    /// <summary>
    /// Defines a repository that provides access to <see cref="IRefreshToken"/> objects.
    /// </summary>
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        /// <summary>
        /// The <see cref="IDisposableObject{DatabaseContext}"/> that this repository uses to access the database.
        /// It's an <see cref="IDisposableObject{DatabaseContext}"/> to allow for passed-in <see cref="DatabaseContext"/> objects to be not disposed by this
        /// object.
        /// </summary>
        IDisposableObject<DatabaseContext> context;

        /// <summary>
        /// The <see cref="IValueIdFormatter{string}"/> that this repository uses to retrieving the ID from a token.
        /// </summary>
        IValueIdFormatter<string> valueIdFormatter = RefreshTokenFactory.String.DefaultIdFormatter;

        /// <summary>
        /// Whether this object has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> that this repository is using.
        /// </summary>
        /// <returns>Returns the <see cref="DatabaseContext"/> object that this repository is using.</returns>
        public DatabaseContext Context
        {
            get
            {
                ThrowIfDisposed();
                return context.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
        /// </summary>
        public RefreshTokenRepository() : this(new DisposableObject<DatabaseContext>(new DatabaseContext(), shouldDispose: true))
        {
        }

        /// <summary>
        /// Adds the given refresh token to the repository.
        /// </summary>
        /// <param name="refreshToken">The refresh token that should be added to the repository.</param>
        public void Add(ICreatedToken<IRefreshToken> refreshToken)
        {
            Context.RefreshTokens.Add(new Models.RefreshToken(refreshToken));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="DatabaseContext"/> that this repository should use, will not be disposed with this object.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public RefreshTokenRepository(DatabaseContext context) : this(new DisposableObject<DatabaseContext>(context, shouldDispose: false))
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
        /// </summary>
        /// <param name="context">The context that this repository should use.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public RefreshTokenRepository(IDisposableObject<DatabaseContext> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.context = context;
        }

        /// <summary>
        /// Removes(deletes) the given refreshToken from this repository.
        /// </summary>
        /// <param name="refreshToken">The refresh token to remove from the repository.</param>
        public void Remove(IRefreshToken token)
        {
            ThrowIfDisposed();
            IHasId<string> id = token as IHasId<string>;
            if (id != null)
            {
                Context.RefreshTokens.Remove(Context.RefreshTokens.Find(id.Id));
            }
        }

        /// <summary>
        /// Gets the refresh refreshToken that can be used by the given client to retrive access tokens for the given user's account.
        /// </summary>
        /// <remarks>
        /// Note that while only one refresh token *should* be active at a time, the usage of this method by <see cref="OAuthWorks.OAuthProvider"/>
        /// requires that all refresh tokens be retrievable, that way unused/old tokens will be able to be easily destroyed.
        /// </remarks>
        /// <param name="user">The user that owns the accout that the refreshToken gives access to.</param>
        /// <param name="client">The client that maintains possesion of the refresh refreshToken.</param>
        /// <returns>Returns the refresh tokens that can be used by the given client for the given user's account if one exists. Otherwise returns null.</returns>
        public IEnumerable<IRefreshToken> GetByUserAndClient(IUser user, IClient client)
        {
            ThrowIfDisposed();
            return Context.RefreshTokens.Where(t => t.User.Id == user.Id && t.Client.Name == client.Name);
        }

        /// <summary>
        /// Gets a refresh refreshToken by the value that was given to the client.
        /// </summary>
        /// <param name="refreshToken">The refresh token that was issued to the client.</param>
        /// <returns>Returns the refresh token that belongs to the given value.</returns>
        public IRefreshToken GetByValue(string token)
        {
            ThrowIfDisposed();
            return Context.RefreshTokens.Find(token.Split('-').Last());
        }

        /// <summary>
        /// Gets a <see cref="IRefreshToken"/> object from the database using the given identifier.
        /// </summary>
        /// <param name="id">The identifier that the <see cref="IRefreshToken"/> should be retrieved with.</param>
        /// <returns></returns>
        public IRefreshToken GetById(string id)
        {
            ThrowIfDisposed();
            return Context.RefreshTokens.Find(id);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RefreshTokenRepository"/> class.
        /// </summary>
        ~RefreshTokenRepository()
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
                if (!disposing)
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