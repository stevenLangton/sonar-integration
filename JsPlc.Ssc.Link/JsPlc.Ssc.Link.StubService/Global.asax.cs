using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using JsPlc.Ssc.Link.StubService.StubRepository;
using JsPlc.Ssc.Link.StubService.Filters;
using System.Web.Http.Filters;
using log4net;
using System.IO;

namespace JsPlc.Ssc.Link.StubService
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
			GlobalConfiguration.Configuration.Filters.Add(new GlobalActionExecutedExceptionFilter());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer(new StubRepositoryInitializer());

			log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }
    }
}
