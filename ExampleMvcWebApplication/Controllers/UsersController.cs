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

using ExampleMvcWebApplication.Models;
using ExampleMvcWebApplication.ViewModels;
using Newtonsoft.Json;
using OAuthWorks;
using OAuthWorks.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ExampleMvcWebApplication.Controllers
{
    /// <summary>
    /// Defines a <see cref="Controller"/> that provides actions for users.
    /// </summary>
    public class UsersController : Controller
    {
        // GET: /Users/LogIn
        public ActionResult LogIn(string @return = null)
        {
            return View(@return);
        }

        public ActionResult CreateAccount(string @return = null)
        {
            return View(@return);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateAccount(Account account, string @return = null)
        {
            using(UsersApiController controller = new UsersApiController())
            {
                controller.PutUpdateAccount(account.Username, account).Wait();
                return Redirect(@return ?? Request.UrlReferrer.AbsolutePath);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> LogIn(string username, string password, string @return = null)
        {
            OAuthApiController.CredentialsTokenRequest request = new OAuthApiController.CredentialsTokenRequest
            {
                ClientId = "ExampleClient",
                ClientSecret = "notasecret",
                GrantType = "password",
                Password = password,
                Username = username,
                RedirectUri = new Uri("https://localhost43301/users/login"),
                Scope = "all"
            };

            using (OAuthApiController api = new OAuthApiController())
            {
                var response = api.RequestAccessToken(request);
                var token = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), new { access_token = "", refresh_token = "" });
                if (token != null)
                {
                    Response.Cookies.Add(new HttpCookie("auth_token", token.access_token) { HttpOnly = true });
                    Response.Cookies.Add(new HttpCookie("refresh_token", token.refresh_token) { HttpOnly = true });
                }
            }

            return Redirect(@return ?? Request.UrlReferrer.AbsolutePath);
        }
    }
}