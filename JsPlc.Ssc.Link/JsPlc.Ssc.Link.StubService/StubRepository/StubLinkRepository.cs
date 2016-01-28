using System;
using System.Collections.Generic;
using System.Linq;
using JsPlc.Ssc.Link.StubService.Extensions;
using JsPlc.Ssc.Link.StubService.StubModels;
using JsPlc.Ssc.Link.StubService.StubInterfaces;
using Microsoft.Owin.Security.Provider;
using WebGrease.Css.Extensions;

namespace JsPlc.Ssc.Link.StubService.StubRepository
{
    public class ColleagueServices:IColleagueServices
    {
        private readonly StubRepositoryContext _db;

        public ColleagueServices() { }

        public ColleagueServices(StubRepositoryContext context) { _db = context; }

        ColleagueDto IColleagueServices.GetColleague(string colleagueId)
        {
            var colleague = _db.Colleagues.FirstOrDefault(e =>e.ColleagueId.ToLower().Equals(colleagueId.ToLower()));
            if (colleague == null) return null;
            var mgr = _db.Colleagues.FirstOrDefault(x => x.ColleagueId.Equals(colleague.ManagerId));
            var retval = colleague.ToColleagueDto(mgr);
            //retval.Manager = mgr.ToColleagueDto(mgr);
            return retval;
        }

        ColleagueDto IColleagueServices.GetColleagueByEmail(string emailAddress)
        {
            var colleague = _db.Colleagues.FirstOrDefault(e => e.EmailAddress.ToLower().Equals(emailAddress.ToLower()));
            StubColleague mgr = null;
            if (colleague != null)
            {
                mgr = _db.Colleagues.FirstOrDefault(x => x.ColleagueId.Equals(colleague.ManagerId));
            }
            var retval = colleague.ToColleagueDto(mgr);
            //retval.Manager = mgr.ToColleagueDto();
            return retval;
        }

        List<ColleagueDto> IColleagueServices.GetDirectReports(string managerId)
        {
            return GetDirectReports(managerId);
        }

        List<ColleagueDto> IColleagueServices.GetDirectReportsByManagerEmail(string managerEmail)
        {
            return GetDirectReportsByManagerEmail(managerEmail);
        }

        bool IColleagueServices.IsManager(string colleagueId)
        {
            var dr = GetDirectReports(colleagueId);
            return dr.Any();
        }

        bool IColleagueServices.IsManagerByEmail(string email)
        {
            var dr = GetDirectReportsByManagerEmail(email);
            return dr != null && dr.Any();
        }

        private List<ColleagueDto> GetDirectReports(string managerId)
        {
            var mgr = _db.Colleagues.FirstOrDefault(x => x.ColleagueId.Equals(managerId));
            if (mgr == null) return null;
            var colleagueList = _db.Colleagues.Where(x => x.ManagerId.Equals(mgr.ColleagueId));
            return colleagueList.Any() ? colleagueList.ToList().ToColleagueDtoList(mgr) : null;
        }

        private List<ColleagueDto> GetDirectReportsByManagerEmail(string managerEmail)
        {
            var mgr = _db.Colleagues.FirstOrDefault(x => x.EmailAddress.Equals(managerEmail));
            if (mgr == null) return null;

            var colleagueList = _db.Colleagues.Where(x => x.ManagerId.Equals(mgr.ColleagueId));
            return colleagueList.Any() ? colleagueList.ToList().ToColleagueDtoList(mgr) : null;
        }

        void IColleagueServices.Dispose()
        {
            _db.Dispose();
        }
    }
}
