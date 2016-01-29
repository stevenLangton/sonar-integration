using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    [Authorize]
    public class ProfileController : LinkBaseController
    {
        // GET: Profile
        public ActionResult Show([Bind(Prefix = "id")] string ColleagueId)
        {
            using (var facade = new LinkServiceFacade())
            {
                ColleagueView ColleagueDetails = facade.GetColleague(ColleagueId);
                LinkMeeting NextMeetingView = facade.GetNextMeeting(ColleagueId);
                ViewBag.Colleague = ColleagueDetails;
                ViewBag.NextMeeting = NextMeetingView;
                return View();
            }
        }
    }
}
