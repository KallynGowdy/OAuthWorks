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
using ExampleWebApiProject.Models;
using OAuthWorks.DataAccess.Repositories;

namespace ExampleWebApiProject.Repositories
{
    public class AuthorizationCodeRepository : IAuthorizationCodeRepository<IAuthorizationCode>
    {
        DatabaseContext context;

        public AuthorizationCodeRepository()
        {
            context = new DatabaseContext();
        }

        public AuthorizationCodeRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public IAuthorizationCode GetByValue(string authorizationCode)
        {
            return context.AuthorizationCodes.Find(authorizationCode);
        }

        public IAuthorizationCode GetById(string id)
        {
            return context.AuthorizationCodes.Find(id);
        }


        ~AuthorizationCodeRepository()
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


        public IEnumerable<IAuthorizationCode> GetByUserAndClient(IUser user, IClient client)
        {
            return context.AuthorizationCodes.Where(c => c.User.Id.Equals(user.Id) && c.Client.Name.Equals(client.Name));
        }

        public void Add(ICreatedToken<IAuthorizationCode> authorizationCode)
        {
            context.AuthorizationCodes.Add(new AuthorizationCode(authorizationCode));
        }
    }
}