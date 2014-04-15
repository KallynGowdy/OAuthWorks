using Ninject;
using OAuthWorks.Repositories;
using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Parameters;

namespace OAuthWorks.Tests
{
    public class DependencyInjector : IDependencyInjector
    {
        static IKernel kernel = new StandardKernel();

        static DependencyInjector()
        {
            kernel.Bind<IScopeRepository<IScope>>().To<ScopeRepository>();
            kernel.Bind<IAccessTokenRepository<IAccessToken>>().To<AccessTokenRepository>();
            kernel.Bind<IAuthorizationCodeRepository<IAuthorizationCode>>().To<AuthorizationCodeRepository>();
            kernel.Bind<IAuthorizationCodeFactory<IAuthorizationCode>>().To<AuthorizationCodeFactory>();
            kernel.Bind<IAuthorizationCodeResponseFactory<IAuthorizationCodeResponse, AuthorizationCodeResponseException>>().To<AuthorizationCodeResponseFactory>();

            kernel.Bind(typeof(IAccessTokenResponseFactory<IAccessTokenResponse, OAuthWorks.AccessTokenResponseException>)).To<AccessTokenResponseFactory>();

            //kernel.Bind<>().ToConstructor(a => new AccessTokenResponseFactory());

            kernel.Bind<IAccessTokenFactory<IAccessToken>>().To<AccessTokenFactory>();
            kernel.Bind<IReadStore<string, IClient>>().To<ClientRepository>();
        }

        public T GetInstance<T>()
        {   
            return kernel.Get<T>();
        }
    }
}
