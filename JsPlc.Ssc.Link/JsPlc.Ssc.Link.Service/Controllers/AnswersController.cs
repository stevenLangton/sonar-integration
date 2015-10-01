using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class AnswersController : BaseController
    {

        ////GET: http://localhost:35915/api/answers/?periodid=1&employeeid=10
        //[HttpGet]
        //public IHttpActionResult GetAnswers([FromUri]AnswerParams parms)
        //{
        //    if (parms.PeriodId == 0 || parms.EmployeeId == 0)
        //        return BadRequest();

        //     return Ok(_db.GetAnswers(parms));
        // }
        [HttpGet]
        public string Get()
        {
            return "value";
        }
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }
        //GET: http://localhost:35915/api/answers/?meetingId=10
        [HttpGet]
        public IHttpActionResult GetAnswers([FromUri]int meetingId)
        {
            return Ok(_db.GetAnswers(meetingId));
        }

        // POST: api/Answers
        [HttpPost]
        public IHttpActionResult Post(LinkMeeting meeting )
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            _db.SaveMeeting(meeting);
            return Ok();
            //return CreatedAtRoute("api/answers", meeting, meeting);
        }

        // PUT: api/Answers/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Answers/5
        public void Delete(int id)
        {
        }
    }
}
