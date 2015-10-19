using System.Web.Http;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class QuestionsController : BaseController
    {
        public QuestionsController() { }

        public QuestionsController(ILinkRepository repository):base(repository) { }

        [HttpGet] // api/questions/
        public IHttpActionResult GetQuestions()
        {
            return Ok(_db.GetQuestions());
        }
    }
}
