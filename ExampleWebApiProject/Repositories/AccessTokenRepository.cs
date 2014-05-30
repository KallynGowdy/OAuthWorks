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
using OAuthWorks.Implementation.Factories;

namespace ExampleWebApiProject.Repositories
{
    public class AccessTokenRepository : IAccessTokenRepository<IAccessToken>
    {
        private DatabaseContext context;

        private IValueIdFormatter idFormatter = AccessTokenFactory.DefaultIdFormatter;

        public AccessTokenRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void Remove(IAccessToken token)
        {
            IHasId<string> id = token as IHasId<string>;
            if (id != null)
            {
                context.AccessTokens.Remove(context.AccessTokens.Find(id.Id));
            }
        }

        public IAccessToken GetByToken(string token)
        {
            return context.AccessTokens.Find(idFormatter.GetId(token));
        }

        public IEnumerable<IAccessToken> GetByUserAndClient(IUser user, IClient client)
        {
            return context.AccessTokens.Where(t => t.User.Id.Equals(user.Id) && t.Client.Name.Equals(client.Name));
        }

        public IAccessToken GetById(string id)
        {
            return context.AccessTokens.Find(id);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void Add(ICreatedToken<IAccessToken> token)
        {
            context.AccessTokens.Add(new Models.AccessToken(token));
        }
    }
}