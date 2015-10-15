using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using JsPlc.Ssc.Link.Portal.Security;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class TeamController : LinkBaseController
    {
        // GET: Team/Create
        [HttpGet]
        [Authorize]
        [LinkAuthorizeManager]
        public ActionResult Index()
        {
            ViewBag.Title = "Team";
            var managerId = CurrentUser.Colleague.ColleagueId; // Current manager is logged in - so use their ColleagueId 
            using (var facade = new LinkServiceFacade())
            {
                var team = facade.GetTeamView(managerId);
                return team == null ? View() : View(team.ToList());
            }
        }
    }
}
