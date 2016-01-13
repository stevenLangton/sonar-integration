using JsPlc.Ssc.Link.Portal.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    [Authorize]
    public class ObjectiveController : LinkBaseController
    {
        // GET: Objective
        public ActionResult Index()
        {
            return View();
        }
    }
}