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
using Elmah;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Controllers;
using JsPlc.Ssc.Link.Portal.ModelBinding;

namespace JsPlc.Ssc.Link.Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        [ExcludeFromCodeCoverage]
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterApiRoutes(new HttpConfiguration());
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
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
            HttpContext context;
            try
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new Exception("Global error handler fired..")));

                var app = sender as MvcApplication;
                if (app == null) return;

                context = app.Context;
                var ex = app.Server.GetLastError();

                context.Response.Clear();
                context.ClearError();
                var httpException = ex as HttpException;

                if (ex != null) ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new Exception(ex.Message)));
                
                var routeData = new RouteData();
                routeData.Values["controller"] = "errors";
                routeData.Values["exception"] = ex;
                routeData.Values["action"] = "http500";
                if (httpException != null)
                {
                    ErrorLog.GetDefault(context).Log(new Error(httpException));
                    switch (httpException.GetHttpCode())
                    {
                        case 404:
                            routeData.Values["action"] = "http404";
                            break;
                        case 403:
                            routeData.Values["action"] = "http403";
                            break;
                        case 500:
                            routeData.Values["action"] = "http500";
                            break;
                    }
                }
                IController controller = new ErrorsController();
                controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
            }
            catch (Exception ex)
            {
                var app = sender as MvcApplication;
                if (app != null)
                {
                    context = app.Context;
                    ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new Exception("Global error handler FAILED..")));
                    ErrorLog.GetDefault(context).Log(new Error(ex));
                }
            }
        }
    }
}
