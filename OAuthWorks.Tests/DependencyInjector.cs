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

using Ninject;
using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Parameters;
using OAuthWorks.DataAccess.Repositories;
using OAuthWorks.Implementation.Factories;

namespace OAuthWorks.Tests
{
    public class DependencyInjector : IDependencyInjector
    {
        public readonly static IKernel Kernel = new StandardKernel();

        static DependencyInjector()
        {
            Kernel.Bind<IScopeRepository<IScope>>().To<ScopeRepository>();
            Kernel.Bind<IAccessTokenRepository<IAccessToken>>().To<AccessTokenRepository>();
            Kernel.Bind<IAuthorizationCodeRepository<IAuthorizationCode>>().To<AuthorizationCodeRepository>();
            Kernel.Bind<IAuthorizationCodeFactory<IAuthorizationCode>>().To<AuthorizationCodeFactory>();
            Kernel.Bind<IAuthorizationCodeResponseFactory<IAuthorizationCodeResponse, AuthorizationCodeResponseExceptionBase>>().To<AuthorizationCodeResponseFactory>();

            Kernel.Bind(typeof(IAccessTokenResponseFactory)).To<AccessTokenResponseFactory>();

            //kernel.Bind<>().ToConstructor(a => new AccessTokenResponseFactory());

            Kernel.Bind<IAccessTokenFactory<IAccessToken>>().To<AccessTokenFactory>();
            Kernel.Bind<IReadStore<string, IClient>>().To<ClientRepository>();
            Kernel.Bind<IOAuthProvider>().ToConstructor(k => new OAuthProvider(
                 k.Inject<IAccessTokenRepository<IAccessToken>>(),
                 k.Inject<IAuthorizationCodeRepository<IAuthorizationCode>>(),
                 k.Inject<IScopeRepository<IScope>>(),
                 k.Inject<IReadStore<string, IClient>>(),
                 k.Inject<IRefreshTokenRepository<IRefreshToken>>()
                ));
            Kernel.Bind<IRefreshTokenRepository<IRefreshToken>>().To<RefreshTokenRepository>();
        }

        public T GetInstance<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
