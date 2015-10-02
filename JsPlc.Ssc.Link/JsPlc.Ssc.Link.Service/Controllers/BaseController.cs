using System.Web.Http;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class BaseController : ApiController
    {
        protected LinkRepository _db;

        public BaseController()
        {

            _db=new LinkRepository(new RepositoryContext());
        }

        //public BaseController(RepositoryContext dbContext)
        //{
        //    _db = dbContext;
        //}

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
