using System.Web.Http;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;
using JsPlc.Ssc.Link.Service.Services;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly ILinkRepository _db;
        protected readonly IMeetingService _dbMeeting;
        protected readonly IObjectivesService _dbObjectives;
        protected readonly IColleagueService _dbColleagues;

        public BaseController()
        {
            _db = new LinkRepository(new RepositoryContext());
            _dbMeeting = new MeetingService(new RepositoryContext(), new ColleagueService(new ServiceFacade()));
            _dbObjectives = new ObjectivesService(new RepositoryContext());
            _dbColleagues = new ColleagueService(new ServiceFacade());
        }

        public BaseController(ILinkRepository repository)
        {
            _db = repository;
        }

        public BaseController(IMeetingService repoMeeting)
        {
            _dbMeeting = repoMeeting;
        }

        public BaseController(IObjectivesService repoObjectives)
        {
            _dbObjectives = repoObjectives;
        }

        public BaseController(IColleagueService repoColleagues)
        {
            _dbColleagues = repoColleagues;
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
