using System.Web.Http;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class QuestionsController : BaseController
    {
        //GET http://localhost:35915/api/questions/?periodid=1
        public IHttpActionResult GetQuestions([FromUri]int periodId)
        {
            if(periodId==0)
                return BadRequest();

            return Ok(_db.GetQuestions(periodId));
        }

        // GET: http://localhost:35915/api/questions/?periodid=1&employeeid=1
       //[HttpGet]
       //public IHttpActionResult GetQuestions([FromUri]AnswerParams parms)
       //{
       //    if (parms.PeriodId == 0 || parms.EmployeeId == 0)
       //        return BadRequest();

       //     return Ok(_db.GetQuestions(parms));
       // }

        // POST: api/Questions
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Questions/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Questions/5
        public void Delete(int id)
        {
        }
    }
}
