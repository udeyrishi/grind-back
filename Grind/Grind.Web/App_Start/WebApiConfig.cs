using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Grind.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
