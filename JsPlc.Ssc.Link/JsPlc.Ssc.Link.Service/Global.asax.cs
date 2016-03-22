using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using JsPlc.Ssc.Link.Repository;
using System;
using System.IO;
using System.Web.Http.Filters;
using log4net;

namespace JsPlc.Ssc.Link.Service
{
    public class WebApiApplication : HttpApplication
    {
		protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
			GlobalConfiguration.Configuration.Filters.Add(new GlobalActionExecutedExceptionFilterAttribute());


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new RepositoryInitializer());

			log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }
    }

	public class GlobalActionExecutedExceptionFilterAttribute : ExceptionFilterAttribute
	{
		private static readonly ILog log = LogManager.GetLogger("GlobalActionExecutedEx");

		public override void OnException(HttpActionExecutedContext context)
		{
			log.Error(context.Exception);
		}
	}
}
