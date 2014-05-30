using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;

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
            MvcConfig.Register(RouteTable.Routes);
            SerializeSettings(GlobalConfiguration.Configuration);
        }

        private void SerializeSettings(HttpConfiguration configuration)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            configuration.Formatters.JsonFormatter.SerializerSettings = jsonSettings;
        }
    }
}
