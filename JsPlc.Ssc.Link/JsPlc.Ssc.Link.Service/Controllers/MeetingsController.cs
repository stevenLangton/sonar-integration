using System.Web.Http;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class MeetingsController : BaseController
    {

       //GET :
        [HttpGet]
        public IHttpActionResult GetMeetings([FromUri] int employeeId)
        {
            return null;
        }

        //GET: /api/Meetings/10
        [HttpGet]
        public IHttpActionResult GetMeeting(int Id)
        {
            return Ok(_db.GetMeeting(Id));
        }

        // POST: api/Meetings
        [HttpPost]
        public IHttpActionResult Post(LinkMeeting meeting )
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            _db.SaveMeeting(meeting);
            return Ok();
            //return CreatedAtRoute("api/answers", meeting, meeting);
        }

        // PUT: api/Meetings/5
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
