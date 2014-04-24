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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    class Client : IClient, IHasId<string>
    {
        public Client(string secret)
        {
            this.secret = secret;
        }

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public IEnumerable<Uri> RedirectUris
        {
            get;
            set;
        }

        private string secret;

        public bool IsValidRedirectUri(Uri redirectUri)
        {
            return RedirectUris.Any(r => r.Equals(redirectUri));
        }

        public bool MatchesSecret(string secret)
        {
            return this.secret.Equals(secret);
        }

        public bool Equals(IClient other)
        {
            return other != null && other is Client && this.Id.Equals(((Client)other).Id);
        }
    }
}
