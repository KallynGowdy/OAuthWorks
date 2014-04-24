using ExampleWebApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace ExampleWebApiProject.Controllers
{
    /// <summary>
    /// Defines a controller for a user that can log in.
    /// </summary>
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        public class Account
        {
            public string Username
            {
                get;
                set;
            }

            public string Password
            {
                get;
                set;
            }
        }

        [Route("logIn")]
        [HttpPost]
        public bool LogIn(Account given)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                User user = context.Users.Find(given.Username);

                if (user != null && user.IsValidPassword(given.Password))
                {
                    FormsAuthentication.SetAuthCookie(given.Username, false);
                    return true;
                }
            }
            return false;
        }

        [Route("")]
        [HttpPost]
        public void CreateAccount(Account newUser)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                User user = new User
                {
                    Id = newUser.Username,
                    Password = new HashedValue(newUser.Password)
                };

                context.Users.Add(user);
                context.SaveChanges();
                FormsAuthentication.SetAuthCookie(newUser.Username, false);
            }
        }
    }
}
