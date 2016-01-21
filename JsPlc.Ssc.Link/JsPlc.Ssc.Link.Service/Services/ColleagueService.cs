using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;

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

            return coll;
        }

        ColleagueView IColleagueService.GetColleagueByEmail(string emailAddress)
        {
            var coll = _svc.GetColleagueByEmail(emailAddress);

            return coll;
        }

        List<ColleagueView> IColleagueService.GetDirectReports(string managerId)
        {
            IEnumerable<ColleagueView> coll = _svc.GetDirectReports(managerId);

            return coll.ToList();
        }

        List<ColleagueView> IColleagueService.GetDirectReportsByManagerEmail(string emailAddress)
        {
            IEnumerable<ColleagueView> coll = _svc.GetDirectReportsByManagerEmail(emailAddress);

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
    }


}
