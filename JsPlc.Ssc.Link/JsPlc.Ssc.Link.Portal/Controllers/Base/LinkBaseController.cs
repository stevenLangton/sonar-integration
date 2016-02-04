using System.Net;
using System.Web;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Portal.Models;
using System.Configuration;
using System;
using System.Globalization;
using Microsoft.Ajax.Utilities;

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
        protected static string ServicesBaseUrl = ConfigurationManager.AppSettings["ServicesBaseUrl"];

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
                    CurrentUser.IsLineManager = facade.IsManagerByEmail(User.Identity.Name);
                    CurrentUser.Colleague = facade.GetColleagueByUsername(User.Identity.Name);
                }

                TempData["CurrentUser"] = CurrentUser;
            }
        }

        public bool IsLineManager()
        {
            return CurrentUser != null && CurrentUser.IsLineManager; // for controllers
        }

        public static bool HasMeetingAccess(int meetingId, string colleagueId)
        {
            if (CurrentUser == null || CurrentUser.Colleague == null) return false;

            if (colleagueId.IsNullOrWhiteSpace()) colleagueId = CurrentUser.Colleague.ColleagueId;

            using (var facade = new LinkServiceFacade())
            {
                return facade.HasMeetingAccess(meetingId, colleagueId);
            }
            return false;
        }
        public static bool HasColleagueAccess(string colleagueId, string otherColleagueId)
        {
            if (CurrentUser == null || CurrentUser.Colleague == null) return false;

            if (colleagueId.IsNullOrWhiteSpace()) colleagueId = CurrentUser.Colleague.ColleagueId;

            using (var facade = new LinkServiceFacade())
            {
                return facade.HasColleagueAccess(colleagueId, otherColleagueId);
            }
            return false;
        }

        public static string GetIPAddress()
        {
            try
            {
                return GetIP4Address(System.Web.HttpContext.Current);
            }
            catch (Exception)
            {
                return "-";
            }
        }
        public static string GetIP4Address(HttpContext context)
        {
            string IP4Address = String.Empty;

            if (context != null && context.Request.UserHostAddress != null)
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(context.Request.UserHostAddress))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IP4Address + "=" + IPA;
                        break;
                    }
                }
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IP4Address + "=" + IPA;
                        break;
                    }
                }

                if (IP4Address != String.Empty)
                {
                    return IP4Address;
                }

            }
            return IP4Address;
        }
    }
}