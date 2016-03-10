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
using System.Web.Helpers;
using Elmah;
using JsPlc.Ssc.Link.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

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

        public static void LogElmahInfo(this object obj, string objectContextMessage="")
        {
            try
            {
                if (ConfigurationManager.AppSettings["LogDebugInfo"].ToLower().Equals("true"))
                {
                    string jsonValue = JsonConvert.SerializeObject(obj);
                    string logMsg = jsonValue;

                    if (!objectContextMessage.IsNullOrWhiteSpace())
                    {
                        logMsg = "##ObjContext:" + objectContextMessage + "\n" + jsonValue;
                    }
                    LogElmahInfo(logMsg);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new Exception("LogElmahInfo extension method failed..")));
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
            }
        }
        public static void LogElmahInfo(string stringValue)
        {
            try
            {
                if (ConfigurationManager.AppSettings["LogDebugInfo"].ToLower().Equals("true"))
                {
                    ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new Exception(stringValue)));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new Exception("LogElmahInfo failed..")));
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
            }
        }

    }
}