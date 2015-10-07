using System.Web.Http;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class QuestionsController : BaseController
    {
        
        //GET /api/questions/?periodid=1
        [HttpGet]
        public IHttpActionResult GetQuestions([FromUri]int periodId)
        {
            if(periodId==0)
                return BadRequest();

            return Ok(_db.GetQuestions(periodId));
        }
    }
}
