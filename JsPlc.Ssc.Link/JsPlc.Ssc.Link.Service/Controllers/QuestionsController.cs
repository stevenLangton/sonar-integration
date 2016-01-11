using System.Web.Http;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class QuestionsController : BaseController
    {
        public QuestionsController() { }

        public QuestionsController(ILinkRepository repository):base(repository) { }

        [HttpGet] // api/questions/
        public IHttpActionResult GetQuestions()
        {
            return Ok(_dbMeeting.GetQuestions());
        }
    }
}
