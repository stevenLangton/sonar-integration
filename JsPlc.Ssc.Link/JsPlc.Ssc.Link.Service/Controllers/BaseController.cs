using System.Web.Http;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly ILinkRepository _db;

        public BaseController()
        {

            _db=new LinkRepository(new RepositoryContext());
        }

        public BaseController(ILinkRepository repository)
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
