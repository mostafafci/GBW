using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace GBW
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //// Web API configuration and services
            //// Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //// Web API routes
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{action}/{Id}",
             defaults: new { Id = RouteParameter.Optional });
            // Web API configuration and services
            //var enableCorsAttribute = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(enableCorsAttribute);
            //config.EnableCors(new EnableCorsAttribute("*", "*", "GET,POST,PUT,DELETE,OPTIONS"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //var cors = new EnableCorsAttribute("*", "accept,accesstoken,authorization,cache-control,pragma,content-type,origin", "GET,PUT,POST,DELETE,TRACE,HEAD,OPTIONS");
            //config.EnableCors(cors);
        }
    }
}
