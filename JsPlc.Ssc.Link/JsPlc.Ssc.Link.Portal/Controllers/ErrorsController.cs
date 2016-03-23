using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using JsPlc.Ssc.Link.Portal.Properties;
using Microsoft.Owin.Logging;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Http404()
        {
            Response.StatusCode = 404;
            return Content("Error 404 occured", "text/plain");
        }

        public ActionResult Http500()
        {
            Response.StatusCode = 500;
            return Content("Error 500 occured", "text/plain");
        }

        public ActionResult Http403()
        {
            Response.StatusCode = 403;
            return Content("Error 403 occured", "text/plain");
        }
    }
}