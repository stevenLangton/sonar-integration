using System.Web.Http;
using JsPlc.Ssc.Link.StubService.StubRepository;
using JsPlc.Ssc.Link.StubService.StubInterfaces;
using JsPlc.Ssc.Link.StubService.StubInterfaces;

namespace JsPlc.Ssc.Link.StubService.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly IStubLinkRepository _db;

        public BaseController()
        {
            _db = new StubLinkRepository(new StubRepositoryContext());
        }

        public BaseController(IStubLinkRepository repository)
        {
            _db = repository;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
