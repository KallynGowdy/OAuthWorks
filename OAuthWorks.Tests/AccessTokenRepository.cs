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

using OAuthWorks.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class AccessTokenRepository : DictionaryRepository<IAccessToken>, IAccessTokenRepository<IAccessToken>
    {
        public AccessTokenRepository()
        {
            IdSelector = t => ((IHasId<string>)t).Id;
        }

        public IAccessToken GetByToken(string token)
        {
            string[] split = token.Split('-');
            return this.GetById(split[0]);
        }

        public IAccessToken GetByUserAndClient(IUser user, IClient client)
        {
            return Entities.SingleOrDefault(a => a.Client.Equals(client) && a.User.Equals(user));
        }
    }
}
