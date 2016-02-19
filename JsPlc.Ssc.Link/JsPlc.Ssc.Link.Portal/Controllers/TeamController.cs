using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Services;
using iTextSharp.text;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using JsPlc.Ssc.Link.Portal.Security;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Asn1.Crmf;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class TeamController : LinkBaseController
    {
        // ## Knockout VIEWS - Sprint 2: 
        // Team/LinkMeetings.cshtml and Team/_ColleagueMeetingHistory.cshtml

        // ## Mockups:
        // /LinkForm/MyTeam.cshtml and Linkform/_ListMeetingsPartial.cshtml

        [HttpGet]
        [Authorize]
        public JsonResult GetColleagueMeetings(string ColleagueId) // Another method in LinkForm for MyMeetings
        {
            object jsonData;

            List<ColleagueTeamView> teamMeetings = new List<ColleagueTeamView>();

            ColleagueTeamView mymeeting = ServiceFacade.GetMyMeetingsView(ColleagueId) ?? new ColleagueTeamView();
            mymeeting = AssignMeetingsByDate(mymeeting);
            teamMeetings.Add(mymeeting);
            jsonData = teamMeetings ?? (object)"Error";

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = jsonData // will be "Error" if requesting "TeamMeeting" and not LineManager
            };
            return jsonResult;
        }

        [ScriptMethod(UseHttpGet = true)]
        [HttpGet]
        [Authorize]
        //[LinkAuthorizeManager] // IMPORTANT Check removed as this method is common for LinkMeetings view
        public JsonResult GetMeetings(string myOrTeams) // Another method in LinkForm for MyMeetings
        {
            object jsonData;

            List<ColleagueTeamView> teamMeetings = null;
            var colleagueId = CurrentUser.Colleague.ColleagueId; // If Currently a manager is logged in - use their ColleagueId

            // diff method called based on myOrTeams = "MyMeetings" or "TeamMeetings"
            if (myOrTeams == "TeamMeetings" && CurrentUser.IsLineManager)
            {
                var meetings = ServiceFacade.GetTeamView(colleagueId); // Get Meetings for the Manager
                if (meetings!= null) teamMeetings = meetings.ToList();
            }
            else
            {
                teamMeetings = new List<ColleagueTeamView>();
                ColleagueTeamView mymeeting = ServiceFacade.GetMyMeetingsView(colleagueId) ?? new ColleagueTeamView();
                mymeeting = AssignMeetingsByDate(mymeeting);
                teamMeetings.Add(mymeeting);
            }
            jsonData = teamMeetings ?? (object) "Error";

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = jsonData // will be "Error" if requesting "TeamMeeting" and not LineManager
            };
            return jsonResult;
        }

        /// <summary>
        /// Gets a flattened list of Past meetings for the entire team (sorted - nearest then oldest..)
        /// </summary>
        /// <param name="managerId">Optional param - maybe useful for unit testing. If blank uses CurretnUser details</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [LinkAuthorizeManager] // IMPORTANT Check - only manager screen will call this..
        public JsonResult GetTeamsMeetingRecords(string managerId="")
        {
            object jsonData;

            // Logged in user is manager
            if (managerId.IsNullOrWhiteSpace()) 
                managerId = CurrentUser.Colleague.ColleagueId;

            if (!managerId.Equals(CurrentUser.Colleague.ColleagueId))
                // Protect from trying to get data for another line manager
            {
                jsonData = "AccessDenied";
            }
            else
            {
                var teamView = ServiceFacade.GetTeamView(managerId); // ONE facade call..
                var pastTeamMeetings = new List<LinkMeetingView>();
                if (teamView != null)
                {
                    foreach (var colleague in teamView)
                    {
                        var coll = AssignMeetingsByDate(colleague); // very quick assignment on portal side
                        if (coll.PastMeetings != null)
                        {
                            pastTeamMeetings.AddRange(coll.PastMeetings);
                        }
                    }
                    if (pastTeamMeetings.Any())
                    {
                        pastTeamMeetings = pastTeamMeetings.OrderByDescending(x => x.MeetingDate).ToList();
                    }
                }
                jsonData = pastTeamMeetings;
            }

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = jsonData
            };
            return jsonResult;
        }


        public static ColleagueTeamView AssignMeetingsByDate(ColleagueTeamView mymeeting)
        {
            if (mymeeting.Meetings != null && mymeeting.Meetings.Any())
            {
                List<LinkMeetingView> meetings = mymeeting.Meetings;
                mymeeting.PastMeetings = meetings
                            .Where(x => DateTime.Now.Subtract(x.MeetingDate).TotalDays / (365.25 / 12) <= 12
                            && DateTime.Now.Subtract(x.MeetingDate).TotalHours >= 0)
                            .OrderByDescending(x => x.MeetingDate)
                            .ToList();
                mymeeting.MeetingsInLast12Months = mymeeting.PastMeetings.Count();
                mymeeting.LastMeeting = mymeeting.PastMeetings.FirstOrDefault();
                mymeeting.LastInCompleteMeeting = mymeeting.PastMeetings
                        .FirstOrDefault(x => x.Status == MeetingStatus.InComplete);

                mymeeting.UpcomingMeetings = meetings
                    .Where(x => x.MeetingDate >= DateTime.Now)
                    .OrderBy(x => x.MeetingDate)
                    .ToList();
                mymeeting.LatestMeeting = mymeeting.UpcomingMeetings.FirstOrDefault();
            }
            return mymeeting;
        }

        // GET: /Team/LinkMeetings
        // ## TEAM view (old name Meetings)
        [HttpGet]
        [Authorize]
        [LinkAuthorizeManager] // IMPORTANT Check
        public ActionResult YourTeam()
        {
            TempData["tabName"] = "team";
            ViewBag.ViewType = "team";
            TempData["ViewType"] = "TeamMeetings";
            return View("YourTeam");
        }

        // GET: /Team/MeetingRecords
        // ## Meeting records view
        [HttpGet]
        [Authorize]
        [LinkAuthorizeManager] // IMPORTANT Check
        public ActionResult MeetingRecords()
        {
            TempData["tabName"] = "meetingRecords";
            return View();
        }

        // GET: /Team  // Calls LinkMeetings method
        [HttpGet]
        [Authorize]
        [LinkAuthorizeManager] // IMPORTANT Check
        public ActionResult Index()
        {
            TempData["tabName"] = "team";
            ViewBag.Title = "Team";
            var managerId = CurrentUser.Colleague.ColleagueId; // Current manager is logged in - so use their ColleagueId 

            return YourTeam(); // use the new action method
        }

        //public ActionResult MyTeam_mockup()
        //{
        //    return View();
        //}
    }
}
