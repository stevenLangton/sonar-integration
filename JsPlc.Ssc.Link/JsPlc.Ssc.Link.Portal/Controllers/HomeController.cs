using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Welcome");
        }

        [Authorize]
        public ActionResult LinkForm()
        {
            // for now, may this should show any Ongoing(InProgress) LinkForm or NewLinkForm if no ongoing exists.

            ViewBag.Message = "Link Meeting Form for - " + User.Identity.GetUserName();

            return View();
        }

        [Authorize]
        public ActionResult LinkReport()
        {
            ViewBag.Message = "Link Report for - " + User.Identity.GetUserName();

            return View();
        }

        [Authorize]
        public ActionResult Team()
        {
            ViewBag.Message = "Team for - " + User.Identity.GetUserName();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Welcome()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}