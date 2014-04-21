using ExampleWebApiProject.Models;
using OAuthWorks;
using OAuthWorks.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace ExampleWebApiProject.Repositories
{
    public class ClientRepository : IRepository<IClient>
    {
        DatabaseContext context = new DatabaseContext();

        public IClient GetById(string id)
        {
            return context.Clients.Find(id);
        }

        public void Add(IClient obj)
        {
            string str = ((IObjectContextAdapter)context).ObjectContext.Connection.ConnectionString;
            context.Clients.Add((Client)obj);
            context.SaveChanges();
        }

        public void Update(IClient obj)
        {
            context.Clients.Attach((Client)obj);
            context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void RemoveById(string id)
        {
            var client = context.Clients.Find(id);
            context.Clients.Remove(client);
            context.SaveChanges();
        }

        public IEnumerator<IClient> GetEnumerator()
        {
            return context.Clients.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return context.Clients.AsEnumerable().GetEnumerator();
        }

        ~ClientRepository()
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

        public ClientRepository(DatabaseContext context)
        {
            this.context = context;
        }
    }
}