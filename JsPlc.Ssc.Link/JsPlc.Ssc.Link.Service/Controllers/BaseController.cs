using System.Web.Http;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly ILinkRepository _db;
        protected readonly IMeeting _dbMeeting;
        protected readonly IObjectives _dbObjectives;

        public BaseController()
        {
            _db = new Repository.LinkRepository(new RepositoryContext());
            _dbMeeting = new Repository.Meeting(new RepositoryContext());
            _dbObjectives = new Repository.LinkObjectives(new RepositoryContext());
        }

        public BaseController(ILinkRepository repository)
        {
            _db = repository;
        }

        public BaseController(IMeeting repoMeeting)
        {
            _dbMeeting = repoMeeting;
        }

        public BaseController(IObjectives repoObjectives)
        {
            _dbObjectives = repoObjectives;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
                _dbMeeting.Dispose();
                _dbObjectives.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
