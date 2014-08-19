using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AddressFinderAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "searchActionApi",
                routeTemplate: "api/{controller}/search/{*terms}"
            );
            config.Routes.MapHttpRoute(
                name: "searchTopActionApi",
                routeTemplate: "api/{controller}/searchtop/{top}/{*terms}"
            );
        }
    }
}
