using System.Web.Http;
using Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Hosting;
using System;
using System.Configuration;

namespace IntegregracionBalanzaApiLocal
{

    public class Webserver
    {
        private IDisposable _webapp;
        public void Start()
        {
            try
            {
                _webapp = WebApp.Start<Startup>(ConfigurationManager.AppSettings["server"]);
            }
            catch (Exception ex)
            {

            }

        }

        public void Stop()
        {
            _webapp?.Dispose();
        }

    }


    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}
