using System.Web.Http;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class QuestionsController : BaseController
    {
        [HttpGet] // api/questions/
        public IHttpActionResult GetQuestions()
        {
            return Ok(_db.GetQuestions());
        }
    }
}
