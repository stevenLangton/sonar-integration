using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using System.Web.Configuration;
using Microsoft.Ajax.Utilities;

namespace JsPlc.Ssc.Link.Service.Services
{
    public class ColleagueService : IColleagueService
    {
        private readonly IStubServiceFacade _svc;
        private readonly IDomainTranslationService _domainTranslationService;

        public ColleagueService() { }

        public ColleagueService(IStubServiceFacade svc)
        {
            _svc = svc; 
            IConfigurationDataService configurationDataService = new ConfigurationDataService();
            _domainTranslationService = new DomainTranslationService(configurationDataService);
        }

        public ColleagueService(IStubServiceFacade svc, IDomainTranslationService domainTranslationService)
        {
            _svc = svc;
            _domainTranslationService = domainTranslationService;
        }

        /// <summary>
        /// Get ColleagueProfile
        /// </summary>
        /// <param name="colleagueId"></param>
        /// <returns></returns>
        ColleagueView IColleagueService.GetColleague(string colleagueId)
        {
            var coll = _svc.GetColleague(colleagueId);
            // Dont replace outgoing values..
            // coll.EmailAddress = AdDomainToDbDomain(coll.EmailAddress);
            return coll;
        }

        ColleagueView IColleagueService.GetColleagueByEmail(string emailAddress)
        {
            emailAddress = _domainTranslationService.AdDomainToDbDomain(emailAddress);

            var coll = _svc.GetColleagueByEmail(emailAddress);
            //coll.EmailAddress = AdDomainToDbDomain(coll.EmailAddress);
            return coll;
        }

        List<ColleagueView> IColleagueService.GetDirectReports(string managerId)
        {
            IEnumerable<ColleagueView> coll = _svc.GetDirectReports(managerId);
            //if (coll == null) return null;

            var colleagues = coll as ColleagueView[] ?? coll.ToArray();
            foreach (var item in colleagues)
            {
                item.EmailAddress = _domainTranslationService.AdDomainToDbDomain(item.EmailAddress);
            }
            return colleagues.ToList();
        }

        List<ColleagueView> IColleagueService.GetDirectReportsByManagerEmail(string emailAddress)
        {
            emailAddress = _domainTranslationService.AdDomainToDbDomain(emailAddress);

            IEnumerable<ColleagueView> coll = _svc.GetDirectReportsByManagerEmail(emailAddress);
            //if (coll == null) return null;

            var colleagues = coll as ColleagueView[] ?? coll.ToArray();
            //foreach (var item in colleagues)
            //{
            //    item.EmailAddress = AdDomainToDbDomain(item.EmailAddress);
            //}
            return colleagues.ToList();
        }

        bool IColleagueService.IsManager(string colleagueId)
        {
            IEnumerable<ColleagueView> coll = _svc.GetDirectReports(colleagueId);
            return (coll != null) && coll.Any();
        }

        bool IColleagueService.IsManagerByEmail(string emailAddress)
        {
            emailAddress = _domainTranslationService.AdDomainToDbDomain(emailAddress);
            IEnumerable<ColleagueView> coll = _svc.GetDirectReportsByManagerEmail(emailAddress);
            return (coll != null) && coll.Any();
        }

        void IColleagueService.Dispose()
        {
            _svc.Dispose();
        }

    }


}
