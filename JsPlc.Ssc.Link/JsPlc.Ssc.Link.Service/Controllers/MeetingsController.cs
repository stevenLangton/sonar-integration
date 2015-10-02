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
        public IHttpActionResult GetMeetings(int employeeId)
        {
            return Ok(_db.GetMeetings(employeeId));
        }

        //GET: /api/Meetings/?employeeId=1
        [HttpGet]
        public IHttpActionResult CreateMeeting([FromUri]int employeeId, int periodId)
        {
            var meeting = _db.CreateMeeting(employeeId, periodId);

            if (meeting == null)
                return NotFound();

            return Ok(meeting);
        }

        // POST: api/Meetings
        [HttpPost]
        public IHttpActionResult SaveMeeting(LinkMeeting meeting )
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            _db.SaveMeeting(meeting);
            return Ok();
            //return CreatedAtRoute("api/answers", meeting, meeting);
        }

        // PUT: api/Meetings/5
        public void UpdateMeeting(int id, [FromBody]string value)
        {
        }
    }
}
