using OAuthWorks.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class AuthorizationCodeRepository : DictionaryRepository<IAuthorizationCode>, IAuthorizationCodeRepository<IAuthorizationCode>
    {
        public AuthorizationCodeRepository()
        {
            IdSelector = c => ((AuthorizationCode)c).Id;
        }

        public IAuthorizationCode GetByValue(string authorizationCode)
        {
            string[] splits = authorizationCode.Split('-');
            return GetById(splits[0]);
        }
    }
}
