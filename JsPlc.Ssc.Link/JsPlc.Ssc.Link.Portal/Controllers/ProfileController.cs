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
        public ProfileController() { }
        public ProfileController(ILinkUserView CurrentUser, ILinkServiceFacade Facade)
            : base(CurrentUser, Facade)
        {
            
        }

        // GET: Profile
        public ActionResult Show([Bind(Prefix = "id")] string ColleagueId)
        {
            ColleagueView ColleagueDetails = ServiceFacade.GetColleague(ColleagueId);
            LinkMeeting NextMeetingView = ServiceFacade.GetNextMeeting(ColleagueId);
            ViewBag.Title = ColleagueDetails.FirstName + " " + ColleagueDetails.LastName;
            ViewBag.Colleague = ColleagueDetails;
            ViewBag.NextMeeting = NextMeetingView;

            ColleagueTeamView ColleagueMeetings = ServiceFacade.GetMyMeetingsView(ColleagueId) ?? new ColleagueTeamView();
            ColleagueMeetings = TeamController.AssignMeetingsByDate(ColleagueMeetings);

            ViewBag.ColleagueMeetings = ColleagueMeetings;

            return View();
        }
    }
}
