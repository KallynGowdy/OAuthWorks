using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuthWorks.DataAccess.Repositories;
namespace OAuthWorks.Tests
{
    class ScopeRepository : DictionaryRepository<Scope>, IScopeRepository<Scope>
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }
}
