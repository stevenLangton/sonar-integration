using System.Web.Http;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Portal.Controllers.Base;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class HomeController : LinkBaseController
    {
        public ActionResult Index()
        {
            TempData["IPAddress"] = GetIPAddress();
            return View("Welcome");
        }

        // GET: /Home/LinkMeetings/
        // ## COLLEAGUE's meetings
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Authorize]
        public ActionResult LinkMeetings()
        {
            TempData["tabName"] = "meetings";
            TempData["ViewType"] = "MyMeetings";
            return View("LinkMeetings");
        }

        public ActionResult Contact()
        {
            TempData["tabName"] = "Contact";
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Welcome()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult JsVars()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Unauthorized([FromUri]string unauthPage="")
        {
            ViewBag.unauthPage = unauthPage;
            return View();
        }
    }
}