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
using OAuthWorks.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleWebApiProject.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository<IRefreshToken>
    {
        DatabaseContext context;

        public RefreshTokenRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void Remove(IRefreshToken token)
        {
            IHasId<string> id = token as IHasId<string>;
            if (id != null)
            {
                context.RefreshTokens.Remove(context.RefreshTokens.Find(id.Id));
            }
        }

        public IEnumerable<IRefreshToken> GetByUserAndClient(IUser user, IClient client)
        {
            return context.RefreshTokens.Where(t => t.User.Equals(user) && t.Client.Equals(client));
        }

        public IRefreshToken GetByValue(string token)
        {
            return context.RefreshTokens.Find(token.Split('-').Last());
        }

        public IRefreshToken GetById(string id)
        {
            return context.RefreshTokens.Find(id);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void Add(IRefreshToken obj)
        {
            context.RefreshTokens.Add(new ExampleWebApiProject.Models.RefreshToken((HashedRefreshToken)obj));
        }

        public void Update(IRefreshToken obj)
        {
            context.RefreshTokens.Attach(new ExampleWebApiProject.Models.RefreshToken((HashedRefreshToken)obj));
            context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void RemoveById(string id)
        {
            context.RefreshTokens.Remove(context.RefreshTokens.Find(id));
        }

        public IEnumerator<IRefreshToken> GetEnumerator()
        {
            return context.RefreshTokens.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}