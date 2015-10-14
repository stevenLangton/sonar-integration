using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Portal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JsPlc.Ssc.Link.Portal.Controllers.Base
{
    public class LinkBaseController : Controller
    {
        public UserView CurrentUser; // careful as this maybe null when not logged in

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (User == null) return;

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var appUser = manager.FindById(User.Identity.GetUserId());
            if (appUser!= null)
            {
                CurrentUser = appUser.ToUserView();
            }

            TempData["CurrentUser"] = CurrentUser; // for UserView object for Views
  
        }

        //// Anything to do before the view finally renders.
        //protected override void OnActionExecuted(ActionExecutedContext filterContext)

        public bool IsLineManager()
        {
            return CurrentUser != null && CurrentUser.IsLineManager; // for controllers
        }

    }
}