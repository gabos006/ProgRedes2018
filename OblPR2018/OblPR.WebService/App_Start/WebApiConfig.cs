using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OblPR.WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());



            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

    }
}
