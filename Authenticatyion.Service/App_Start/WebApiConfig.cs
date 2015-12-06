using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Authentication.Service
{
    public static class WebApiConfig
    {
        private const string ApiVersion = "v1";

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: ApiVersion + "/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
