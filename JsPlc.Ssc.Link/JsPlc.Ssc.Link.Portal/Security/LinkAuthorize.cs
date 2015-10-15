using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JsPlc.Ssc.Link.Portal.Security
{
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
                return facade.IsManager(userName);
            }
            //if (userName.ToLower().Contains("sandip"))
            //{
            //    // Query LinkRepo to see if any direct reports
            //    // call API to get HasDirectReports (dont use DB directly)
            //    return true;
            //}
            //return false;
        }
    }

    // source: https://mycodepad.wordpress.com/2014/05/17/mvc-custom-authorizeattribute-for-custom-authentication/
    [AttributeUsageAttribute( AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true )]
    public abstract class LinkAuthorizeBaseAttribute : AuthorizeAttribute {
         
        //Custom named parameters for annotation
        public abstract bool IsAuthorized(HttpContextBase httpContext);

        //Called when access is denied
        protected override void HandleUnauthorizedRequest( AuthorizationContext filterContext ) {
            //User isn't logged in
            if ( !filterContext.HttpContext.User.Identity.IsAuthenticated ) {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary( new { controller = "Account", action = "Login", reason = "Login with correct access"  } )
                );
            }
            //User is logged in but has no access
            else {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login", reason = "This login is unauthorized." })
                );
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