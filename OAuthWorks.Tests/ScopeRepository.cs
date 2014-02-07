using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuthWorks.Repositories;
namespace OAuthWorks.Tests
{
    class ScopeRepository : DictionaryRepository<string, IScope>, IScopeRepository<IScope>
    {
        public ScopeRepository()
        {
            IdSelector = s => s.Id;
        }

        public IEnumerable<IScope> GetAllScopes()
        {
            return Entities;
        }

        public IEnumerable<IScope> GetByToken(IAccessToken token)
        {
            return null;
        }
    }
}
