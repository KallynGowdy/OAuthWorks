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