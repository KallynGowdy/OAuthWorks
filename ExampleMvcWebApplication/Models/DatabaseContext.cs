using OAuthWorks.Implementation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExampleMvcWebApplication.Models
{
    /// <summary>
    /// Defines a class that embodies a unit-of-work that is used to access and manipulate a database.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Gets or sets the <see cref="DbSet{User}"/> object that is used to access the users contained in the database.
        /// </summary>
        /// <returns>Returns a <see cref="DbSet{User}"/> object.</returns> 
        public DbSet<User> Users
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Scope}"/> object that is used to access the scopes contained in the database.
        /// </summary>
        /// <returns>Returns a <see cref="DbSet{Scope}"/> object.</returns> 
        public DbSet<Scope> Scopes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{AuthorizationCode}"/> object that is used to access the authorization codes contained in the database.
        /// </summary>
        /// <returns>Returns a <see cref="DbSet{AuthorizationCode}"/> object.</returns> 
        public DbSet<AuthorizationCode> AuthorizationCodes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Client}"/> object that is used to access the clients contained in the database.
        /// </summary>
        /// <returns>Returns a <see cref="DbSet{Client}"/> object.</returns> 
        public DbSet<Client> Clients
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{AccessToken}"/> object that is used to access the access tokens contained in the database.
        /// </summary>
        /// <returns>Returns a <see cref="DbSet{AccessToken}"/> object.</returns> 
        public DbSet<AccessToken> AccessTokens
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{RefreshToken}"/> object that is used to access the refresh tokens contained in the database.
        /// </summary>
        /// <returns>Returns a <see cref="DbSet{RefreshToken}"/> object.</returns> 
        public DbSet<RefreshToken> RefreshTokens
        {
            get;
            set;
        }
    }
}