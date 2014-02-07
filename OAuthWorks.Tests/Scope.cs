using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class Scope : IScope
    {
        public string Id
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        

        public bool Equals(IScope other)
        {
            return other != null && other is Scope && ((Scope)other).Id == this.Id;
        }
    }
}
