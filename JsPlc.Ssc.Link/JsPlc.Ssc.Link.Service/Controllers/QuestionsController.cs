using System.Web.Http;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class QuestionsController : BaseController
    {
        
        //GET /api/questions/?periodid=1
        [HttpGet]
        public IHttpActionResult GetQuestions()
        {
            return Ok(_db.GetQuestions());
        }
    }
}
