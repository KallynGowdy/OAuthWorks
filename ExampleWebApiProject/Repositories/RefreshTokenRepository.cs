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

        public void Add(ICreatedToken<IRefreshToken> refreshToken)
        {
            context.RefreshTokens.Add(new Models.RefreshToken(refreshToken));
        }
    }
}