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
    class AuthorizationCodeRepository : DictionaryRepository<IAuthorizationCode>, IAuthorizationCodeRepository
    {
        public AuthorizationCodeRepository()
        {
            IdSelector = c => ((IHasId<string>)c).Id;
        }

        public IAuthorizationCode GetByValue(string authorizationCode)
        {
            string[] splits = authorizationCode.Split('-');
            return GetById(splits.Last());
        }


        public IEnumerable<IAuthorizationCode> GetByUserAndClient(IUser user, IClient client)
        {
            return Entities.Where(t => t.Client.Equals(client) && t.User.Equals(user));
        }

        public void Add(ICreatedToken<IAuthorizationCode> authorizationCode)
        {
            base.Add(authorizationCode.Token);
        }
    }
}
