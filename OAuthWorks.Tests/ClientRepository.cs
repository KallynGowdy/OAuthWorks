using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class ClientRepository : DictionaryRepository<string, IClient>
    {
        public ClientRepository()
        {
            IdSelector = c => ((Client)c).Id;
        }
    }
}
