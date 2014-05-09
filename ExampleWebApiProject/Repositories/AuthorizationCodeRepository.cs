using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAuthWorks;
using ExampleWebApiProject.Models;
using OAuthWorks.DataAccess.Repositories;

namespace ExampleWebApiProject.Repositories
{
    public class AuthorizationCodeRepository : IAuthorizationCodeRepository<IAuthorizationCode>
    {
        DatabaseContext context;

        public AuthorizationCodeRepository()
        {
            context = new DatabaseContext();
        }

        public AuthorizationCodeRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public IAuthorizationCode GetByValue(string authorizationCode)
        {
            return context.AuthorizationCodes.Find(authorizationCode);
        }

        public IAuthorizationCode GetById(string id)
        {
            return context.AuthorizationCodes.Find(id);
        }

        public void Add(IAuthorizationCode obj)
        {
            AuthorizationCode code = (AuthorizationCode)obj;
            context.Users.Attach(code.User);
            context.AuthorizationCodes.Add(code);
            context.SaveChanges();
        }

        public void Update(IAuthorizationCode obj)
        {
            context.AuthorizationCodes.Attach((AuthorizationCode)obj);
            context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void RemoveById(string id)
        {
            var code = context.AuthorizationCodes.Find(id);
            context.AuthorizationCodes.Remove(code);
            context.SaveChanges();
        }

        public IEnumerator<IAuthorizationCode> GetEnumerator()
        {
            return context.AuthorizationCodes.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        ~AuthorizationCodeRepository()
        {
            Dispose(false);
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);   
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }


        public IEnumerable<IAuthorizationCode> GetByUserAndClient(IUser user, IClient client)
        {
            return context.AuthorizationCodes.Where(c => c.User.Id.Equals(user.Id) && c.Client.Equals(client));
        }
    }
}