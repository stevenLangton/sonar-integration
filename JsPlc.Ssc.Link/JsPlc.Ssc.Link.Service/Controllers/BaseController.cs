using System.Web.Http;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;
using JsPlc.Ssc.Link.Service.Services;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class BaseController : ApiController
    {
        //protected readonly ILinkRepository _db;
        protected readonly IMeetingService _dbMeeting;
        protected readonly IObjectivesService _dbObjectives;
        protected readonly IColleagueService _dbColleagues;
        protected readonly IPdpService _dbPdp;
        protected readonly IColleaguePdpService _dbColleaguePdp;
        protected readonly IConfigurationDataService _configurationDataService;
        protected readonly IDomainTranslationService _domainTranslationService;

        public BaseController()
        {
            _configurationDataService = new ConfigurationDataService();
            _domainTranslationService = new DomainTranslationService(_configurationDataService);

            //_db = new LinkRepository(new RepositoryContext());
            _dbMeeting = new MeetingService(new RepositoryContext(), new ColleagueService(new StubServiceFacade()));
            _dbObjectives = new ObjectivesService(new RepositoryContext());
            _dbColleagues = new ColleagueService(new StubServiceFacade());
            _dbPdp = new PdpService(new RepositoryContext());
            _dbColleaguePdp = new ColleaguePdpService(new RepositoryContext(), _configurationDataService);
        }

        public BaseController(//ILinkRepository linkRepository=null,
            IMeetingService meetingService = null, IObjectivesService objectivesService = null,
            IColleagueService colleagueService = null, IPdpService pdpService = null,
            IConfigurationDataService configurationDataService = null,
            IDomainTranslationService domainTranslationService = null, IColleaguePdpService dbColleaguePdp = null)
        {
            _configurationDataService = configurationDataService;
            _domainTranslationService = domainTranslationService;

            //_db = linkRepository;
            _dbMeeting = meetingService;
            _dbObjectives = objectivesService;
            _dbColleagues = colleagueService;
            _dbPdp = pdpService;
            _dbColleaguePdp = dbColleaguePdp;
        }

        //public BaseController(ILinkRepository repository)
        //{
        //    _db = repository;
        //}

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

        public BaseController(IPdpService repoPdp)
        {
            _dbPdp = repoPdp;
        }

        public BaseController(IColleaguePdpService repoColleaguePdp)
        {
            _dbColleaguePdp = repoColleaguePdp;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_db.Dispose();
                _dbMeeting.Dispose();
                _dbObjectives.Dispose();
                _dbPdp.Dispose();
                _dbColleaguePdp.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
