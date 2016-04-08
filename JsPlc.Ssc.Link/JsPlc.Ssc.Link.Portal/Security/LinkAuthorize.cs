using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JsPlc.Ssc.Link.Portal.Security;
using JsPlc.Ssc.Link.Models;
using System.Net;
using System.Web.Helpers;

namespace JsPlc.Ssc.Link.Portal.Security
{
    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
    {
        public const string HTTP_HEADER_NAME = "x-RequestVerificationToken";

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            //  Only validate POSTs
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                var headerTokenValue = request.Headers[HTTP_HEADER_NAME];

                // Ajax POSTs using jquery have a header set that defines the token.
                // However using unobtrusive ajax the token is still submitted normally in the form.
                // if the header is present then use it, else fall back to processing the form like normal
                if (headerTokenValue != null)
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null
                        ? antiForgeryCookie.Value
                        : null;

                    AntiForgery.Validate(cookieValue, headerTokenValue);
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute()
                        .OnAuthorization(filterContext);
                }
            }
        }
    }

    public class TeamAccessAttribute : LinkAuthorizeBaseAttribute
    {
        //You have team access when you can access your own data and that of your direct reports
        public override bool IsAuthorized(HttpContextBase httpContext)
        {
            //ClaimsPrincipal currentPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            return HasAccess(httpContext);
        }

        //Check if the logged in user has TeamAccess privileage
        private bool HasAccess(HttpContextBase httpContext)
        {
            string EmailAddress = AuthorizationService.GetEmailAddress();

            using (var facade = new LinkServiceFacade())
            {
                ColleagueView Colleague = facade.GetColleagueByUsername(EmailAddress);

                if (Colleague != null)
                {
                    return AuthorizationService.HasTeamAccess(Colleague.ColleagueId, httpContext.Request.QueryString["colleagueid"]);
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
    //End of class TeamAccessAttribute

    public class LinkAuthorizeManagerAttribute: LinkAuthorizeBaseAttribute
    {
        public override bool IsAuthorized(HttpContextBase httpContext)
        {
            return HasDirectReports(httpContext);
        }

        private bool HasDirectReports(HttpContextBase httpContext)
        {
            var userName = httpContext.User.Identity.Name;
            using (var facade = new LinkServiceFacade())
            {
                return facade.IsManagerByEmail(userName);
            }
        }
    }

    // source: https://mycodepad.wordpress.com/2014/05/17/mvc-custom-authorizeattribute-for-custom-authentication/
    [AttributeUsageAttribute( AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true )]
    public abstract class LinkAuthorizeBaseAttribute : AuthorizeAttribute {
         
        //Custom named parameters for annotation
        public abstract bool IsAuthorized(HttpContextBase httpContext);

        //Called when access is denied
        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext ) {
            //User isn't logged in
            if ( !filterContext.HttpContext.User.Identity.IsAuthenticated ) {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new { success = false, message = "You are not authenticated", data = String.Empty }
                    };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", reason = "Login with correct access" }));
                }
            }
            //User is logged in but has no access
            else {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new { success = false, message = "You are not authorized", data = String.Empty }
                    };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", reason = "This login is unauthorized." }));
                }
            }
        }
 
        //Core authentication, called before each action
        protected override bool AuthorizeCore( HttpContextBase httpContext) {
            var b = httpContext.User.Identity.IsAuthenticated;
            //Is user logged in?
            //If user is logged in and we need a custom check:
            return b && IsAuthorized(httpContext);
            //Returns true or false, meaning allow or deny. False will call HandleUnauthorizedRequest above
        }
    }

}