using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class User : IUser
    {
        public string Id
        {
            get;
            set;
        }

        public bool HasGrantedScope(IClient client, IScope scope)
        {
            return false;
        }
    }
}
