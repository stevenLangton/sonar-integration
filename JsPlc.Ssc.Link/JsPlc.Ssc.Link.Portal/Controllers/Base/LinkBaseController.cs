using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Portal.Models;

namespace JsPlc.Ssc.Link.Portal.Controllers.Base
{
    public class LinkBaseController : Controller
    {
        public static LinkUserView CurrentUser { get; private set; } // careful as this maybe null when not logged in

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Request.IsAuthenticated)
            {
                CurrentUser = new LinkUserView();

                using (var facade = new LinkServiceFacade())
                {
                    CurrentUser.IsLineManager = facade.IsManager(User.Identity.Name);
                    CurrentUser.Colleague = facade.GetColleagueByUsername(User.Identity.Name);
                }

                TempData["CurrentUser"] = CurrentUser; // for UserView object for Views
            }
        }

        public bool IsLineManager()
        {
            return CurrentUser != null && CurrentUser.IsLineManager; // for controllers
        }

    }
}