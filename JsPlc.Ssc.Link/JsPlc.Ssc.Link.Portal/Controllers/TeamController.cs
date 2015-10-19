using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Services;
using iTextSharp.text;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using JsPlc.Ssc.Link.Portal.Security;
using Org.BouncyCastle.Asn1.Crmf;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class TeamController : LinkBaseController
    {
        // ## Knockout VIEWS - Sprint 2: 
        // Team/LinkMeetings.cshtml and Team/_ColleagueMeetingHistory.cshtml

        // ## Mockups:
        // /LinkForm/MyTeam.cshtml and Linkform/_ListMeetingsPartial.cshtml

        [ScriptMethod(UseHttpGet = true)]
        [HttpGet]
        [Authorize]
        //[LinkAuthorizeManager] // IMPORTANT Check removed as this method is common for LinkMeetings view
        public JsonResult GetMeetings(string myOrTeams) // Another method in LinkForm for MyMeetings
        {
            object jsonData;

            using (var facade = new LinkServiceFacade())
            {
                List<TeamView> teamMeetings = null;
                var colleagueId = CurrentUser.Colleague.ColleagueId; // If Currently a manager is logged in - use their ColleagueId

                // diff method called based on myOrTeams = "MyMeetings" or "TeamMeetings"
                if (myOrTeams == "TeamMeetings" && CurrentUser.IsLineManager)
                {
                    var meetings = facade.GetTeamView(colleagueId); // Get Meetings for the Manager
                    if (meetings!= null) teamMeetings = meetings.ToList();
                }
                else
                {
                    teamMeetings = new List<TeamView>();
                    TeamView mymeeting = facade.GetMyMeetingsView(colleagueId) ?? new TeamView();
                    teamMeetings.Add(mymeeting);
                }
                jsonData = teamMeetings ?? (object) "Error";
            }

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = jsonData // will be "Error" if requesting "TeamMeeting" and not LineManager
            };
            return jsonResult;
        }

        // GET: /Team/LinkMeetings
        // ## TEAM meetings view
        [HttpGet]
        [Authorize]
        [LinkAuthorizeManager] // IMPORTANT Check
        public ActionResult Meetings()
        {
            TempData["tabName"] = "team";
            TempData["ViewType"] = "TeamMeetings";
            return View("LinkMeetings");
        }

        // GET: /Team  // TODO retire this to use LinkMeetings method
        [HttpGet]
        [Authorize]
        [LinkAuthorizeManager] // IMPORTANT Check
        public ActionResult Index()
        {
            TempData["tabName"] = "team";
            ViewBag.Title = "Team";
            var managerId = CurrentUser.Colleague.ColleagueId; // Current manager is logged in - so use their ColleagueId 

            return Meetings(); // use the new action method

            using (var facade = new LinkServiceFacade())
            {
                var team = facade.GetTeamView(managerId);
                return team == null ? View() : View(team.ToList());
            }
        }
    }
}
