using ExampleWebApiProject.Models;
using OAuthWorks;
using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleWebApiProject.Factories
{
    public class AuthorizationCodeFactory : IAuthorizationCodeFactory<AuthorizationCode>
    {
        public ICreatedToken<AuthorizationCode> Create(Uri redirectUri, IUser user, IClient client, IEnumerable<IScope> scopes)
        {
            string code = new Random().Next(int.MaxValue).ToString();
            return new CreatedToken<AuthorizationCode>(new AuthorizationCode
            {
                RedirectUri = redirectUri.ToString(),
                User = (User)user,
                Client = (Client)client,
                Scopes = scopes.Cast<Scope>().ToList(),
                ExpirationDateUtc = DateTime.UtcNow.Add(new TimeSpan(2, 0, 0)),
                Code = code
            }, code);
        }

        public AuthorizationCode Create()
        {
            throw new NotImplementedException();
        }
    }
}