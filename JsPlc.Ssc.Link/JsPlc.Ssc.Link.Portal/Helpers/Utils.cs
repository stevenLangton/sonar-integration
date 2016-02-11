using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Portal.Helpers
{
    public static class Utils
    {
        public static void ClearAllCookies(this HttpContextBase httpContext)
        {
            if (httpContext.Session != null)
            {
                httpContext.Session.Abandon();
            }
            foreach (var cookie in httpContext.Request.Cookies.Cast<string>().ToList())
            {
                httpContext.Response.Cookies.Set(new HttpCookie(cookie)
                {
                    Expires = DateTime.Now.AddYears(-1)
                });
            }
        }
    }
}