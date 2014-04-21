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
            IdSelector = t => ((AccessToken)t).Id;
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
