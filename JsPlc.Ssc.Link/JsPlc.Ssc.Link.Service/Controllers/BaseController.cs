using System.Web.Http;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly ILinkRepository _db;
        protected readonly IMeeting _dbMeeting;

        public BaseController()
        {
            _db = new Repository.LinkRepository(new RepositoryContext());
            _dbMeeting = new Repository.Meeting(new RepositoryContext());
        }

        public BaseController(ILinkRepository repository)
        {
            _db = repository;
        }

        public BaseController(IMeeting repoMeeting)
        {
            _dbMeeting = repoMeeting;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
                _dbMeeting.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
