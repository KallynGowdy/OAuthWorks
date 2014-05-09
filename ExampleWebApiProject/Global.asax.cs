using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;

[assembly: PreApplicationStartMethod(typeof(ExampleWebApiProject.WebApiApplication), "EnableFormsAuth")]

namespace ExampleWebApiProject
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        
        public static void EnableFormsAuth()
        {
            FormsAuthentication.EnableFormsAuthentication(null);
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
        }
    }
}
