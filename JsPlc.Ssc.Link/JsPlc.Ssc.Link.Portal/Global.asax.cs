using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Controllers;
using JsPlc.Ssc.Link.Portal.ModelBinding;
using System.IO;
using log4net;
using System.Web.Helpers;
using System.Security.Claims;

namespace JsPlc.Ssc.Link.Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog _logger = LogManager.GetLogger("GlobalActionExecutedEx");

        [ExcludeFromCodeCoverage]
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterApiRoutes(new HttpConfiguration());
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

            // App specific model binding.
            // BindersConfig.RegisterModelBinders(); //not needed for now.. kept for future.
        }
        /// <summary>
        /// Error handler: fires when httpErrors has existingResponse="PassThrough" in web.config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            _logger.Error(Server.GetLastError());
        }
    }
}
