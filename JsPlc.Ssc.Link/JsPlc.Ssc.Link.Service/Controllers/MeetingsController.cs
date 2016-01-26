using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class MeetingsController : BaseController
    {
        public MeetingsController() { }

        public MeetingsController(ILinkRepository repository) : base(repository) { }

        public MeetingsController(IMeetingService repository) : base(repository) { }

        [HttpGet] //api/Meetings/10
        public IHttpActionResult GetMeeting(int id)
        {
            var meeting = _dbMeeting.GetMeeting(id);

            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(meeting);
        }

        [HttpGet] //newMeeting/E001
        [Route("newmeeting/{colleagueId}")]
        public IHttpActionResult CreateMeeting(string colleagueId)
        {
            var meeting = _dbMeeting.CreateMeeting(colleagueId);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        [HttpPost] // POST: api/Meetings
        public IHttpActionResult SaveMeeting(MeetingView meetingView )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_dbMeeting.SaveMeeting(meetingView));

            //return CreatedAtRoute("api/meetings", new {id=meetingView.MeetingId}, meetingView);
        }

        [HttpPut] // PUT: api/Meetings/5
        public IHttpActionResult UpdateMeeting( MeetingView meetingView)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _dbMeeting.UpdateMeeting(meetingView);

            return Ok();
        }

        [HttpGet]
        [Route("mymeetings/{colleagueId}")] // mymeetings/E001
        public IHttpActionResult GetMyMeetings(string colleagueId)
        {
            var meetings = _dbMeeting.GetMeetings(colleagueId);

            if (meetings == null)
                return NotFound();

            return Ok(meetings);
        }
        [HttpGet]
        [Route("myteam/{managerId}")] // myteam/E0010
        public IHttpActionResult GetMyTeam(string managerId)
        {
            IEnumerable<ColleagueTeamView> colleaguesAndMeetings = _dbMeeting.GetTeam(managerId);

            var teamViews = colleaguesAndMeetings as IList<ColleagueTeamView> ?? colleaguesAndMeetings.ToList();

            if (!teamViews.Any())
                return NotFound();

            return Ok(teamViews);
        }
    }
}
