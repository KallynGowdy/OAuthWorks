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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAuthWorks.Implementation.Factories;

namespace ExampleMvcWebApplication.Repositories
{
    /// <summary>
    /// Defines an example access token repository that provides the excange mecanizm between our <see cref="DatabaseContext"/> and an <see cref="OAuthProvider"/>.
    /// </summary>
    public class AccessTokenRepository : IAccessTokenRepository
    {
        /// <summary>
        /// The <see cref="IDisposableObject{DatabaseContext}"/> that this repository uses to access the database.
        /// It's an <see cref="IDisposableObject{DatabaseContext}"/> to allow for passed-in <see cref="DatabaseContext"/> objects to be not disposed by this
        /// object.
        /// </summary>
        private DatabaseContext context;

        /// <summary>
        /// The <see cref="IValueIdFormatter"/> used to parse given access tokens into their respective parts.
        /// </summary>
        private IValueIdFormatter<string> idFormatter = AccessTokenFactory.String.DefaultIdFormatter;

        /// <summary>
        /// Whether this object has already been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public AccessTokenRepository(DatabaseContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="idFormatter">The identifier formatter.</param>
        /// <exception cref="System.ArgumentNullException">idFormatter</exception>
        public AccessTokenRepository(DatabaseContext context, IValueIdFormatter<string> idFormatter) : this(context)
        {
            if (idFormatter == null)
            {
                throw new ArgumentNullException("idFormatter");
            }
            this.idFormatter = idFormatter;
        }

        #region IAccessTokenRepository Implementation

        /// <summary>
        /// Adds the given token to this repository.
        /// </summary>
        /// <param name="token">The token to add.</param>
        public void Add(ICreatedToken<IAccessToken> token)
        {
            ThrowIfDisposed();
            context.AccessTokens.Add(new AccessToken(token));
        }

        /// <summary>
        /// Removes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        public void Remove(IAccessToken token)
        {
            ThrowIfDisposed();
            IHasId<string> id = token as IHasId<string>;
            if (id != null)
            {
                context.AccessTokens.Remove(context.AccessTokens.Find(id.Id));
            }
        }

        /// <summary>
        /// Gets the token by it's issued value.
        /// </summary>
        /// <param name="token">The value that was issued to the client.</param>
        /// <returns>Returns the token that is accociated with that value.</returns>
        public IAccessToken GetByToken(string token)
        {
            ThrowIfDisposed();
            return context.AccessTokens.Find(idFormatter.GetId(token));
        }

        /// <summary>
        /// Gets a list of access tokens by the given user and client.
        /// </summary>
        /// <param name="user">The user that the tokens belong to.</param>
        /// <param name="client">The client that the tokens were issued to.</param>
        /// <returns>Returns an enumerable list of tokens that belong to the given user and were issued to the given client.</returns>
        public IEnumerable<IAccessToken> GetByUserAndClient(IUser user, IClient client)
        {
            ThrowIfDisposed();
            return context.AccessTokens.Where(t => t.User.Id.Equals(user.Id) && t.Client.Name.Equals(client.Name));
        }

        /// <summary>
        /// Gets the <see cref="IAccessToken"/> object from the database that has the given identifier.
        /// </summary>
        /// <param name="id">The identifier of the token to retrieve.</param>
        /// <returns>Returns a <see cref="IAccessToken"/> that represents the retrieved token.</returns>
        public IAccessToken GetById(string id)
        {
            ThrowIfDisposed();
            return context.AccessTokens.Find(id);
        } 
        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="AccessTokenRepository"/> class.
        /// </summary>
        ~AccessTokenRepository()
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
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if this object has already been disposed.
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