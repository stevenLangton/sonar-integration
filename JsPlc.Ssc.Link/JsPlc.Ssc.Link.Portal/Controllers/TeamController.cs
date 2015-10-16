using System.Linq;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using JsPlc.Ssc.Link.Portal.Security;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class TeamController : LinkBaseController
    {
        // GET: Team/Create
        [HttpGet]
        [Authorize]
        [LinkAuthorizeManager] // IMPORTANT Check
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
