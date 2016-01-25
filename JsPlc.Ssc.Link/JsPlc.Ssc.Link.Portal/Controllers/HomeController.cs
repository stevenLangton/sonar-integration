using System.Web.Http;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Portal.Controllers.Base;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class HomeController : LinkBaseController
    {
        public ActionResult Index()
        {
            return View("Welcome");
        }

        [System.Web.Mvc.Authorize]
        public ActionResult LinkForm()
        {
            // for now, may this should show any Ongoing(InProgress) LinkForm or NewLinkForm if no ongoing exists.

            ViewBag.Message = "Link Meeting Form for - " + User.Identity.Name;

            return View();
        }

        [System.Web.Mvc.Authorize]
        public ActionResult LinkReport()
        {
            //ViewBag.Message = "Link Report for - " + CurrentUser.Colleague.FirstName + " " + CurrentUser.Colleague.LastName;
            ViewBag.Message = "Link Report for - " + User.Identity.Name;
            return MyMeetings();

            //return View();
        }

        // GET: /Team/LinkMeetings/My
        // ## COLLEAGUE meetings view
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Authorize]
        public ActionResult MyMeetings()
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    RedirectToAction("Index", "Home");
            //}
            TempData["tabName"] = "report";
            TempData["ViewType"] = "MyMeetings";
            return View("../Team/LinkMeetings");
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Team()
        {
            ViewBag.Message = "Team for - " + User.Identity.Name;

            return View();
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