using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class SecurityController : BaseController
    {
        public SecurityController() { }

        public SecurityController(ILinkRepository repository) : base(repository) { }

        public SecurityController(IMeetingService repository) : base(repository) { }

        /// <summary>
        /// Check meeting access : api/HasMeetingAccess/5/E001
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="colleagueId"></param>
        /// <returns></returns>
        [HttpGet] 
        [Route("api/HasMeetingAccess/{meetingId}/{colleagueId}")]
        public IHttpActionResult HasMeetingAccess(int meetingId, string colleagueId)
        {
            var meeting = _dbMeeting.GetMeeting(meetingId);
            var colleague = _dbColleagues.GetColleague(colleagueId);
            var directReports = _dbColleagues.GetDirectReports(colleagueId);
            if (meeting == null || colleague == null)
                return NotFound();
            // Either you are colleague of meeting OR manager of meeting 
            // OR the meeting.ColleagueId matches on of your directReports, you have access..
            if (meeting.ColleagueId.Equals(colleagueId) || meeting.ManagerId.Equals(colleagueId)
                || (directReports!= null && directReports.Any(x => x.ColleagueId.Equals(meeting.ColleagueId))))
            {
                return Ok(true);
            }

            return Ok(false);
        }

    }
}
