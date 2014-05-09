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