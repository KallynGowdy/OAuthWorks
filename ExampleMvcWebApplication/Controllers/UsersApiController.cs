using ExampleMvcWebApplication.Models;
using ExampleMvcWebApplication.ViewModels;
using OAuthWorks;
using OAuthWorks.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExampleMvcWebApplication.Controllers
{
    [RoutePrefix("api/users")]

    public class UsersApiController : BaseApiController
    {
        public UsersApiController() { }

        [Route("{id}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> PutUpdateAccount(string id, Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.Username)
            {
                return BadRequest();
            }
            User u = Context.Users.Find(id);

            if (u != null)
            {
                IAuthorizationResult authorization = ValidateAuthorization("all", "updateAccount");

                if (authorization.IsValidated)
                {
                    await UpdateAccount(id, account);
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, authorization.ErrorDescription);
                }
            }
            else
            {
                u = await CreateAccount(id, account);
                return CreatedAtRoute("DefaultApi", new { controller = "users", id }, new { UserId = u.Id });
            }
        }

        private async Task UpdateAccount(string id, Account account)
        {
            User.Password = new HashedValue(account.Password);
            User.Id = account.Username;
            Context.Entry(User).State = System.Data.Entity.EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        private async Task<User> CreateAccount(string id, Account account)
        {
            User user = new User
            {
                Id = id,
                Password = new HashedValue(account.Password)
            };
            Context.Users.Add(user);
            await Context.SaveChangesAsync();
            return user;
        }
    }
}
