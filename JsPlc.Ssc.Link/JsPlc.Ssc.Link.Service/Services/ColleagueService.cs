using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using System.Web.Configuration;

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
            coll.EmailAddress = whatDomain(coll.EmailAddress);
            return coll;
        }

        ColleagueView IColleagueService.GetColleagueByEmail(string emailAddress)
        {
            var coll = _svc.GetColleagueByEmail(emailAddress);
            coll.EmailAddress = whatDomain(coll.EmailAddress);
            return coll;
        }

        List<ColleagueView> IColleagueService.GetDirectReports(string managerId)
        {
            IEnumerable<ColleagueView> coll = _svc.GetDirectReports(managerId);
            foreach (var item in coll)
            {
                item.EmailAddress = whatDomain(item.EmailAddress);
            }
            return coll.ToList();
        }

        List<ColleagueView> IColleagueService.GetDirectReportsByManagerEmail(string emailAddress)
        {
            IEnumerable<ColleagueView> coll = _svc.GetDirectReportsByManagerEmail(emailAddress);
            foreach (var item in coll)
            {
                item.EmailAddress = whatDomain(item.EmailAddress);
            }
            return coll.ToList();
        }

        bool IColleagueService.IsManager(string colleagueId)
        {
            IEnumerable<ColleagueView> coll = _svc.GetDirectReports(colleagueId);
            return (coll != null) && coll.Any();
        }

        bool IColleagueService.IsManagerByEmail(string email)
        {
            IEnumerable<ColleagueView> coll = _svc.GetDirectReportsByManagerEmail(email);
            return (coll != null) && coll.Any();
        }

        void IColleagueService.Dispose()
        {
            _svc.Dispose();
        }

        //used to replcae domian so intailization data works for dev, test and live
        private string whatDomain(string colleagueEmail)
        {
            //domain to use
            string actualDomian = WebConfigurationManager.AppSettings["LinkDomain"];
            
            //old intailization
            if(colleagueEmail.Contains("@linktool.onmicrosoft.com"))
            {
               colleagueEmail = colleagueEmail.Replace("@linktool.onmicrosoft.com", actualDomian);
            }
            //new intailization
            if(colleagueEmail.Contains("@domain.com"))
            {
               colleagueEmail = colleagueEmail.Replace("@domain.com", actualDomian);
            }

            return colleagueEmail;
        }
    }


}
