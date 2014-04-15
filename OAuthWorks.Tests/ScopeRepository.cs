using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuthWorks.Repositories;
namespace OAuthWorks.Tests
{
    class ScopeRepository : DictionaryRepository<string, Scope>, IScopeRepository<Scope>
    {
        public ScopeRepository()
        {
            IdSelector = s => s.Id;
        }

        public IEnumerable<Scope> GetAllScopes()
        {
            return Entities;
        }

        public IEnumerable<Scope> GetByToken(IAccessToken token)
        {
            return null;
        }

        public IEnumerator<Scope> GetEnumerator()
        {
            return base.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }
}
