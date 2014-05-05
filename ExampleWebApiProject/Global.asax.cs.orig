using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
<<<<<<< Updated upstream
=======
using System.Web.Security;

[assembly: PreApplicationStartMethod(typeof(ExampleWebApiProject.WebApiApplication), "EnableFormsAuth")]
>>>>>>> Stashed changes

namespace ExampleWebApiProject
{
    public class WebApiApplication : System.Web.HttpApplication
    {
<<<<<<< Updated upstream
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
=======
        
        public static void EnableFormsAuth()
        {
            FormsAuthentication.EnableFormsAuthentication(null);
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
>>>>>>> Stashed changes
        }
    }
}
