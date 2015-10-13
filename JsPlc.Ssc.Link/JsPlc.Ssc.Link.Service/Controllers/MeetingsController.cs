using System.Web.Http;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class MeetingsController : BaseController
    {

       //GET: /api/Meetings/10
        [HttpGet]
        public IHttpActionResult GetMeeting(int Id)
        {
            var meeting = _db.GetMeeting(Id);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        //GET: /api/Meetings/10
        [HttpGet]
        public IHttpActionResult GetMeetings(string employeeId)
        {
            return Ok(_db.GetMeetings(employeeId));
        }

        //GET: /api/Meetings/?employeeId=1
        [HttpGet]
        
        public IHttpActionResult CreateMeeting([FromUri]string employeeId, int periodId)
        {
            var meeting = _db.CreateMeeting(employeeId, periodId);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        // POST: api/Meetings
        [HttpPost]
        public IHttpActionResult SaveMeeting(MeetingView meetingView )
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            return Ok(_db.SaveMeeting(meetingView));
            //return CreatedAtRoute("api/answers", meeting, meeting);
        }

        // PUT: api/Meetings/5
        [HttpPut]
        public IHttpActionResult UpdateMeeting(int id, [FromBody]MeetingView meetingView)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _db.UpdateMeeting(id,meetingView);
        }
    }
}
