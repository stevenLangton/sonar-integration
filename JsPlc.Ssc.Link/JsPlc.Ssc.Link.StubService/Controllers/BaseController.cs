using System.Web.Http;
using JsPlc.Ssc.Link.StubService.StubRepository;
using JsPlc.Ssc.Link.StubService.StubInterfaces;

namespace JsPlc.Ssc.Link.StubService.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly IColleagueServices ColleagueServices;

        public BaseController()
        {
            ColleagueServices = new ColleagueServices(new StubRepositoryContext());
        }

        public BaseController(IColleagueServices repository)
        {
            ColleagueServices = repository;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ColleagueServices.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
