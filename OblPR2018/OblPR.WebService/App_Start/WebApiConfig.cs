using OblPR.Data.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OblPR.WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var cors = new EnableCorsAttribute("", "", "*");
            //config.EnableCors(cors);


            var ip = ConfigurationManager.AppSettings["serverIp"];
            var port = ConfigurationManager.AppSettings["serverPort"];

            var playerManager = (IPlayerManager) Activator.GetObject(
                typeof(IPlayerManager),
                $"tcp://{ip}:{port}/{ServiceNames.PlayerManager}");


            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

    }
}
