using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.ModelBinding;

namespace JsPlc.Ssc.Link.Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterApiRoutes(new HttpConfiguration());
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            // App specific model binding.
            //BindersConfig.RegisterModelBinders(); not needed for now.. kept for future.
        }
    }
}
