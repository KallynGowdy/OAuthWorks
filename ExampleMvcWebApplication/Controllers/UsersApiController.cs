using ExampleMvcWebApplication.Models;
using ExampleMvcWebApplication.ViewModels;
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

        /// <summary>
        /// Creates a new account with the specified username and password.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IHttpActionResult> PostCreateAccount(string id, Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.Username)
            {
                return BadRequest();
            }

            User user = new User
            {
                Id = id,
                Password = new HashedValue(account.Password)
            };

            Context.Users.Add(user);
            await Context.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { controller = "users", id }, account);
        }

        [Route("{id}")]
        [OAuthAuthorize("all", "updateAccount")]     
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

            if (User != null)
            {
                User.Password = new HashedValue(account.Password);
                User.Id = account.Username;
                Context.Entry(User).State = System.Data.Entity.EntityState.Modified;
                await Context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
