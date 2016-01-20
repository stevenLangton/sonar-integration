using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Portal.Models;
using System.Configuration;
using System;
using System.Globalization;

namespace JsPlc.Ssc.Link.Portal.Controllers.Base
{
    public class LinkBaseController : Controller
    {
        protected static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        protected static string appKey = ConfigurationManager.AppSettings["ida:AppKey"];
        protected static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        protected static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        protected static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        protected static string LinkApiResourceId = ConfigurationManager.AppSettings["LinkApiResourceId"];
        protected static string LinkApiBaseAddress = ConfigurationManager.AppSettings["LinkApiBaseAddress"];

        protected static readonly string Authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        public static LinkUserView CurrentUser { get; private set; } // careful as this maybe null when not logged in

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Request.IsAuthenticated)
            {
                CurrentUser = new LinkUserView();

                using (var facade = new LinkServiceFacade())
                {
                    CurrentUser.IsLineManager = facade.IsManager(User.Identity.Name);
                    CurrentUser.Colleague = facade.GetColleagueByUsername(User.Identity.Name);
                }

                TempData["CurrentUser"] = CurrentUser;
            }
        }

        public bool IsLineManager()
        {
            return CurrentUser != null && CurrentUser.IsLineManager; // for controllers
        }

    }
}