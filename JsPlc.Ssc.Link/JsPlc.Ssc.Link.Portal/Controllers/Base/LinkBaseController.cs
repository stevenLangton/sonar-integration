using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using iTextSharp.text;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Portal.Models;
using System.Configuration;
using System;
using System.Globalization;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Asn1;
using JsPlc.Ssc.Link.Portal.Properties;

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

		ILinkUserView _currentUser = null;
		public ILinkUserView CurrentUser
		{
			get
			{
				if (_currentUser == null)
				{
					initCurrentUser();
				}
				return _currentUser;
			}
		}
        protected ILinkServiceFacade ServiceFacade { get; set; }

        public LinkBaseController()
        {
            ServiceFacade = new LinkServiceFacade();
        }

        public LinkBaseController(ILinkUserView currentUser, ILinkServiceFacade facade)
        {
			_currentUser = currentUser;
            ServiceFacade = facade;
        }

        //protected override void OnAuthentication(AuthenticationContext filterContext)
        //{
        //    base.OnAuthentication(filterContext);
        //    // http://stackoverflow.com/questions/12568426/mvc3-windows-authentication-override-user-identity
        //    //if (Request.IsAuthenticated)
        //    //{
        //    //    filterContext.HttpContext.User = new ClaimsPrincipal(User);
        //    //}
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.VersionNumber = GetAssemblyVersion();

            if (!initCurrentUser())
            {
                filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Unauthorized.cshtml" };
            }
        }

        [ExcludeFromCodeCoverage]
        protected List<string> GetAllClaims(IIdentity userIdentity)
        {
            var retval = new List<string>();
            try
            {
                var claimIdentity = userIdentity as ClaimsIdentity;
                if (claimIdentity != null)
                {
                    var claims = claimIdentity.Claims;
                    var claimsList = claims as IList<Claim> ?? claims.ToList();
                    retval.AddRange(claimsList.Select(claim => string.Format("Type: {0}, Value: {1}", claim.Type, claim.Value)));
                }
            }
            catch (Exception)
            {
                // suppress all errors
            }
            return retval;
        }

        public bool IsLineManager()
        {
            return CurrentUser != null && CurrentUser.IsLineManager; // for controllers
        }

		public bool HasMeetingAccess(int meetingId)
        {
			if (CurrentUser == null
				|| CurrentUser.Colleague == null
				|| CurrentUser.Colleague.ColleagueId.IsNullOrWhiteSpace()) return false;

			return ServiceFacade.HasMeetingAccess(meetingId, CurrentUser.Colleague.ColleagueId);
        }

		public bool HasColleagueAccess(string otherColleagueId)
        {
			if (CurrentUser == null
				|| CurrentUser.Colleague == null
				|| CurrentUser.Colleague.ColleagueId.IsNullOrWhiteSpace()) return false;

			return ServiceFacade.HasColleagueAccess(CurrentUser.Colleague.ColleagueId, otherColleagueId);
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

        [ExcludeFromCodeCoverage]
        public string GetAssemblyVersion()
        {
            // Test version
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            string versionNumber = currentAssembly.GetName().Version.ToString();
            return versionNumber;
        }

		#region Private Methods
		private bool initCurrentUser()
		{
            bool success = true;

			if (Request.IsAuthenticated && !Request.RawUrl.ToLower().Contains("/account/signout"))
			{
				_currentUser = new LinkUserView();

				//var authenticatedEmailAddr = User.Identity.Name;
				var emailClaim =
					ClaimsPrincipal.Current.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
				var authenticatedEmailAddr = "";
				if (emailClaim != null)
				{
					authenticatedEmailAddr = emailClaim.Value;
				}

				if (authenticatedEmailAddr.IsNullOrWhiteSpace())
				{
					authenticatedEmailAddr = User.Identity.Name;
				}
				// Last ditch check
				if (authenticatedEmailAddr.IsNullOrWhiteSpace())
				{
					var exceptionMsg = "";
					if (emailClaim == null || emailClaim.Value.IsNullOrWhiteSpace())
					{
						exceptionMsg += "Error: Missing claim = http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
					}
					if (User.Identity.Name.IsNullOrWhiteSpace())
					{
						exceptionMsg += "\n --- Error: Username/Email expected on authentication, Found blank email address claim value";
					}

                    success = false;
				}

				_currentUser.IsLineManager = ServiceFacade.IsManagerByEmail(authenticatedEmailAddr);
				_currentUser.Colleague = ServiceFacade.GetColleagueByUsername(authenticatedEmailAddr);

				if (_currentUser.Colleague == null)
				{
					var exceptionMessage = string.Format("Colleague not found. App Name: {0}; AD Email: {1}", Resources.AppName, authenticatedEmailAddr);
                    success = false;
				}
				else if (_currentUser.Colleague.EmailAddress.IsNullOrWhiteSpace())
				{
					var exceptionMessage = string.Format("Colleague's Email not found. App Name: {0}; AD Email: {1}", Resources.AppName, authenticatedEmailAddr);
                    success = false;
				}
                else if (_currentUser.Colleague.Manager==null)
                {
                    var exceptionMessage = string.Format("Colleague's manager not found. App Name: {0}; AD Email: {1}", Resources.AppName, authenticatedEmailAddr);
                    success = false;
                }
				TempData["CurrentUser"] = _currentUser;
			}

            return success;
		}


		#endregion

        protected JsonResult MakeJsonObject(dynamic DataObject, bool Success = true, string Message = "")
        {
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { success = Success, message = Message, data = DataObject }
            };
        }
    }
}