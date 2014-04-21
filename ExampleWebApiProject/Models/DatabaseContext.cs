using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExampleWebApiProject.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users
        {
            get;
            set;
        }

        public DbSet<Scope> Scopes
        {
            get;
            set;
        }

        public DbSet<AuthorizationCode> AuthorizationCodes
        {
            get;
            set;
        }

        public DbSet<Client> Clients
        {
            get;
            set;
        }
    }
}