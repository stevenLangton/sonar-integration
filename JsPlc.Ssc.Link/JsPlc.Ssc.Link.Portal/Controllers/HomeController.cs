using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Welcome");
        }

        public ActionResult LinkForm()
        {
            ViewBag.Message = "Link Meeting Form.";

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