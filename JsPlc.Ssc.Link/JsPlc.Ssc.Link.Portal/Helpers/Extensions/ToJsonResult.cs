using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace JsPlc.Ssc.Link.Portal.Helpers.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static JsonResult ToJsonResult(this HttpResponseMessage response, object jsonPayload, object modelErrors, string customStatusCode)
        {
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new LinkJsonData
                {
                    ModelErrors = modelErrors,
                    JsonObject = jsonPayload,
                    JsonStatusCode = new JsonStatusCode
                    {
                        ApiStatusCode = response.StatusCode,
                        CustomStatusCode = customStatusCode
                    }
                }
            };
        }
    }
}