using System.Web.Http;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class MeetingsController : BaseController
    {

        [HttpGet] //api/Meetings/10
        public IHttpActionResult GetMeeting(int id)
        {
            var meeting = _db.GetMeeting(id);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        [HttpGet] //newMeeting/E001
        [Route("newmeeting/{colleagueId}")]
        public IHttpActionResult CreateMeeting(string colleagueId)
        {
            var meeting = _db.CreateMeeting(colleagueId);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        [HttpPost] // POST: api/Meetings
        public IHttpActionResult SaveMeeting(MeetingView meetingView )
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            _db.SaveMeeting(meetingView);

            return CreatedAtRoute("api/answers", new {id=meetingView.MeetingId}, meetingView);
        }

        [HttpPut] // PUT: api/Meetings/5
        public IHttpActionResult UpdateMeeting(int id, MeetingView meetingView)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _db.UpdateMeeting(id,meetingView);

            return Ok();
        }
    }
}
