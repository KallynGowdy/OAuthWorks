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


using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    [TestFixture]
    public class OAuthProviderTests
    {
        OAuthProvider provider;
        DependencyInjector dependencyInjector = new DependencyInjector();

        Scope exampleScope;

        Client client;

        [TestFixtureSetUp]
        public void SetUp()
        {
            provider = new OAuthProvider(dependencyInjector)
            {
                ScopeParser = (p, s) => s.Split(' ', '-').Select(a => p.ScopeRepository.GetById(a)),
                ScopeFormatter = (s) => string.Join(" ", s)
            };

            exampleScope = new Scope
            {
                Id = "exampleScope",
                Description = "The first and only scope that can be requested"
            };

            ((ScopeRepository)provider.ScopeRepository).Add(exampleScope);

            client = new Client("secret")
            {
                Id = "bob",
                Name = "Client",
                RedirectUris = new Uri[]
                {
                    new Uri("http://example.com/oauth/response/token")
                }
            };

            ((ClientRepository)provider.ClientRepository).Add(client);
        }


        [Test]
        public void TestOAuthProvider()
        {
            User user = new User
            {
                Id = "Id"
            };

            //A request from the client for an authorization code.
            AuthorizationCodeRequest codeRequest = new AuthorizationCodeRequest
            {
                ClientId = "bob",
                ClientSecret = "secret",
                RedirectUri = new Uri("http://example.com/oauth/response/token"),
                ResponseType = AuthorizationCodeResponseType.Code,
                Scope = "exampleScope",
                State = "state"
            };

            //Normally you would authorize the client and user here.
            //You would also determine if the user needs to provide consent using
            //provider.HasAccess(user, client, scope)

            //Then retrieve the 
            IAuthorizationCodeResponse response = provider.RequestAuthorizationCode(codeRequest, user);

            Debug.Assert(response != null);

            AccessTokenRequest tokenRequest = new AccessTokenRequest
            {
                ClientId = "bob",
                ClientSecret = "secret",
                RedirectUri = new Uri("http://example.com/oauth/response/token"),
                AuthorizationCode = response.Code
            };

            var tokenResponse = provider.RequestAccessToken(tokenRequest, user);

            Debug.Assert(tokenResponse != null);

            Debug.Assert(provider.HasAccess(user, client, exampleScope));

            Client otherClient = new Client("otherSecret")
            {
                Id = "evilClient",
                Name = "BadGuy",
                RedirectUris = new Uri[]
                {
                    new Uri("http://sealinfosite.com/oauth/giveme/token")
                }
            };

            ((ClientRepository)provider.ClientRepository).Add(otherClient);

            Debug.Assert(!provider.HasAccess(user, otherClient, exampleScope));
        }

    }
}
