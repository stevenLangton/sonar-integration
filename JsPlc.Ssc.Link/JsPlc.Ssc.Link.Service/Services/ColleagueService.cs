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
        private readonly ServiceFacade _svc;

        public ColleagueService() { }

        public ColleagueService(ServiceFacade svc) { _svc = svc; }

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
            emailAddress = AdDomainToDbDomain(emailAddress);

            var coll = _svc.GetColleagueByEmail(emailAddress);
            //coll.EmailAddress = AdDomainToDbDomain(coll.EmailAddress);
            return coll;
        }

        List<ColleagueView> IColleagueService.GetDirectReports(string managerId)
        {
            IEnumerable<ColleagueView> coll = _svc.GetDirectReports(managerId);
            if (coll == null) return null;

            var colleagues = coll as ColleagueView[] ?? coll.ToArray();
            foreach (var item in colleagues)
            {
                item.EmailAddress = AdDomainToDbDomain(item.EmailAddress);
            }
            return colleagues.ToList();
        }

        List<ColleagueView> IColleagueService.GetDirectReportsByManagerEmail(string emailAddress)
        {
            emailAddress = AdDomainToDbDomain(emailAddress);

            IEnumerable<ColleagueView> coll = _svc.GetDirectReportsByManagerEmail(emailAddress);
            if (coll == null) return null;

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
            emailAddress = AdDomainToDbDomain(emailAddress);
            IEnumerable<ColleagueView> coll = _svc.GetDirectReportsByManagerEmail(emailAddress);
            return (coll != null) && coll.Any();
        }

        void IColleagueService.Dispose()
        {
            _svc.Dispose();
        }

        /// <summary>
        /// Used to replace domain coming from Azure AD to that stored in DB for lookups to work in dev, test and live
        /// </summary>
        /// <param name="colleagueEmail"></param>
        /// <returns></returns>

        private string AdDomainToDbDomain(string colleagueEmail)
        {
            //domain to use
            string azureAdEmailDomain = WebConfigurationManager.AppSettings["AzureLinkDomain"];
            string dbLinkDomain = WebConfigurationManager.AppSettings["DbLinkDomain"];
            if (dbLinkDomain.IsNullOrWhiteSpace()) dbLinkDomain = "@linktool.onmicrosoft.com"; // use stubbed values

            // No need to touch anything if both domains match
            if (azureAdEmailDomain.Equals(dbLinkDomain)) return colleagueEmail;

            // Replacing Authenticated email domain with the DB Email domain...
            var inputString = colleagueEmail;
            string name = ""; 
            string domain = ""; 
            string[] parts = inputString.Split('@');
            if (parts.Length < 2) return colleagueEmail;
            name = parts[0];  
            domain = parts[1];

            if (!domain.Equals(dbLinkDomain))
            {
                colleagueEmail = string.Format("{0}{1}", name, dbLinkDomain);
            }
            return colleagueEmail;
        }
    }


}
