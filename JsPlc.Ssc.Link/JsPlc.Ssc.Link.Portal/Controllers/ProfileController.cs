using JsPlc.Ssc.Link.Portal.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class ProfileController : LinkBaseController
    {
        // GET: Profile
        public ActionResult Show(string Id)
        {
            return View();
        }
    }
}
