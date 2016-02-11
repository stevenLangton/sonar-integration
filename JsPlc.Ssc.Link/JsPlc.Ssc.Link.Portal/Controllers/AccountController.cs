using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Security.Claims;
using System.Configuration;
using System.Globalization;

// The following using statements were added for this sample.
using System.Web.Routing;
using System.Web.UI.WebControls;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Cookies;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using JsPlc.Ssc.Link.Portal.Helpers;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
 
    public class AccountController : LinkBaseController
    {
        // NOT USED
        public ActionResult Login(string reason)
        {
            RedirectToRouteResult result=null;
            if (Request.IsAuthenticated)
            {
                result = new RedirectToRouteResult(new RouteValueDictionary(
                    new RouteValueDictionary(
                        new {controller = "Home", action = "Unauthorized", reason = "This login is unauthorized."})));
            }
            //else
            //{
            //    result = new RedirectToRouteResult(new RouteValueDictionary(
            //        new RouteValueDictionary(
            //            new { controller = "Account", action = "SignIn"})));
            //}
            return result;
        } 


        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/", IsPersistent = true}, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void OwinSignIn()
        {
            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { RedirectUri = "/", IsPersistent = true});
        }

        public void SignOut()
        {
            // Remove all cache entries for this user and send an OpenID Connect sign-out request.
            var userObj = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");
            if (userObj == null)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
                return;
            }

            string userObjectId = userObj.Value;
            AuthenticationContext authContext = new AuthenticationContext(Startup.Authority, new NaiveSessionCache(userObjectId));
            authContext.TokenCache.Clear();

            HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        //public void SignIn()
        //{
        //    // Send an OpenID Connect sign-in request.
        //    if (!Request.IsAuthenticated)
        //    {
        //        HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
        //    }
        //}
        //public void SignOut()
        //{
        //    // Send an OpenID Connect sign-out request.
        //    HttpContext.GetOwinContext().Authentication.SignOut(
        //        OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        //}

    }
}