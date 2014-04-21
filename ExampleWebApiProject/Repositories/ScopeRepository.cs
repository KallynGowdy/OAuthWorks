using ExampleWebApiProject.Models;
using OAuthWorks.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleWebApiProject.Repositories
{
    public class ScopeRepository : IScopeRepository<Scope>
    {
        DatabaseContext context = new DatabaseContext();
        
        public IEnumerable<Scope> GetAllScopes()
        {
            return context.Scopes.AsEnumerable();
        }

        public IEnumerable<Scope> GetByToken(OAuthWorks.IAccessToken token)
        {
            return null;
        }

        public Scope GetById(string id)
        {
            return context.Scopes.Find(id);
        }

        public IEnumerator<Scope> GetEnumerator()
        {
            return context.Scopes.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal void Add(OAuthWorks.IScope scope)
        {
            context.Scopes.Add((Scope)scope);
            context.SaveChanges();
        }

        ~ScopeRepository()
        {
            Dispose(false);
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);   
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public ScopeRepository(DatabaseContext context)
        {
            // TODO: Complete member initialization
            this.context = context;
        }
    }
}